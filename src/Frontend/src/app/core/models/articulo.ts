import { TiendaItemDto } from "./tienda";

export interface ArticuloItemDto {
  id: number;
  codigo: string;
  descripcion: string;
  precio: number;
  imagenUrl?: string | null;
  stock: number;
  fecha?: string;
  tiendas: TiendaItemDto[];
}
export interface ArticuloDetailDto extends ArticuloItemDto {}
