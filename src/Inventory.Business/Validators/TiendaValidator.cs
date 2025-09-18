using FluentValidation;
using Inventory.Entities.Models;

namespace Inventory.Business.Validators;

public class TiendaValidator : AbstractValidator<Tienda>
{
    public TiendaValidator()
    {
        RuleFor(x => x.Sucursal).NotEmpty().MaximumLength(100);
    }
}
