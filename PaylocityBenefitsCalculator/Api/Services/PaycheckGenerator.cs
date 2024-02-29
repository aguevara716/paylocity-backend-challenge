using Api.Models;

namespace Api.Services;

public interface IPaycheckGenerator
{
    ValidationResult<Paycheck> GeneratePaycheck(int checkId, int employeeId);
}

public sealed class PaycheckGenerator : IPaycheckGenerator
{
    private readonly IEmployeeDataService _employeeDataService;
	private readonly IPayrollAdjustmentCalculator[] _payrollAdjustmentCalculators;

    public PaycheckGenerator(IEmployeeDataService employeeDataService,
                             IPayrollAdjustmentCalculator[] payrollAdjustmentCalculators)
    {
        _employeeDataService = employeeDataService;
        _payrollAdjustmentCalculators = payrollAdjustmentCalculators;
    }

    private static DateOnly GetCheckDate(int checkId)
    {
        /*
         * 26 - 26 = 0 checks ago (so today)
         * 26 - 0 = 26 checks ago (so a year ago)
         * 
         * # checks ago * # days in a pay period = # days ago
         * checkdate = DateTime.AddDays(-1 * # days ago)
         */

        var numberOfDaysAgo = (PayPeriodSettings.CHECKS_PER_YEAR - checkId) * PayPeriodSettings.DAYS_PER_PAY_PERIOD;
        var checkDate = DateTime.Today.AddDays(-1 * numberOfDaysAgo);

        return DateOnly.FromDateTime(checkDate);
    }

    private IEnumerable<Adjustment> CalculateAdjustments(Employee employee)
    {
        foreach (var payrollAdjustmentCalculator in _payrollAdjustmentCalculators)
        {
            if (!payrollAdjustmentCalculator.CanExecute(employee))
                continue;

            var adjustment = new Adjustment
            {
                Amount = payrollAdjustmentCalculator.Execute(employee),
                Name = payrollAdjustmentCalculator.Name
            };
            yield return adjustment;
        }
    }

    public ValidationResult<Paycheck> GeneratePaycheck(int checkId, int employeeId)
    {
        if (checkId < 0)
            return ValidationResult<Paycheck>.GetFailure("Check ID cannot be negative");

        var checkDate = GetCheckDate(checkId);
        if (checkDate > DateOnly.FromDateTime(DateTime.Today))
            return ValidationResult<Paycheck>.GetFailure("Pay period hasn't occurred yet");

        var employee = _employeeDataService.Get(employeeId);
        if (employee is null)
            return ValidationResult<Paycheck>.GetFailure($"Employee with ID {employeeId} not found");

        var basePay = employee.Salary / PayPeriodSettings.CHECKS_PER_YEAR;
        var adjustments = CalculateAdjustments(employee);

        var paycheck = new Paycheck
        {
            Adjustments = adjustments.ToArray(),
            BasePay = basePay,
            CheckDate = checkDate,
            Id = checkId,
            Employee = employee
        };
        return ValidationResult<Paycheck>.GetSuccess(paycheck);
    }
}
