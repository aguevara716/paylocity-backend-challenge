using System;
using Api.Models;
using Api.Services;
using FluentAssertions;
using Xunit;

namespace ApiTests.Services;

public sealed class AgeDeductionCalculatorTests
{
	private readonly IPayrollAdjustmentCalculator _ageDeductionCalculator;

	public AgeDeductionCalculatorTests()
	{
		_ageDeductionCalculator = new AgeDeductionCalculator();
	}

	[Theory]
	[InlineData(49, false)]
	[InlineData(50, true)]
	[InlineData(51, true)]
	public void IsEligible_Should_ReturnFalseIfAgeThresholdNotMet(int employeeAge, bool expectedResponse)
	{
		var employee = new Employee
		{
			DateOfBirth = DateTime.Today.AddYears(-1 * employeeAge)
		};

		var isEligible = _ageDeductionCalculator.IsEligible(employee);

		isEligible.Should().Be(expectedResponse);
	}

	[Theory]
    [InlineData(49, 0)]
    [InlineData(50, (-200 * 12) / 26)]
    [InlineData(51, (-200 * 12) / 26)]
    public void Execute_Should_ReturnDeductionAmount(int employeeAge, decimal expectedAmount)
	{
        var employee = new Employee
        {
            DateOfBirth = DateTime.Today.AddYears(-1 * employeeAge)
        };

        var charge = _ageDeductionCalculator.Execute(employee);

		charge.Should().BeApproximately(expectedAmount, 0.5m);
    }
}
