using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartphoneStore.Model.TabletSmartphone;

namespace SmartphoneStore.Api.TabletSmartphone;

[ApiController]
[Route("api/v1/tablets")]
public class TabletSmartphonesController : ControllerBase
{
    private readonly ITabletSmartphoneOrchestrator _orchestrator;

    public TabletSmartphonesController(ITabletSmartphoneOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    [HttpPost("{tabletId}/smartphones/{smartphoneId}")]
    public async Task<IActionResult> CreateLinkAsync([Required] Guid tabletId, [Required] int smartphoneId)
    {
        var model = await _orchestrator.CreateLinkAsync(tabletId, smartphoneId);
        return Ok(model);
    }

    [HttpGet("{tabletId}/smartphones")]
    public async Task<IActionResult> GetLinkedSmartphonesAsync([Required] Guid tabletId)
    {
        var ids = await _orchestrator.GetSmartphonesByTabletAsync(tabletId);
        return Ok(ids);
    }

    [HttpGet("{tabletId}/smartphones/{smartphoneId}")]
    public async Task<IActionResult> GetSmartphoneFromTabletAsync([Required] Guid tabletId, [Required] int smartphoneId)
    {
        var model = await _orchestrator.GetSmartphoneFromTabletAsync(tabletId, smartphoneId);
        return Ok(model);
    }

    [HttpDelete("{tabletId}/smartphones/{smartphoneId}")]
    public async Task<IActionResult> DeleteLinkAsync([Required] Guid tabletId, [Required] int smartphoneId)
    {
        await _orchestrator.DeleteLinkAsync(tabletId, smartphoneId);
        return NoContent();
    }
}