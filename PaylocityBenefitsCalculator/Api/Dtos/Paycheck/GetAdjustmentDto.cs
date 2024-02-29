namespace Api.Dtos.Paycheck;

public sealed class GetAdjustmentDto : IDto
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
}
