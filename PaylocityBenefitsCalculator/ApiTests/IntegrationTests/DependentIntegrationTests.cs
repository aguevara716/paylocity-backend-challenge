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

public class DependentIntegrationTests : IntegrationTest
{
    // GET
    [Fact]
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        var dependents = new List<GetDependentDto>
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
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
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
        var dependent = new CreateDependentDto
        {
            DateOfBirth = DateTime.Today,
            EmployeeId = 1,
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
        var dependent = new CreateDependentDto
        {
            DateOfBirth = DateTime.Today,
            EmployeeId = 2,
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
