export interface ArticuloTiendaListItemDto {
  articuloId: number;
  articuloCodigo: string;
  articuloDescripcion: string;
  tiendaId: number;
  tiendaSucursal: string;
  stock: number;
  fechaAlta: string;
  precio: number; 
  articuloImagenUrl: string;
}
