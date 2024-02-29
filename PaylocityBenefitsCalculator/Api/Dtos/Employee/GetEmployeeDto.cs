using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public sealed class GetEmployeeDto : EmployeeDtoBase
{
    public int Id { get; set; }
    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();
}
