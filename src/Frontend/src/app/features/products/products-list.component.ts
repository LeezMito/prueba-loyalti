import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { ArticuloItemDto } from '../../core/models/articulo';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { ArticuloService } from '../../core/services/articulo.service';

@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.sass'],
})
export class ProductsListComponent implements OnInit {
  
  items: ArticuloItemDto[] = [];
  loading = false;

  constructor(
    private api: ArticuloService, 
    private cart: CartService, 
    public auth: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.api.list().subscribe({
      next: (d: ArticuloItemDto[]) => {
        this.items = d.map(x => ({
          id: x.id,
          codigo: x.codigo,
          descripcion: x.descripcion,
          precio: x.precio,
          imagenUrl: x.imagenUrl,
          stock: x.stock,
          tiendas: x.tiendas,
        }));
        this.loading = false;
      },
      error: () => this.loading = false,
    });
  }
  
  async addToCart(item: ArticuloItemDto) {
    const res = await this.cart.add(item, 1);
    if (res.ok) {
      this.toastr.success(`Añadido: ${item.descripcion}`, 'Carrito');
    } else if (res.reason === 'out_of_stock') {
      this.toastr.error(`Ya no hay stock disponible de "${item.descripcion}".`, 'Sin stock');
    } else if (res.reason === 'limited') {
      this.toastr.warning(`Solo hay ${res.max} pza(s). Se ajustó al máximo.`, 'Stock limitado');
    }
  }

  get nombreUsuario(): string {
    return this.auth.snapshot?.nombreCompleto ?? 'Cliente';
  }
}
