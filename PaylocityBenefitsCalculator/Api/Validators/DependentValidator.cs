using Api.Models;
using Api.Repositories;
using Api.Services;

namespace Api.Validators;

public interface IDependentValidator
{
	ValidationResult<Dependent> Validate(Dependent dependent);
}

public sealed class DependentValidator : IDependentValidator
{
    private readonly IEmployeeDataService _employeeDataService;
    private readonly IEmployeeValidator _employeeValidator;

    public DependentValidator(IEmployeeDataService employeeDataService,
                              IEmployeeValidator employeeValidator)
    {
        _employeeDataService = employeeDataService;
        _employeeValidator = employeeValidator;
    }

    public ValidationResult<Dependent> Validate(Dependent dependent)
    {
        if (dependent.EmployeeId == 0)
            return ValidationResult<Dependent>.GetFailure("Dependents must be associated with an employee");

        if (dependent.Relationship == Relationship.None)
            return ValidationResult<Dependent>.GetFailure("The relationship must be specified");

        var employee = _employeeDataService.Get(dependent.EmployeeId);
        if (employee is null)
            return ValidationResult<Dependent>.GetFailure($"An employee with ID {dependent.EmployeeId} does not exist");

        employee.Dependents.Add(dependent);
        var employeeValidationResult = _employeeValidator.Validate(employee);
        if (!employeeValidationResult.IsSuccess)
            return ValidationResult<Dependent>.GetFailure(employeeValidationResult.Error);

        return ValidationResult<Dependent>.GetSuccess(dependent);
    }
}
