using System;
using System.Collections.Generic;
using Api.Models;
using Api.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ApiTests.Services;

public sealed class PaycheckGeneratorTests
{
	private readonly IEmployeeDataService _employeeDataService;
	private readonly IPayrollAdjustmentCalculator _mockedPayrollAdjustmentCalculator;
	private readonly IPayrollAdjustmentCalculator[] _payrollAdjustmentCalculators;
	private readonly IPaycheckGenerator _paycheckGenerator;

	public PaycheckGeneratorTests()
	{
		_employeeDataService = Substitute.For<IEmployeeDataService>();
		_mockedPayrollAdjustmentCalculator = Substitute.For<IPayrollAdjustmentCalculator>();
		_payrollAdjustmentCalculators = new[]
		{
			_mockedPayrollAdjustmentCalculator
		};
		_paycheckGenerator = new PaycheckGenerator(_employeeDataService, _payrollAdjustmentCalculators);
	}

	private static Employee GetEmployee()
	{
		return new()
		{
			DateOfBirth = DateTime.Today,
			Dependents = new List<Dependent>(),
			FirstName = "FN",
			Id = 123,
			LastName = "LN",
			Salary = 52_000
		};
	}

	[Fact]
	public void GeneratePaycheck_Should_FailIfCheckIdIsNull()
	{
        var checkId = -26;
        var employee = GetEmployee();
        var mockAdjustment = new Adjustment
        {
            Name = "Mocked Adjustment",
            Amount = -100m
        };
        _employeeDataService.Get(0).ReturnsForAnyArgs(employee);
        _mockedPayrollAdjustmentCalculator.IsEligible(null).ReturnsForAnyArgs(true);
        _mockedPayrollAdjustmentCalculator.Execute(null).ReturnsForAnyArgs(mockAdjustment.Amount);
        _mockedPayrollAdjustmentCalculator.Name.Returns(mockAdjustment.Name);

        var paycheckResult = _paycheckGenerator.GeneratePaycheck(checkId, employee.Id);

        _mockedPayrollAdjustmentCalculator.DidNotReceiveWithAnyArgs().IsEligible(null);
        _mockedPayrollAdjustmentCalculator.DidNotReceiveWithAnyArgs().Execute(null);
        paycheckResult.Should().NotBeNull();
        paycheckResult.IsSuccess.Should().BeFalse();
		paycheckResult.Data.Should().BeNull();
    }

	[Fact]
	public void GeneratePaycheck_Should_FailIfPaycheckBelongsToFuturePayPeriod()
	{
        var checkId = 100;
        var employee = GetEmployee();
        var mockAdjustment = new Adjustment
        {
            Name = "Mocked Adjustment",
            Amount = -100m
        };
        _employeeDataService.Get(0).ReturnsForAnyArgs(employee);
        _mockedPayrollAdjustmentCalculator.IsEligible(null).ReturnsForAnyArgs(true);
        _mockedPayrollAdjustmentCalculator.Execute(null).ReturnsForAnyArgs(mockAdjustment.Amount);
        _mockedPayrollAdjustmentCalculator.Name.Returns(mockAdjustment.Name);

        var paycheckResult = _paycheckGenerator.GeneratePaycheck(checkId, employee.Id);

        _mockedPayrollAdjustmentCalculator.DidNotReceiveWithAnyArgs().IsEligible(null);
        _mockedPayrollAdjustmentCalculator.DidNotReceiveWithAnyArgs().Execute(null);
        paycheckResult.Should().NotBeNull();
        paycheckResult.IsSuccess.Should().BeFalse();
        paycheckResult.Data.Should().BeNull();
    }

