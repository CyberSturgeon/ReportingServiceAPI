using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Core.Configuration.Filters;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Controllers;
[Route("api/comissions")]
public class ComissionController(
    IComissionService comissionService,
    IMapper mapper) : Controller
{
    [HttpGet("")]
    public async Task<ActionResult<ComissionResponse>> GetComissionByIdAsync([FromQuery] Guid? customerId = null,
                                                       [FromQuery] Guid? accountId = null, [FromQuery] DateTimeFilter? date = null)
    {
        var comission = mapper.Map<ComissionResponse>(
                       await comissionService.GetAsync(customerId, accountId, date));

        return Ok(comission);
    }

}
