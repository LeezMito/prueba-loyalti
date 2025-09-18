namespace Inventory.Entities.Models;

public class ArticuloTienda
{
    public int ArticuloId { get; set; }
    public Articulo Articulo { get; set; } = null!;

    public int TiendaId { get; set; }
    public Tienda Tienda { get; set; } = null!;

    public DateTime FechaAlta { get; set; }
}
