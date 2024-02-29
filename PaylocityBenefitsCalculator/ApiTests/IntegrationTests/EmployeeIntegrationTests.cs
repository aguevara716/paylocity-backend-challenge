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

public sealed class EmployeeIntegrationTests : IntegrationTest
{
    private readonly static List<GetEmployeeDto> employees = new()
    {
        new()
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        },
        new()
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10),
            Dependents = new List<GetDependentDto>
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
                }
            }
        },
        new()
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17),
            Dependents = new List<GetDependentDto>
            {
                new()
                {
                    Id = 4,
                    FirstName = "DP",
                    LastName = "Jordan",
                    Relationship = Relationship.DomesticPartner,
                    DateOfBirth = new DateTime(1974, 1, 2)
                }
            }
        }
    };

    // GET
    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var expectedEmployees = employees;

        var response = await HttpClient.GetAsync("/api/v1/employees");

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        var actualEmployeesResponse = await response.Content.DeserializeTo<ApiResponse<List<GetEmployeeDto>>>();
        actualEmployeesResponse!.Success.Should().BeTrue();
        var actualEmployees = actualEmployeesResponse!.Data!;
        foreach (var expectedEmployee in expectedEmployees)
        {
            var actualEmployee = actualEmployees.FirstOrDefault(e => e.Id == expectedEmployee.Id);
            actualEmployee.Should().NotBeNull().And.BeEquivalentTo(expectedEmployee);
        }
    }

    [Fact]
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var employeeId = 3;
        var expectedEmployee = employees.FirstOrDefault(e => e.Id == employeeId);

        var response = await HttpClient.GetAsync($"/api/v1/employees/{employeeId}");

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        var actualEmployeeResponse = await response.Content.DeserializeTo<ApiResponse<Employee>>();
        var actualEmployee = actualEmployeeResponse!.Data!;
        actualEmployee.Should().NotBeNull().And.BeEquivalentTo(expectedEmployee);
    }

    [Fact]
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    // POST
    [Fact]
    public async Task Post_Should_CreateEmployeeIfValidationPasses()
    {
        var employee = new CreateEmployeeDto
        {
            DateOfBirth = DateTime.Today,
            Dependents = new List<CreateDependentDto>(),
            FirstName = "FN",
            LastName = "LN",
            Salary = 0m
        };

        var response = await HttpClient.PostAsync("/api/v1/employees", employee);

        await response.ShouldReturn(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(Relationship.DomesticPartner, Relationship.DomesticPartner)]
    [InlineData(Relationship.DomesticPartner, Relationship.Spouse)]
    [InlineData(Relationship.Spouse, Relationship.Spouse)]
    [InlineData(Relationship.Spouse, Relationship.DomesticPartner)]
    public async Task Post_Should_NotCreateEmployeeIfValidationFails(Relationship r1, Relationship r2)
    {
        var employee = new CreateEmployeeDto
        {
            DateOfBirth = DateTime.Today,
            Dependents = new List<CreateDependentDto>()
            {
                new()
                {
                    DateOfBirth = DateTime.Today,
                    FirstName = "FN",
                    LastName = "LN",
                    Relationship = r1
                },
                new()
                {
                    DateOfBirth = DateTime.Today,
                    FirstName = "FN",
                    LastName = "LN",
                    Relationship = r2
                }
            },
            FirstName = "FN",
            LastName = "LN",
            Salary = 0m
        };

        var response = await HttpClient.PostAsync("/api/v1/employees", employee);

        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }
}
