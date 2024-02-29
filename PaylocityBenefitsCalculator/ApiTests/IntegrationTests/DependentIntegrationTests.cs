using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using ApiTests.Extensions;
using FluentAssertions;
using Xunit;

namespace ApiTests.IntegrationTests;

public sealed class DependentIntegrationTests : IntegrationTest
{
    private readonly static List<GetDependentDto> Dependents = new()
    {
        new()
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        },
        new()
        {
            Id = 2,
            FirstName = "Child1",
            LastName = "Morant",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(2020, 6, 23)
        },
        new()
        {
            Id = 3,
            FirstName = "Child2",
            LastName = "Morant",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(2021, 5, 18)
        },
        new()
        {
            Id = 4,
            FirstName = "DP",
            LastName = "Jordan",
            Relationship = Relationship.DomesticPartner,
            DateOfBirth = new DateTime(1974, 1, 2)
        }
    };

    private async Task<GetEmployeeDto> CreateEmployeeForTest(CreateDependentDto? newDependent = null)
    {
        var dependents = new List<CreateDependentDto>();
        if (newDependent is not null)
            dependents.Add(newDependent);
        var newEmployee = new CreateEmployeeDto
        {
            DateOfBirth = DateTime.Today,
            Dependents = dependents,
            FirstName = "FN",
            LastName = "LN",
            Salary = decimal.MaxValue
        };
        var createResponse = await HttpClient.PostAsync("/api/v1/employees", newEmployee);
        var createdEmployeeResponse = await createResponse.Content.DeserializeTo<ApiResponse<GetEmployeeDto>>();
        var createdEmployee = createdEmployeeResponse!.Data!;
        return createdEmployee;
    }

    // GET
    [Fact]
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var expectedDependents = Dependents;

        var response = await HttpClient.GetAsync("/api/v1/dependents");

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        var actualDependentsResponse = await response.Content.DeserializeTo<ApiResponse<List<GetDependentDto>>>();
        var actualDependents = actualDependentsResponse!.Data!;
        foreach (var expectedDependent in expectedDependents)
        {
            var actualDependent = actualDependents.FirstOrDefault(a => a.Id == expectedDependent.Id);
            actualDependent.Should().NotBeNull().And.BeEquivalentTo(expectedDependent);
        }
    }

    [Fact]
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = Dependents.FirstOrDefault(d => d.Id == 1);
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    [Fact]
    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    // POST
    [Fact]
    public async Task Post_Should_CreateDependentIfValidationPasses()
    {
        var createdEmployee = await CreateEmployeeForTest();
        var dependent = new CreateDependentDto
        {
            DateOfBirth = DateTime.Today,
            EmployeeId = createdEmployee!.Id,
            FirstName = "FN",
            LastName = "LN",
            Relationship = Relationship.Child
        };

        var response = await HttpClient.PostAsync("/api/v1/dependents", dependent);

        await response.ShouldReturn(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_Should_NotCreateDependentIfValidationFails()
    {
        var createdEmployee = await CreateEmployeeForTest(new CreateDependentDto
        {
            DateOfBirth = DateTime.Today,
            EmployeeId = 0,
            FirstName = "FN",
            LastName = "LN",
            Relationship = Relationship.DomesticPartner
        });
        var dependent = new CreateDependentDto
        {
            DateOfBirth = DateTime.Today,
            EmployeeId = createdEmployee!.Id,
            FirstName = "FN",
            LastName = "LN",
            Relationship = Relationship.DomesticPartner
        };

        var createDependentResponse = await HttpClient.PostAsync("/api/v1/dependents", dependent);

        await createDependentResponse.ShouldReturn(HttpStatusCode.BadRequest);

        // ensure the employee doesn't have a new dependent associated with them
        var getEmployeeResponse = await HttpClient.GetAsync($"/api/v1/employees/{dependent.EmployeeId}");
        var employeeApiResponse = await getEmployeeResponse.Content.DeserializeTo<ApiResponse<GetEmployeeDto>>();
        employeeApiResponse!.Data!.Dependents.Count(d => d.Relationship == Relationship.Spouse || d.Relationship == Relationship.DomesticPartner).Should().Be(1);
    }
}
