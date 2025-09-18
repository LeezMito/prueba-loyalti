export interface ArticuloTiendaItemDto {
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
