using BarberElite.Application.Servicos.Commands;
using FluentValidation;

namespace BarberElite.Application.Servicos.Validators;

public sealed class CreateServicoCommandValidator : AbstractValidator<CreateServicoCommand>
{
    public CreateServicoCommandValidator()
    {
        RuleFor(x => x.Dto.Nome)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Dto.DuracaoMinutos)
            .GreaterThan(0)
            .LessThanOrEqualTo(480);

        RuleFor(x => x.Dto.Preco)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Dto.Descricao)
            .MaximumLength(500);

        // ✅ AQUI
        RuleFor(x => x.Dto.Tipo)
            .IsInEnum();
    }
}

public sealed class UpdateServicoCommandValidator : AbstractValidator<UpdateServicoCommand>
{
    public UpdateServicoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Dto.Nome)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Dto.DuracaoMinutos)
            .GreaterThan(0)
            .LessThanOrEqualTo(480);

        RuleFor(x => x.Dto.Preco)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Dto.Descricao)
            .MaximumLength(500);

        // ✅ AQUI
        RuleFor(x => x.Dto.Tipo)
            .IsInEnum();
    }
}
