using BarberElite.Application.Disponibilidades.Commands;
using BarberElite.Application.Disponibilidades.DTOs;
using BarberElite.Application.Disponibilidades.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BarberElite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DisponibilidadesController : ControllerBase
{
    private readonly IMediator _mediator;
    public DisponibilidadesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public Task<DisponibilidadeResponseDto> Create([FromBody] DisponibilidadeCreateDto dto, CancellationToken ct)
        => _mediator.Send(new CreateDisponibilidadeCommand(dto), ct);

    [HttpPut("{id:guid}")]
    public Task<DisponibilidadeResponseDto> Update(Guid id, [FromBody] DisponibilidadeUpdateDto dto, CancellationToken ct)
        => _mediator.Send(new UpdateDisponibilidadeCommand(id, dto), ct);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new InativarDisponibilidadeCommand(id), ct);
        return NoContent();
    }

    [HttpGet("barbeiro/{barbeiroId:guid}")]
    public Task<List<DisponibilidadeResponseDto>> ListByBarbeiro(Guid barbeiroId, [FromQuery] bool apenasAtivas = true, CancellationToken ct = default)
        => _mediator.Send(new ListDisponibilidadesByBarbeiroQuery(barbeiroId, apenasAtivas), ct);
}
