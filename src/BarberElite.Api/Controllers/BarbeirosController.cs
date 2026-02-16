using BarberElite.Application.Barbeiros.Commands;
using BarberElite.Application.Barbeiros.DTOs;
using BarberElite.Application.Barbeiros.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BarberElite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BarbeirosController : ControllerBase
{
    private readonly IMediator _mediator;

    public BarbeirosController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BarbeiroResponseDto>> Create([FromBody] BarbeiroCreateDto dto, CancellationToken ct)
    {
        // se seu CreateCommand usa (UsuarioId, Nome) direto:
        var created = await _mediator.Send(new CreateBarbeiroCommand(dto.UsuarioId, dto.Nome), ct);
        return Ok(created);

        // Se seu CreateCommand for (Dto) em vez disso:
        // var created = await _mediator.Send(new CreateBarbeiroCommand(dto), ct);
    }

    [HttpGet]
    public Task<List<BarbeiroResponseDto>> List([FromQuery] bool apenasAtivos = true, CancellationToken ct = default)
        => _mediator.Send(new ListBarbeirosQuery(apenasAtivos), ct);

    [HttpGet("{id:guid}")]
    public Task<BarbeiroResponseDto> GetById(Guid id, CancellationToken ct)
        => _mediator.Send(new GetBarbeiroByIdQuery(id), ct);

    [HttpPut("{id:guid}")]
    public Task<BarbeiroResponseDto> Update(Guid id, [FromBody] BarbeiroUpdateDto dto, CancellationToken ct)
        => _mediator.Send(new UpdateBarbeiroCommand(id, dto), ct);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new InativarBarbeiroCommand(id), ct);
        return NoContent();
    }
}

