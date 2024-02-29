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
	public void CanExecute_Should_AlwaysReturnTrue()
	{
		var canExecute = _employeeBenefitDeductionCalculator.CanExecute(null);

		canExecute.Should().BeTrue();
	}

	[Fact]
	public void Execute_Should_AlwaysReturnCost()
	{
		var expectedCost = (-1000 * 12) / 26;

		var cost = _employeeBenefitDeductionCalculator.Execute(null);

		cost.Should().BeApproximately(expectedCost, 0.6m);
	}
}

