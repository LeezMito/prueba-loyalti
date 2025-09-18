import { Injectable } from '@angular/core';
import { delay, map, of } from 'rxjs';

export interface CheckoutPayload {
  clienteId: number;
  items: { articuloId: number; qty: number; }[];
  shippingAddress: string;
  paymentMethod: 'card' | 'oxxo' | 'transfer';
  cardLast4?: string | null;
  total: number;
}

@Injectable({ providedIn: 'root' })
export class OrderService {
  createOrder(payload: CheckoutPayload) {
    return of(payload).pipe(
      delay(1200),
      map(p => ({
        orderId: Math.floor(Math.random() * 900000) + 100000,
        createdAt: new Date().toISOString(),
      }))
    );
  }
}
