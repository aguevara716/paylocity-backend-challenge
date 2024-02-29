using Api.Models;

namespace Api.Services;

public sealed class AgeDeductionCalculator : PayrollAdjustmentCalculatorBase
{
    public override string Name => throw new NotImplementedException();

    protected override decimal InvokeCalculation(Employee employee)
    {
        // -$200 per month
        var charge = CalculateFromMonthlyCost(200m);
        return -1 * charge;
    }

    public override bool CanExecute(Employee employee)
    {
        var today = DateTime.Today;
        var age = today.Year - employee.DateOfBirth.Year;
        if (employee.DateOfBirth.Date > today.AddYears(-1 * age))
            age--;

        return age >= 50;
    }
}
