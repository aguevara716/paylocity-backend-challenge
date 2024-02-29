namespace Api.Models;

public sealed class Adjustment : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
}
