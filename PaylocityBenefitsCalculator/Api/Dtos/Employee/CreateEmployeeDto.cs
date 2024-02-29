using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public sealed class CreateEmployeeDto : EmployeeDtoBase
{
    public ICollection<CreateDependentDto> Dependents { get; set; } = new List<CreateDependentDto>();
}
