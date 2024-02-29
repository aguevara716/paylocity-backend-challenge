using Api.Models;

namespace Api.Services;

public sealed class DependentBenefitDeductionCalculator : PayrollAdjustmentCalculatorBase
{
    public override string Name => "Dependent Benefits";

    protected override decimal InvokeCalculation(Employee employee)
    {
        // -$600 per month, per dependent
        var baseCost = CalculateFromMonthlyCost(600m);
        var numberOfDependents = employee.Dependents.Count;
        var totalCost = -1 * baseCost * numberOfDependents;
        return totalCost;
    }

    public override bool IsEligible(Employee employee) => employee?.Dependents?.Any() ?? false;
}
