using Inventory.Entities.DTOs;

namespace Inventory.Business.Services;

public interface ICheckoutService
{
    Task<CheckoutResultDto> ConfirmAsync(int clienteId, CancellationToken ct = default);
}
