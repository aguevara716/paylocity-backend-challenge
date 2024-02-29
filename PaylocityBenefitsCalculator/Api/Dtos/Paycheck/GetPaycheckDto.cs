using Api.Dtos.Employee;

namespace Api.Dtos.Paycheck;

public sealed class GetPaycheckDto : IDto
{
    public int CheckId { get; set; }
    public DateOnly CheckDate { get; set; }
    public decimal BasePay { get; set; }
    public decimal NetPay { get; set; }
    public GetAdjustmentDto[] Adjustments { get; set; } = Array.Empty<GetAdjustmentDto>();
    public GetEmployeeDto Employee { get; set; } = null!;
}

public sealed class GetAdjustmentDto : IDto
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
}
