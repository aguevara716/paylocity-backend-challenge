using Api.Dtos.Paycheck;
using Api.Mappers;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class PaychecksController : ControllerBase
{
    private readonly IGetPaycheckDtoMapper _paycheckMapper;
    private readonly IPaycheckGenerator _paycheckGenerator;

    public PaychecksController(IGetPaycheckDtoMapper paycheckMapper,
                               IPaycheckGenerator paycheckGenerator)
    {
        _paycheckMapper = paycheckMapper;
        _paycheckGenerator = paycheckGenerator;
    }

    [SwaggerOperation(Summary = "Retrieves a paycheck for the given employee")]
    [HttpGet("{checkId}/employee/{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int checkId, int employeeId)
    {
        var paycheckResult = _paycheckGenerator.GeneratePaycheck(checkId, employeeId);
        if (!paycheckResult.IsSuccess)
            return BadRequest(paycheckResult.Error);

        var getPaycheckDto = _paycheckMapper.Map(paycheckResult.Data!);
        return ApiResponse<GetPaycheckDto>.BuildSuccess(getPaycheckDto);
    }
}
