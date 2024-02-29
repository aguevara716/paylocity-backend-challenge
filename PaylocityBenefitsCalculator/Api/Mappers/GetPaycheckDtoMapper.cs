using Api.Dtos.Paycheck;
using Api.Models;

namespace Api.Mappers;

public interface IGetPaycheckDtoMapper : IDtoMapper<GetPaycheckDto, Paycheck>
{

}

public class GetPaycheckDtoMapper : DtoMapperBase<GetPaycheckDto, Paycheck>, IGetPaycheckDtoMapper
{
    private readonly IGetAdjustmentDtoMapper _adjustmentMapper;
    private readonly IGetEmployeeDtoMapper _employeeMapper;

    public GetPaycheckDtoMapper(IGetAdjustmentDtoMapper adjustmentMapper,
                                IGetEmployeeDtoMapper employeeMapper)
    {
        _adjustmentMapper = adjustmentMapper;
        _employeeMapper = employeeMapper;
    }

    public override GetPaycheckDto Map(Paycheck entity)
    {
        return new GetPaycheckDto
        {
            Adjustments = _adjustmentMapper.Map(entity.Adjustments).ToArray(),
            BasePay = entity.BasePay,
            CheckDate = entity.CheckDate,
            CheckId = entity.Id,
            Employee = _employeeMapper.Map(entity.Employee),
            NetPay = entity.NetPay
        };
    }
}
