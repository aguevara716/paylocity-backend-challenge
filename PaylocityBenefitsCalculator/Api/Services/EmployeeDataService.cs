using Api.Models;
using Api.Repositories;

namespace Api.Services;

public interface IEmployeeDataService
{
    // create

    // read
    Employee[] Get();
    Employee? Get(int id);

    // update

    // delete
}

public sealed class EmployeeDataService : IEmployeeDataService
{
    private readonly IDependentRepository _dependentRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeDataService(IDependentRepository dependentRepository,
                               IEmployeeRepository employeeRepository)
    {
        _dependentRepository = dependentRepository;
        _employeeRepository = employeeRepository;
    }

    // create

    // read
    public Employee[] Get()
    {
        var employees = _employeeRepository.Get().ToArray();
        foreach (var employee in employees)
        {
            employee.Dependents = _dependentRepository.Get().Where(d => d.EmployeeId == employee.Id).ToList();
        }
        return employees;
    }

    public Employee? Get(int id)
    {
        var employee = _employeeRepository.Get(id);
        if (employee is null)
            return null;

        employee.Dependents = _dependentRepository.Get().Where(d => d.EmployeeId == employee.Id).ToList();
        return employee;
    }

    // update

    // delete
}

