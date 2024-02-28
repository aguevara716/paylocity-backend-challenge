using Api.Models;
using Api.Repositories;
using Api.Validators;

namespace Api.Services;

public interface IDependentDataService
{
	// create
	ValidationResult<Dependent> Create(Dependent newDependent);

	// read
	Dependent[] Get();
	Dependent? Get(int id);

	// update

	// delete
}

public sealed class DependentDataService : IDependentDataService
{
	private readonly IDependentRepository _dependentRepository;
	private readonly IEmployeeRepository _employeeRepository;
	private readonly IDependentValidator _dependentValidator;

    public DependentDataService(IDependentRepository dependentRepository,
                                IEmployeeRepository employeeRepository,
                                IDependentValidator dependentValidator)
    {
        _dependentRepository = dependentRepository;
        _employeeRepository = employeeRepository;
        _dependentValidator = dependentValidator;
    }

	// create
	public ValidationResult<Dependent> Create(Dependent newDependent)
	{
		var validationResult = _dependentValidator.Validate(newDependent);
		if (!validationResult.IsSuccess)
			return validationResult;

		_dependentRepository.Create(validationResult.Data!);
		return validationResult;
	}

    // read
    public Dependent[] Get()
	{
		var dependents = _dependentRepository.Get().ToArray();
		foreach (var dependent in dependents)
		{
			dependent.Employee = _employeeRepository.Get(dependent.EmployeeId);
		}
		return dependents;
	}

	public Dependent? Get(int id)
	{
		var dependent = _dependentRepository.Get(id);
		if (dependent is null)
			return null;

		dependent.Employee = _employeeRepository.Get(dependent.EmployeeId);
		return dependent;
	}

	// update

	// delete
}

