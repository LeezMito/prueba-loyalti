import { ArticuloListItemDto } from './articulo';

export interface CartItem {
  articulo: ArticuloListItemDto;
  qty: number;
  fechas: string[];
}

export interface CartState {
  clienteId?: number;
  items: CartItem[];
}

export function cartTotal(state: CartState) {
  return state.items.reduce((s, i) => s + i.articulo.precio * i.qty, 0);
}