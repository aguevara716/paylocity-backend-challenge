using Api.Models;
using Api.Repositories;

namespace Api.Services;

public interface IDependentDataService
{
	// create

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

    public DependentDataService(IDependentRepository dependentRepository,
                                IEmployeeRepository employeeRepository)
    {
        _dependentRepository = dependentRepository;
        _employeeRepository = employeeRepository;
    }

	// create

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