	[Fact]
	public void GeneratePaycheck_Should_FailIfEmployeeDoesNotExist()
	{
        var checkId = 100;
		Employee? employee = null;
        var mockAdjustment = new Adjustment
        {
            Name = "Mocked Adjustment",
            Amount = -100m
        };
        _employeeDataService.Get(0).ReturnsForAnyArgs(employee);
        _mockedPayrollAdjustmentCalculator.IsEligible(null).ReturnsForAnyArgs(true);
        _mockedPayrollAdjustmentCalculator.Execute(null).ReturnsForAnyArgs(mockAdjustment.Amount);
        _mockedPayrollAdjustmentCalculator.Name.Returns(mockAdjustment.Name);

        var paycheckResult = _paycheckGenerator.GeneratePaycheck(checkId, 100);

        _mockedPayrollAdjustmentCalculator.DidNotReceiveWithAnyArgs().IsEligible(null);
        _mockedPayrollAdjustmentCalculator.DidNotReceiveWithAnyArgs().Execute(null);
        paycheckResult.Should().NotBeNull();
        paycheckResult.IsSuccess.Should().BeFalse();
        paycheckResult.Data.Should().BeNull();
    }

	[Fact]
	public void GeneratePaycheck_Should_PassEvenIfAdjustmentsAreNotAdded()
	{
        var checkId = 26;
        var employee = GetEmployee();
        var mockAdjustment = new Adjustment
        {
            Name = "Mocked Adjustment",
            Amount = -100m
        };
        _employeeDataService.Get(0).ReturnsForAnyArgs(employee);
        _mockedPayrollAdjustmentCalculator.IsEligible(null).ReturnsForAnyArgs(false);
        _mockedPayrollAdjustmentCalculator.Execute(null).ReturnsForAnyArgs(mockAdjustment.Amount);
        _mockedPayrollAdjustmentCalculator.Name.Returns(mockAdjustment.Name);

        var paycheckResult = _paycheckGenerator.GeneratePaycheck(checkId, employee.Id);

        _mockedPayrollAdjustmentCalculator.Received().IsEligible(employee);
        _mockedPayrollAdjustmentCalculator.DidNotReceiveWithAnyArgs().Execute(null);
        paycheckResult.Should().NotBeNull();
        paycheckResult.IsSuccess.Should().BeTrue();

        var paycheck = paycheckResult.Data!;
        paycheck.Id.Should().Be(checkId);
        paycheck.CheckDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        paycheck.BasePay.Should().Be(employee.Salary / 26);
        paycheck.Adjustments.Should().BeEmpty();
        paycheck.Employee.Should().Be(employee);
        paycheck.NetPay.Should().Be(employee.Salary / 26);
    }

	[Fact]
	public void GeneratePaycheck_Should_ReturnExpectedData()
	{
		var checkId = 26;
		var employee = GetEmployee();
		var mockAdjustment = new Adjustment
		{
			Name = "Mocked Adjustment",
			Amount = -100m
		};
		_employeeDataService.Get(0).ReturnsForAnyArgs(employee);
		_mockedPayrollAdjustmentCalculator.IsEligible(null).ReturnsForAnyArgs(true);
		_mockedPayrollAdjustmentCalculator.Execute(null).ReturnsForAnyArgs(mockAdjustment.Amount);
		_mockedPayrollAdjustmentCalculator.Name.Returns(mockAdjustment.Name);

		var paycheckResult = _paycheckGenerator.GeneratePaycheck(checkId, employee.Id);

		_mockedPayrollAdjustmentCalculator.Received().IsEligible(employee);
		_mockedPayrollAdjustmentCalculator.Received().Execute(employee);
		paycheckResult.Should().NotBeNull();
		paycheckResult.IsSuccess.Should().BeTrue();

		var paycheck = paycheckResult.Data!;
		paycheck.Id.Should().Be(checkId);
		paycheck.CheckDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
		paycheck.BasePay.Should().Be(employee.Salary / 26);
		paycheck.Adjustments.Should().HaveCount(1);
		paycheck.Adjustments[0].Name.Should().Be(mockAdjustment.Name);
		paycheck.Adjustments[0].Amount.Should().Be(mockAdjustment.Amount);
		paycheck.Employee.Should().Be(employee);
		paycheck.NetPay.Should().Be((employee.Salary / 26) + mockAdjustment.Amount);
	}
}
