using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public abstract class EmployeeDtoBase : IDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();
}

public class GetEmployeeDto : EmployeeDtoBase
{
    public int Id { get; set; }
}

public class PostEmployeeDto : EmployeeDtoBase { }
