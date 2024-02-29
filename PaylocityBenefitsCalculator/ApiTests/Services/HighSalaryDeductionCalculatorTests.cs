using Api.Models;
using Api.Services;
using FluentAssertions;
using Xunit;

namespace ApiTests.Services;

public sealed class HighSalaryDeductionCalculatorTests
{
	private readonly IPayrollAdjustmentCalculator _highSalaryDeductionCalculator;

	public HighSalaryDeductionCalculatorTests()
	{
		_highSalaryDeductionCalculator = new HighSalaryDeductionCalculator();
	}

	[Theory]
    [InlineData(79_999, false)]
    [InlineData(80_000, true)]
    [InlineData(80_001, true)]
    public void IsEligible_Should_ReturnTrueIfSalaryThresholdIsMet(decimal salary, bool expectedResult)
	{
		var employee = new Employee
		{
			Salary = salary
		};

		var isEligible = _highSalaryDeductionCalculator.IsEligible(employee);

		isEligible.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData(79_999, 0)]
    [InlineData(80_000, (-80_000 * 0.02) / 26)]
    [InlineData(80_001, (-80_001 * 0.02) / 26)]
	public void Execute_Should_ReturnProperValue(decimal salary, decimal expectedCost)
	{
		var employee = new Employee
		{
			Salary = salary
		};

		var cost = _highSalaryDeductionCalculator.Execute(employee);

		cost.Should().BeApproximately(expectedCost, 0.5m);
	}
}
