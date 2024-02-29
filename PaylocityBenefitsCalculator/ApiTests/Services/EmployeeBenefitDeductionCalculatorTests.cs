using Api.Services;
using FluentAssertions;
using Xunit;

namespace ApiTests.Services;

public sealed class EmployeeBenefitDeductionCalculatorTests
{
	private readonly IPayrollAdjustmentCalculator _employeeBenefitDeductionCalculator;

	public EmployeeBenefitDeductionCalculatorTests()
	{
		_employeeBenefitDeductionCalculator = new EmployeeBenefitDeductionCalculator();
	}

	[Fact]
	public void IsEligible_Should_AlwaysReturnTrue()
	{
		var isEligible = _employeeBenefitDeductionCalculator.IsEligible(null);

		isEligible.Should().BeTrue();
	}

	[Fact]
	public void Execute_Should_AlwaysReturnCost()
	{
		var expectedCost = (-1000 * 12) / 26;

		var cost = _employeeBenefitDeductionCalculator.Execute(null);

		cost.Should().BeApproximately(expectedCost, 0.6m);
	}
}

