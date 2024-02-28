using Api.Extensions;
using Api.Models;

namespace Api.Validators;

public interface IEmployeeValidator
{
	ValidationResult<Employee> Validate(Employee employee);
}

public sealed class EmployeeValidator : IEmployeeValidator
{
    public ValidationResult<Employee> Validate(Employee employee)
    {
        if (employee is null)
            return ValidationResult<Employee>.GetFailure("Employee must be set");

        var numberOfSignificantOthers = employee.Dependents.Count(d => d.IsSignificantOther());
        if (numberOfSignificantOthers > 1)
            return ValidationResult<Employee>.GetFailure("Employee must only have 1 spouse or 1 domestic partner");

        return ValidationResult<Employee>.GetSuccess(employee);
    }
}
