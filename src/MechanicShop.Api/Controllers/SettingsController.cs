using Asp.Versioning;

using MechanicShop.Contracts.Responses;
using MechanicShop.Infrastructure.Data;
using MechanicShop.Infrastructure.Settings;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MechanicShop.Api.Controllers;

[Route("api/settings")]
[ApiVersionNeutral]
public sealed class SettingsController(IOptions<AppSettings> options, AppDbContext context) : ApiController
{
    private readonly AppSettings _settings = options.Value;
    private readonly AppDbContext _context = context;

    [HttpGet("operating-hours")]
    [ProducesResponseType(typeof(OperatingHoursResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Gets the application's operating hours.")]
    [EndpointDescription("Returns the current configured opening and closing times.")]
    [EndpointName("GetOperatingHours")]
    public IActionResult GetOperatingHours()
    {
        return Ok(new OperatingHoursResponse(_settings.OpeningTime, _settings.ClosingTime));
    }

    [HttpGet("test-time")]
    [AllowAnonymous]
    public IActionResult GetTime()
    {
        var date = _context.Database.SqlQueryRaw<DateTime>("SELECT GETDATE()").AsEnumerable().First();
        return Ok(date);
    }
}