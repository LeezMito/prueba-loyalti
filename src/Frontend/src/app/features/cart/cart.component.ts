import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms'; // 👈 AÑADE ESTO
import { CartService } from '../../core/services/cart.service';
import { cartTotal } from '../../core/models/cart';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.sass'],
})
export class CartComponent {
  vm$!: Observable<any>;
  vm: any;

  constructor(public cart: CartService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.vm$ = this.cart.select();
    this.vm$.subscribe((state) => (this.vm = state));
  }

  total(state: any) {
    return cartTotal(state);
  }
  inc(it: any) {
    const res = this.cart.updateQty(it.articulo.id, it.qty + 1);
    if (!res.ok && res.reason === 'limited') {
      this.toastr.warning(`Stock máximo: ${res.max} pza(s).`, 'Stock limitado');
    } else if (!res.ok && res.reason === 'out_of_stock') {
      this.toastr.error('Sin stock disponible.', 'Carrito');
    }
  }
  dec(it: any) {
    if (it.qty > 1) this.cart.updateQty(it.articulo.id, it.qty - 1);
  }
  updateQty(it: any, val: number) {
    const q = Math.max(1, Number(val) || 1);
    const res = this.cart.updateQty(it.articulo.id, q);
    if (!res.ok && res.reason === 'limited') {
      this.toastr.warning(`Stock máximo: ${res.max} pza(s).`, 'Stock limitado');
    } else if (!res.ok && res.reason === 'out_of_stock') {
      this.toastr.error('Sin stock disponible.', 'Carrito');
    }
  }

  remove(it: any) {
    this.cart.remove(it.articulo.id);
  }
  checkout() {
    this.router.navigateByUrl('/checkout');
  }

  get totalArticulos() {
    return this.vm?.items?.reduce((n: number, i: any) => n + i.qty, 0) ?? 0;
  }
  get totalSubtotal() {
    return this.vm?.items?.reduce((s: number, i: any) => s + i.articulo.precio * i.qty, 0) ?? 0;
  }
}
