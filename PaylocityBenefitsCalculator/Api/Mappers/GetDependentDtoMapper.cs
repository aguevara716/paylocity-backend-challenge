using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Mappers;

public interface IGetDependentDtoMapper : IDtoMapper<GetDependentDto, Dependent>
{

}

public sealed class GetDependentDtoMapper : DtoMapperBase<GetDependentDto, Dependent>, IGetDependentDtoMapper
{
    public override GetDependentDto Map(Dependent entity)
    {
        return new GetDependentDto
        {
            DateOfBirth = entity.DateOfBirth,
            FirstName = entity.FirstName,
            Id = entity.Id,
            LastName = entity.LastName,
            Relationship = entity.Relationship
        };
    }
}

