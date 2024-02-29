using Api.Models;
using Api.Repositories;

namespace Api.Services;

public interface IDataSeeder
{
    void SeedDatabase();
}

public sealed class DataSeeder : IDataSeeder
{
    private readonly BenefitsDbContext _context;

    public DataSeeder(BenefitsDbContext context)
    {
        _context = context;
    }

    private static Employee[] GetEmployees()
    {
        return new Employee[]
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75_420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92_365.22m,
                DateOfBirth = new DateTime(1999, 8, 10)
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143_211.12m,
                DateOfBirth = new DateTime(1963, 2, 17)
            }
        };
    }

    private static Dependent[] GetDependents()
    {
        return new Dependent[]
        {
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3),
                EmployeeId = 2
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23),
                EmployeeId = 2
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18),
                EmployeeId = 2
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2),
                EmployeeId = 3
            }
        };
    }

    public void SeedDatabase()
    {
        var employees = GetEmployees();
        _context.Employees.AddRange(employees);

        var dependents = GetDependents();
        _context.Dependents.AddRange(dependents);
    }
}
