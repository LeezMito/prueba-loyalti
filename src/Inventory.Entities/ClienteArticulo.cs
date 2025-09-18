namespace Inventory.Entities.Models;

public class ClienteArticulo
{
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int ArticuloId { get; set; }
    public Articulo Articulo { get; set; } = null!;

    public DateTime Fecha { get; set; }
}
