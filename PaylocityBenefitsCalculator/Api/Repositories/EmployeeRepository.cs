using Api.Models;

namespace Api.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{

}

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly BenefitsDbContext _context;

    public EmployeeRepository(BenefitsDbContext context)
    {
        _context = context;
    }

    // create
    public Employee Create(Employee newEmployee)
    {
        var nextId = _context.Employees.Max(e => e.Id) + 1;
        newEmployee.Id = nextId;

        _context.Employees.Add(newEmployee);
        return newEmployee;
    }

    public IEnumerable<Employee> Create(IEnumerable<Employee> newEmployees)
    {
        foreach (var newEmployee in newEmployees)
            yield return Create(newEmployee);
    }

    // read
    public IQueryable<Employee> Get()
    {
        return _context.Employees.AsQueryable();
    }

    public Employee? Get(int id)
    {
        return _context.Employees.FirstOrDefault(e => e.Id == id);
    }

    // update

    // delete
}
