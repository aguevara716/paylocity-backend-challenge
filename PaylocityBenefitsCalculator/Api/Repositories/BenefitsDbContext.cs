using Api.Models;

namespace Api.Repositories;

public sealed class BenefitsDbContext
{
    public List<Employee> Employees { get; set; } = new();
    public List<Dependent> Dependents { get; set; } = new();
}
