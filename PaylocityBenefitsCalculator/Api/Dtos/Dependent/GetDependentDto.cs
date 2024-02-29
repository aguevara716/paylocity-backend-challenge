using Api.Models;

namespace Api.Dtos.Dependent;

public abstract class DependentDtoBase : IDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
}

public sealed class GetDependentDto : DependentDtoBase
{
    public int Id { get; set; }
}

// TODO you might need to include the employee ID when creating a new dependent, by itself?
// or this might be handled by a PutDependentDto... hmmm
public sealed class CreateDependentDto : DependentDtoBase
{
    public int EmployeeId { get; set; }
}
