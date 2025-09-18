import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { CartService } from '../../core/services/cart.service';
import { AuthService } from '../../core/services/auth.service';
import { OrderService } from '../../core/services/order.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.sass'],
})
export class CheckoutComponent {
  fullName = '';
  address = '';
  payment: 'card' | 'oxxo' | 'transfer' = 'card';
  cardNumber = '';

  loading = false;
  error?: string;
  success?: string;
  orderId?: number;

  constructor(
    public cart: CartService,
    public auth: AuthService,
    private orders: OrderService,
  ) {}

  get items() { return this.cart.snapshot.items; }
  get total() {
    return this.items.reduce((s, i) => s + i.articulo.precio * i.qty, 0);
  }
  get count() {
    return this.items.reduce((n, i) => n + i.qty, 0);
  }

  submit(f: NgForm) {
    this.error = undefined;
    this.success = undefined;

    if (!f.valid) {
      this.error = 'Revisa los campos obligatorios.';
      return;
    }
    if (!this.cart.snapshot.clienteId) {
      this.error = 'Debes iniciar sesión para completar la compra.';
      return;
    }
    if (!this.items.length) {
      this.error = 'Tu carrito está vacío.';
      return;
    }

    this.loading = true;

    const payload = {
      clienteId: this.cart.snapshot.clienteId!,
      items: this.items.map(l => ({ articuloId: l.articulo.id, qty: l.qty })),
      shippingAddress: this.address,
      paymentMethod: this.payment,
      cardLast4: this.payment === 'card' ? this.cardNumber.slice(-4) : null,
      total: this.total
    };

    this.orders.createOrder(payload).subscribe({
      next: res => {
        this.orderId = res.orderId;
        this.success = `¡Compra realizada con éxito! #${res.orderId} 🎉`;
        this.cart.clear();
        this.loading = false;
      },
      error: err => {
        this.error = err?.error?.error || 'No se pudo completar la compra';
        this.loading = false;
      }
    });
  }
}
