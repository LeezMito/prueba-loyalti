using FluentValidation;
using Inventory.Entities.DTOs;

namespace Inventory.Business.Validators;

public class ClienteArticuloCreateValidator : AbstractValidator<ClienteArticuloCreateDto>
{
    public ClienteArticuloCreateValidator()
    {
        RuleFor(x => x.ClienteId).GreaterThan(0);
        RuleFor(x => x.ArticuloId).GreaterThan(0);
    }
}
