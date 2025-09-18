export interface CartOpResult {
  ok: boolean;
  reason?: 'out_of_stock' | 'limited';
  max?: number;
  qty?: number;
}

import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CartItem, CartState } from '../models/cart';
import { ArticuloListItemDto } from '../models/articulo';
import { ClienteArticuloService } from './cliente-articulo.service';
import { AuthService } from '../services/auth.service';
import { firstValueFrom } from 'rxjs';

const LS_KEY = 'inventory_cart_state_v2';

@Injectable({ providedIn: 'root' })
export class CartService {
  private state$: BehaviorSubject<CartState>;

  constructor(private clienteArticulo: ClienteArticuloService, private auth: AuthService) {
    const saved = localStorage.getItem(LS_KEY);
    const initial: CartState = saved ? JSON.parse(saved) : { items: [] };
    const cid = this.auth.snapshot.clienteId;
    this.state$ = new BehaviorSubject<CartState>({
      ...initial,
      clienteId: cid ?? initial.clienteId,
    });
    this.auth.select().subscribe((s) => this.patch({ clienteId: s.clienteId }));
  }

  select() {
    return this.state$.asObservable();
  }
  get snapshot(): CartState {
    return this.state$.value;
  }
  private save(state: CartState) {
    localStorage.setItem(LS_KEY, JSON.stringify(state));
    this.state$.next({ ...state });
  }
  private patch(partial: Partial<CartState>) {
    this.save({ ...this.snapshot, ...partial });
  }

  setCliente(id: number | undefined) {
    this.patch({ clienteId: id });
  }
  clear() {
    this.save({ items: [], clienteId: this.snapshot.clienteId });
  }

  add(articulo: ArticuloListItemDto, qty: number = 1): CartOpResult {
    const s = { ...this.snapshot, items: [...this.snapshot.items] };
    const idx = s.items.findIndex((i) => i.articulo.id === articulo.id);

    const stockMax = Number(articulo.stock ?? 0);
    const currentQty = idx >= 0 ? s.items[idx].qty : 0;
    if (stockMax <= 0) return { ok: false, reason: 'out_of_stock', max: 0, qty: currentQty };

    const desired = currentQty + qty;
    if (desired <= stockMax) {
      if (idx >= 0) s.items[idx] = { ...s.items[idx], qty: desired };
      else s.items.push({ articulo, qty, fechas: [] });
      this.save(s);
      return { ok: true, qty: desired, max: stockMax };
    }

    if (currentQty >= stockMax) {
      return { ok: false, reason: 'out_of_stock', max: stockMax, qty: currentQty };
    }

    if (idx >= 0) s.items[idx] = { ...s.items[idx], qty: stockMax };
    else s.items.push({ articulo, qty: stockMax, fechas: [] });
    this.save(s);
    return { ok: false, reason: 'limited', max: stockMax, qty: stockMax };
  }

  updateQty(articuloId: number, qty: number): CartOpResult {
    const s = { ...this.snapshot, items: [...this.snapshot.items] };
    const idx = s.items.findIndex((i) => i.articulo.id === articuloId);
    if (idx < 0) return { ok: false };

    const stockMax = Number(s.items[idx].articulo.stock ?? 0);

    if (qty <= 0) {
      s.items.splice(idx, 1);
      this.save(s);
      return { ok: true, qty: 0, max: stockMax };
    }

    if (stockMax <= 0) {
      s.items[idx] = { ...s.items[idx], qty: 0 };
      this.save(s);
      return { ok: false, reason: 'out_of_stock', max: 0, qty: 0 };
    }

    if (qty > stockMax) {
      s.items[idx] = { ...s.items[idx], qty: stockMax };
      this.save(s);
      return { ok: false, reason: 'limited', max: stockMax, qty: stockMax };
    }

    s.items[idx] = { ...s.items[idx], qty };
    this.save(s);
    return { ok: true, qty, max: stockMax };
  }

  async remove(articuloId: number) {
    const clienteId = this.snapshot.clienteId;
    if (!clienteId) throw new Error('No hay cliente autenticado');

    const prev = structuredClone(this.snapshot);
    const s: CartState = structuredClone(this.snapshot);

    const idx = s.items.findIndex((i) => i.articulo.id === articuloId);
    if (idx < 0) return;

    const line = s.items[idx] as CartItem;
    try {
      for (const f of line.fechas) {
        await firstValueFrom(this.clienteArticulo.delete(clienteId, articuloId, f));
      }
      s.items.splice(idx, 1);
      this.save(s);
    } catch (e) {
      this.save(prev);
      throw e;
    }
  }
}
