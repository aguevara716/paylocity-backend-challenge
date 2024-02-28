using Api.Dtos.Employee;
using Api.Models;

namespace Api.Mappers;

public interface IGetEmployeeDtoMapper : IDtoMapper<GetEmployeeDto, Employee>
{

}

public class GetEmployeeDtoMapper : DtoMapperBase<GetEmployeeDto, Employee>, IGetEmployeeDtoMapper
{
    private readonly IGetDependentDtoMapper _dependentMapper;

    public GetEmployeeDtoMapper(IGetDependentDtoMapper dependentMapper)
    {
        _dependentMapper = dependentMapper;
    }

    public override GetEmployeeDto Map(Employee entity)
    {
        return new GetEmployeeDto
        {
            DateOfBirth = entity.DateOfBirth,
            Dependents = _dependentMapper.Map(entity.Dependents).ToList(),
            FirstName = entity.FirstName,
            Id = entity.Id,
            LastName = entity.LastName,
            Salary = entity.Salary
        };
    }
}
