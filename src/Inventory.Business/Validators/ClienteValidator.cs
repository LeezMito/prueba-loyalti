using FluentValidation;
using Inventory.Entities.Models;

namespace Inventory.Business.Validators;

public class ClienteValidator : AbstractValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Apellidos).NotEmpty().MaximumLength(120);
    }
}
