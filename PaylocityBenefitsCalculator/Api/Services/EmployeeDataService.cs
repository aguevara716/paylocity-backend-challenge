using Api.Models;
using Api.Repositories;
using Api.Validators;

namespace Api.Services;

public interface IEmployeeDataService
{
    // create
    ValidationResult<Employee> Create(Employee newEmployee);

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
    private readonly IEmployeeValidator _employeeValidator;

    public EmployeeDataService(IDependentRepository dependentRepository,
                               IEmployeeRepository employeeRepository,
                               IEmployeeValidator employeeValidator)
    {
        _dependentRepository = dependentRepository;
        _employeeRepository = employeeRepository;
        _employeeValidator = employeeValidator;
    }

    // create
    public ValidationResult<Employee> Create(Employee newEmployee)
    {
        var validationResult = _employeeValidator.Validate(newEmployee);
        Console.WriteLine($"validation result: {validationResult.IsSuccess}, {validationResult.Error}");

        if (!validationResult.IsSuccess)
            return validationResult;

        _employeeRepository.Create(validationResult.Data!);
        foreach (var dependent in validationResult.Data!.Dependents)
        {
            dependent.EmployeeId = validationResult.Data!.Id;
            _dependentRepository.Create(dependent);
        }

        return validationResult;
    }

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

