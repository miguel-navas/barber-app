using BarberElite.Application.Servicos.Commands;
using BarberElite.Application.Servicos.DTOs;
using BarberElite.Application.Servicos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace BarberElite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicosController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ServicoResponseDto>> Create([FromBody] ServicoCreateDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateServicoCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public Task<List<ServicoResponseDto>> List(CancellationToken ct)
        => _mediator.Send(new ListServicosQuery(), ct);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServicoResponseDto>> GetById(Guid id, CancellationToken ct)
    {
        // simples: reaproveita list e filtra (MVP). Depois fazemos query específica.
        var all = await _mediator.Send(new ListServicosQuery(), ct);
        var one = all.FirstOrDefault(x => x.Id == id);
        return one is null ? NotFound() : Ok(one);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServicoResponseDto>> Update(Guid id, [FromBody] ServicoUpdateDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateServicoCommand(id, dto), ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DesativarServicoCommand(id), ct);
        return NoContent();
    }
}
