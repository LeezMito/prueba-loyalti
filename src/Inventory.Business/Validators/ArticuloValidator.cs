using FluentValidation;
using Inventory.Entities.Models;

namespace Inventory.Business.Validators;

public class ArticuloValidator : AbstractValidator<Articulo>
{
    public ArticuloValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Precio).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ImagenUrl).MaximumLength(2083).When(x => x.ImagenUrl != null);
    }
}
