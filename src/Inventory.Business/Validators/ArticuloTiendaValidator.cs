using FluentValidation;
using Inventory.Entities.DTOs;

namespace Inventory.Business.Validators;

public class ArticuloTiendaCreateValidator : AbstractValidator<ArticuloTiendaCreateDto>
{
    public ArticuloTiendaCreateValidator()
    {
        RuleFor(x => x.ArticuloId).GreaterThan(0);
        RuleFor(x => x.TiendaId).GreaterThan(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
    }
}

public class ArticuloTiendaUpdateValidator : AbstractValidator<ArticuloTiendaUpdateDto>
{
    public ArticuloTiendaUpdateValidator()
    {
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
    }
}
