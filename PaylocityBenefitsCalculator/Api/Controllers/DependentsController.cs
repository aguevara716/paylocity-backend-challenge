using Api.Dtos.Dependent;
using Api.Mappers;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class DependentsController : ControllerBase
{
    private readonly IDependentDataService _dependentDataService;
    private readonly IGetDependentDtoMapper _getDependentDtoMapper;
    private readonly ICreateDependentDtoMapper _createDependentDtoMapper;

    public DependentsController(IDependentDataService dependentDataService,
                                IGetDependentDtoMapper getDependentDtoMapper,
                                ICreateDependentDtoMapper createDependentDtoMapper)
    {
        _dependentDataService = dependentDataService;
        _getDependentDtoMapper = getDependentDtoMapper;
        _createDependentDtoMapper = createDependentDtoMapper;
    }

    // GET
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = _dependentDataService.Get(id);
        if (dependent is null)
            return NotFound();

        var getDependentDto = _getDependentDtoMapper.Map(dependent);
        return ApiResponse<GetDependentDto>.BuildSuccess(getDependentDto);
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = _dependentDataService.Get();
        var dependentDtos = _getDependentDtoMapper.Map(dependents).ToList();
        return ApiResponse<List<GetDependentDto>>.BuildSuccess(dependentDtos);
    }

    // POST
    [SwaggerOperation(Summary = "Create a dependent and associate them with an employee")]
    [HttpPost("")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Post([FromBody] CreateDependentDto createDependentDto)
    {
        var dependent = _createDependentDtoMapper.Map(createDependentDto);
        var creationResult = _dependentDataService.Create(dependent);
        if (!creationResult.IsSuccess)
            return BadRequest(creationResult.Error);

        var getDependentDto = _getDependentDtoMapper.Map(creationResult.Data!);
        return ApiResponse<GetDependentDto>.BuildSuccess(getDependentDto);
    }
}
