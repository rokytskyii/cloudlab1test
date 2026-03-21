using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartphoneStore.Api.Smartphone.Contract;
using SmartphoneStore.Model.Smartphone;

namespace SmartphoneStore.Api.Smartphone;

[ApiController]
[Route("api/v1/smartphones")]
public class SmartphonesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISmartphoneOrchestrator _orchestrator;

    public SmartphonesController(IMapper mapper, ISmartphoneOrchestrator orchestrator)
    {
        _mapper = mapper;
        _orchestrator = orchestrator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateSmartphone request)
    {
        var dto = _mapper.Map<SmartphoneDto>(request);
        var created = await _orchestrator.CreateAsync(dto);
        var response = _mapper.Map<GetSmartphone>(created);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([Range(1, int.MaxValue)] int id)
    {
        var smartphone = await _orchestrator.GetByIdAsync(id);
        var response = _mapper.Map<GetSmartphone>(smartphone);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var smartphones = await _orchestrator.GetAllAsync(page, pageSize);
        var response = _mapper.Map<IEnumerable<GetSmartphone>>(smartphones);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateSmartphone request)
    {
        if (id != request.Id) return BadRequest("ID в URL не співпадає з ID в тілі запиту.");

        var dto = _mapper.Map<SmartphoneDto>(request);
        var updated = await _orchestrator.UpdateAsync(dto);
        var response = _mapper.Map<GetSmartphone>(updated);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _orchestrator.DeleteAsync(id);
        return NoContent();
    }
}