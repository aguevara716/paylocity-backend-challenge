using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Mappers;

public interface ICreateDependentDtoMapper : IEntityMapper<Dependent, CreateDependentDto>
{

}

public sealed class CreateDependentDtoMapper : EntityMapperBase<Dependent, CreateDependentDto>, ICreateDependentDtoMapper
{
    public override Dependent Map(CreateDependentDto dto)
    {
        return new Dependent
        {
            DateOfBirth = dto.DateOfBirth,
            Employee = null,
            EmployeeId = dto.EmployeeId,
            FirstName = dto.FirstName,
            Id = 0,
            LastName = dto.LastName,
            Relationship = dto.Relationship
        };
    }
}
