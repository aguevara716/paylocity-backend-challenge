using Api.Dtos.Employee;
using Api.Models;

namespace Api.Mappers;

public interface ICreateEmployeeDtoMapper : IEntityMapper<Employee, CreateEmployeeDto>
{

}

public sealed class CreateEmployeeDtoMapper : EntityMapperBase<Employee, CreateEmployeeDto>, ICreateEmployeeDtoMapper
{
    private readonly ICreateDependentDtoMapper _dependentMapper;

    public CreateEmployeeDtoMapper(ICreateDependentDtoMapper dependentMapper)
    {
        _dependentMapper = dependentMapper;
    }

    public override Employee Map(CreateEmployeeDto dto)
    {
        return new Employee
        {
            DateOfBirth = dto.DateOfBirth,
            Dependents = _dependentMapper.Map(dto.Dependents).ToList(),
            FirstName = dto.FirstName,
            Id = 0,
            LastName = dto.LastName,
            Salary = dto.Salary
        };
    }
}
