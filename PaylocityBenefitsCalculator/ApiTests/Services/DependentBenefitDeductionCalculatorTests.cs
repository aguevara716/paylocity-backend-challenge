using System;
using System.Collections.Generic;
using Api.Models;
using Api.Services;
using FluentAssertions;
using Xunit;

namespace ApiTests.Services;

public sealed class DependentBenefitDeductionCalculatorTests
{
	private readonly IPayrollAdjustmentCalculator _dependentBenefitDeductionCalculator;

	public DependentBenefitDeductionCalculatorTests()
	{
		_dependentBenefitDeductionCalculator = new DependentBenefitDeductionCalculator();
	}

	private static IEnumerable<Dependent> GetDependents(int count)
	{
		for (var index = 0; index < count; index++)
		{
			yield return new Dependent
			{
				DateOfBirth = DateTime.Today,
				Employee = null,
				EmployeeId = 0,
				FirstName = "FN",
				Id = 0,
				LastName = "LN",
				Relationship = Relationship.None
			};
		}
	}

	[Theory]
	[InlineData(0, false)]
	[InlineData(10, true)]
	public void IsEligible_Should_ReturnTrueIfAtLeastOneDependentExists(int dependentCount, bool expectedResult)
	{
		var dependents = GetDependents(dependentCount);
		var employee = new Employee
		{
			Dependents = new List<Dependent>(dependents)
		};

		var isEligible = _dependentBenefitDeductionCalculator.IsEligible(employee);

		isEligible.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData(0, 0)]
	[InlineData(1, (-600 * 12) / 26)]
	[InlineData(2, (-600 * 12 * 2) / 26)]
	public void Execute_Should_ReturnProperValue(int dependentCount, decimal expectedCost)
	{
		var dependents = GetDependents(dependentCount);
		var employee = new Employee
		{
			Dependents = new List<Dependent>(dependents)
		};

		var cost = _dependentBenefitDeductionCalculator.Execute(employee);

		cost.Should().BeApproximately(expectedCost, 1.0m);
	}
}
