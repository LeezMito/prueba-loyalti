export interface ArticuloListItemDto {
  id: number;
  codigo: string;
  descripcion: string;
  precio: number;
  imagenUrl?: string | null;
  stock: number;
  tiendaSucursal: string;
  tiendaId: number;
}
export interface ArticuloDetailDto extends ArticuloListItemDto {}
