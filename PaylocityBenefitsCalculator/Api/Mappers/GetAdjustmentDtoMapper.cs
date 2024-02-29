using Api.Dtos.Paycheck;
using Api.Models;

namespace Api.Mappers;

public interface IGetAdjustmentDtoMapper : IDtoMapper<GetAdjustmentDto, Adjustment>
{

}

public class GetAdjustmentDtoMapper : DtoMapperBase<GetAdjustmentDto, Adjustment>, IGetAdjustmentDtoMapper
{
    public override GetAdjustmentDto Map(Adjustment entity)
    {
        return new GetAdjustmentDto
        {
            Amount = entity.Amount,
            Name = entity.Name
        };
    }
}

