namespace Api.Models;

public sealed class Paycheck : IEntity
{
    public int Id { get; set; }
    public DateOnly CheckDate { get; set; }
    public decimal BasePay { get; set; }
    public Adjustment[] Adjustments { get; set; } = Array.Empty<Adjustment>();
    public Employee Employee { get; set; } = null!;

    public decimal NetPay => BasePay + Adjustments.Sum(a => a.Amount);
}
