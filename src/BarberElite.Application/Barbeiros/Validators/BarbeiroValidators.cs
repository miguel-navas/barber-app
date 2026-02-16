using BarberElite.Application.Barbeiros.Commands;
using FluentValidation;

namespace BarberElite.Application.Barbeiros.Validators;

public sealed class CreateBarbeiroCommandValidator : AbstractValidator<CreateBarbeiroCommand>
{
    public CreateBarbeiroCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
    }
}

public sealed class UpdateBarbeiroCommandValidator : AbstractValidator<UpdateBarbeiroCommand>
{
    public UpdateBarbeiroCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Dto.Nome).NotEmpty().MaximumLength(120);
    }
}
