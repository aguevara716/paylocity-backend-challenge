using Api.Models;

namespace Api.Services;

public sealed class HighSalaryDeductionCalculator : PayrollAdjustmentCalculatorBase
{
    public override string Name => "High Salary";

    protected override decimal InvokeCalculation(Employee employee)
    {
        // 2% of the employee's salary, annually
        var annualCost = employee.Salary * 0.02m;
        var costPerCheck = -1 * CalculateFromAnnualCost(annualCost);

        return costPerCheck;
    }

    public override bool CanExecute(Employee employee) => employee.Salary >= 80_000m;
}
