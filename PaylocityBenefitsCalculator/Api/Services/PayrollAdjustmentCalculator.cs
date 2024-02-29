using Api.Models;

namespace Api.Services;

public interface IPayrollAdjustmentCalculator
{
    string Name { get; }
    bool IsEligible(Employee employee);
    decimal Execute(Employee employee);
}

public abstract class PayrollAdjustmentCalculatorBase : IPayrollAdjustmentCalculator
{
    public abstract string Name { get; }

    public decimal Execute(Employee employee)
    {
        if (!IsEligible(employee))
            return 0m;

        return InvokeCalculation(employee);
    }

    public abstract bool IsEligible(Employee employee);
    protected abstract decimal InvokeCalculation(Employee employee);
    protected decimal CalculateFromMonthlyCost(decimal monthlyCost) => CalculateFromAnnualCost(monthlyCost * 12);
    protected decimal CalculateFromAnnualCost(decimal annualCost) => annualCost / PayPeriodSettings.CHECKS_PER_YEAR;
}
