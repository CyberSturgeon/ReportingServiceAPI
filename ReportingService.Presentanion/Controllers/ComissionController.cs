using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Services.Interfaces;
using ReportingService.Presentanion.Models;

namespace ReportingService.Presentanion.Controllers;
[Route("api/comissions")]
public class ComissionController(
    IComissionService comissionService,
    IMapper mapper) : Controller
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ComissionResponse>> GetComissionByIdAsync([FromRoute] Guid id)
    {
        var customer = mapper.Map<ComissionResponse>(
                       await comissionService.GetComissionByIdAsync(id));

        return Ok(customer);
    }

}
