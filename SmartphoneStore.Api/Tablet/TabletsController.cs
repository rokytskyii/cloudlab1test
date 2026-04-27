using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartphoneStore.Api.Tablet.Contract;
using SmartphoneStore.Model.Tablet;

namespace SmartphoneStore.Api.Tablet;

[ApiController]
[Route("api/v1/tablets")]
public class TabletsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITabletOrchestrator _orchestrator;

    public TabletsController(IMapper mapper, ITabletOrchestrator orchestrator)
    {
        _mapper = mapper;
        _orchestrator = orchestrator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateTablet request)
    {
        var dto = _mapper.Map<TabletDto>(request);
        var created = await _orchestrator.CreateAsync(dto);
        var response = _mapper.Map<GetTablet>(created);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var tablet = await _orchestrator.GetByIdAsync(id);
        var response = _mapper.Map<GetTablet>(tablet);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tablets = await _orchestrator.GetAllAsync(page, pageSize);
        var response = _mapper.Map<IEnumerable<GetTablet>>(tablets);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateTablet request)
    {
        var dto = _mapper.Map<TabletDto>(request);
        dto.Id = id;

        var updated = await _orchestrator.UpdateAsync(dto);
        return Ok(_mapper.Map<GetTablet>(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var deletedTablet = await _orchestrator.DeleteAsync(id);
        return Ok(_mapper.Map<GetTablet>(deletedTablet));
    }
}