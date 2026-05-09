using Microsoft.AspNetCore.Mvc;
using SmartphoneStore.Model.MessageBroker;

namespace SmartphoneStore.Api.SmartphoneStats;

[ApiController]
[Route("api/v1/device-stats")]
public class DeviceStatsController : ControllerBase
{
    private readonly ISubscriber _subscriber;

    public DeviceStatsController(ISubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    [HttpGet]
    public IActionResult GetStats()
    {
        return Ok(_subscriber.Data);
    }
}