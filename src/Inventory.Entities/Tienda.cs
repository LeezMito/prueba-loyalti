namespace Inventory.Entities.Models;

public class Tienda
{
    public int Id { get; set; }
    public string Sucursal { get; set; } = null!;
    public string? Direccion { get; set; }

    public ICollection<ArticuloTienda> ArticuloTiendas { get; set; } = new List<ArticuloTienda>();
}
