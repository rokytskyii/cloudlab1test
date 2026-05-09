using Microsoft.AspNetCore.Mvc;
using SmartphoneStore.Model.MessageBroker;

namespace SmartphoneStore.Api.SmartphoneStats;

[ApiController]
[Route("api/v1/smartphones-stats")]
public class SmartphoneStatsController : ControllerBase
{
    private readonly ISubscriber _subscriber;

    public SmartphoneStatsController(ISubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    [HttpGet]
    public IActionResult GetStats()
    {
        return Ok(_subscriber.Data);
    }
}