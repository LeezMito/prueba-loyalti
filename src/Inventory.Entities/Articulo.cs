namespace Inventory.Entities.Models;

public class Articulo
{
    public int Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public decimal Precio { get; set; }
    public string? ImagenUrl { get; set; }
    public int Stock { get; set; }

    public ICollection<ArticuloTienda> ArticuloTiendas { get; set; } = new List<ArticuloTienda>();
    public ICollection<ClienteArticulo> ClienteArticulos { get; set; } = new List<ClienteArticulo>();
}
