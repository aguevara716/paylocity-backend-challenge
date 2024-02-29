using Api.Dtos.Employee;
using Api.Mappers;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class EmployeesController : ControllerBase
{
    private readonly IEmployeeDataService _employeeDataService;
    private readonly IGetEmployeeDtoMapper _getEmployeeDtoMapper;
    private readonly ICreateEmployeeDtoMapper _createEmployeeDtoMapper;

    public EmployeesController(IEmployeeDataService employeeDataService,
                               IGetEmployeeDtoMapper getEmployeeDtoMapper,
                               ICreateEmployeeDtoMapper createEmployeeDtoMapper)
    {
        _employeeDataService = employeeDataService;
        _getEmployeeDtoMapper = getEmployeeDtoMapper;
        _createEmployeeDtoMapper = createEmployeeDtoMapper;
    }

    // GET
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = _employeeDataService.Get(id);
        if (employee is null)
            return NotFound();

        var getEmployeeDto = _getEmployeeDtoMapper.Map(employee);
        return ApiResponse<GetEmployeeDto>.BuildSuccess(getEmployeeDto);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = _employeeDataService.Get();
        var employeeDtos = _getEmployeeDtoMapper.Map(employees).ToList();
        var result = ApiResponse<List<GetEmployeeDto>>.BuildSuccess(employeeDtos);
        return result;
    }

    // POST
    [SwaggerOperation(Summary = "Create a new employee")]
    [HttpPost("")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Post([FromBody] CreateEmployeeDto postEmployeeDto)
    {
        var employee = _createEmployeeDtoMapper.Map(postEmployeeDto);
        var creationResult = _employeeDataService.Create(employee);
        if (!creationResult.IsSuccess)
            return BadRequest(creationResult.Error);

        var getEmployeeDto = _getEmployeeDtoMapper.Map(creationResult.Data!);
        return ApiResponse<GetEmployeeDto>.BuildSuccess(getEmployeeDto);
    }
}
