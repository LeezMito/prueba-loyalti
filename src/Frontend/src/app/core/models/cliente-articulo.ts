export interface ClienteArticuloCreateDto {
  clienteId: number;
  articuloId: number;
  fecha?: string;
}

export interface ClienteArticuloItemDto {
  clienteId: number;
  clienteNombreCompleto: string;
  articuloId: number;
  articuloCodigo: string;
  articuloDescripcion: string;
  fecha: string;
  tiendaId?: number | null;
  tiendaSucursal?: string | null;
  articuloPrecio: number;
  articuloImagenUrl?: string | null;
  stock: number;
}
