using Api.Models;

namespace Api.Dtos.Dependent;

public abstract class DependentDtoBase : IDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
}

public class GetDependentDto : DependentDtoBase
{
    public int Id { get; set; }
}

public class PostDependentDto : DependentDtoBase { }
