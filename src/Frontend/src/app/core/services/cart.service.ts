export interface CartOpResult {
  ok: boolean;
  reason?: 'out_of_stock' | 'limited';
  max?: number;
  qty?: number;
}

import { Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { CartItem, CartState } from '../models/cart';
import { ArticuloItemDto } from '../models/articulo';
import { ClienteArticuloService } from './cliente-articulo.service';
import { AuthService } from '../services/auth.service';
import { ClienteArticuloItemDto } from '../models/cliente-articulo';

@Injectable({ providedIn: 'root' })
export class CartService {
  private state$ = new BehaviorSubject<CartState>({ items: [] });

  constructor(
    private clienteArticulo: ClienteArticuloService,
    private auth: AuthService
  ) {
    this.auth.select().subscribe(async s => {
      await this.setCliente(s.clienteId);
    });
    if (this.auth.snapshot?.clienteId) {
      this.setCliente(this.auth.snapshot.clienteId);
    }
  }

  select() { return this.state$.asObservable(); }

  get snapshot(): CartState { return this.state$.value; }

  private setState(state: CartState) {
    this.state$.next({ ...state });
  }

  private async reload(clienteId: number) {
    const rows = await firstValueFrom(this.clienteArticulo.listByCliente(clienteId));
    const grouped = this.hydrateFromApi(rows);
    this.setState({ clienteId, items: grouped });
  }

  private hydrateFromApi(rows: ClienteArticuloItemDto[]): CartItem[] {
    const map = new Map<number, CartItem>();
    for (const r of rows) {
      const a: ArticuloItemDto = {
        id: r.articuloId,
        codigo: r.articuloCodigo,
        descripcion: r.articuloDescripcion,
        precio: r.articuloPrecio,
        imagenUrl: r.articuloImagenUrl ?? null,
        stock: r.stock,
        fecha: r.fecha,
        tiendas: null!,
      };
      const existing = map.get(a.id);
      if (existing) {
        existing.qty += 1;
        existing.fechas.push(r.fecha);
      } else {
        map.set(a.id, { articulo: a, qty: 1, fechas: [r.fecha] });
      }
    }
    return Array.from(map.values());
  }

  async setCliente(id: number | undefined) {
    if (!id) { this.setState({ items: [], clienteId: undefined }); return; }
    await this.reload(id);
  }

  clear() {
    this.setState({ items: [], clienteId: this.snapshot.clienteId });
  }

  async add(articulo: ArticuloItemDto, qty: number = 1): Promise<CartOpResult> {
    const clienteId = this.snapshot.clienteId;
    if (!clienteId) throw new Error('No hay cliente autenticado');

    const stockMax = Number(articulo.stock ?? 0);
    const line = this.snapshot.items.find(i => i.articulo.id === articulo.id);
    const currentQty = line?.qty ?? 0;

    if (stockMax <= 0) return { ok: false, reason: 'out_of_stock', max: 0, qty: currentQty };

    const desired = currentQty + qty;
    const toCreate = Math.min(qty, Math.max(0, stockMax - currentQty));

    if (toCreate <= 0) {
      return currentQty >= stockMax
        ? { ok: false, reason: 'out_of_stock', max: stockMax, qty: currentQty }
        : { ok: false, reason: 'limited', max: stockMax, qty: currentQty };
    }

    for (let i = 0; i < toCreate; i++) {
      const fecha = new Date().toISOString();
      await firstValueFrom(this.clienteArticulo.create({
        clienteId,
        articuloId: articulo.id,
        fecha
      }));
    }

    await this.reload(clienteId);

    if (desired > stockMax) {
      return { ok: false, reason: 'limited', max: stockMax, qty: stockMax };
    }
    return { ok: true, qty: desired, max: stockMax };
  }

  async updateQty(articuloId: number, qty: number): Promise<CartOpResult> {
    const clienteId = this.snapshot.clienteId;
    if (!clienteId) throw new Error('No hay cliente autenticado');

    const line = this.snapshot.items.find(i => i.articulo.id === articuloId);
    if (!line) return { ok: false };

    const stockMax = Number(line.articulo.stock ?? 0);
    if (qty <= 0) {
      for (const f of line.fechas) {
        await firstValueFrom(this.clienteArticulo.delete(clienteId, articuloId, f));
      }
      await this.reload(clienteId);
      return { ok: true, qty: 0, max: stockMax };
    }

    if (stockMax <= 0) {
      for (const f of line.fechas) {
        await firstValueFrom(this.clienteArticulo.delete(clienteId, articuloId, f));
      }
      await this.reload(clienteId);
      return { ok: false, reason: 'out_of_stock', max: 0, qty: 0 };
    }

    const current = line.qty;
    const target = Math.min(qty, stockMax);

    if (target > current) {
      const diff = target - current;
      for (let i = 0; i < diff; i++) {
        const fecha = new Date().toISOString();
        await firstValueFrom(this.clienteArticulo.create({ clienteId, articuloId, fecha }));
      }
    } else if (target < current) {
      const diff = current - target;
      for (let i = 0; i < diff; i++) {
        const fecha = line.fechas[line.fechas.length - 1 - i];
        await firstValueFrom(this.clienteArticulo.delete(clienteId, articuloId, fecha));
      }
    }

    await this.reload(clienteId);

    if (qty > stockMax) {
      return { ok: false, reason: 'limited', max: stockMax, qty: stockMax };
    }
    return { ok: true, qty: target, max: stockMax };
  }

  async remove(articuloId: number) {
    const clienteId = this.snapshot.clienteId;
    if (!clienteId) throw new Error('No hay cliente autenticado');

    const line = this.snapshot.items.find(i => i.articulo.id === articuloId);
    if (!line) return;

    for (const f of line.fechas) {
      await firstValueFrom(this.clienteArticulo.delete(clienteId, articuloId, f));
    }
    await this.reload(clienteId);
  }
}
