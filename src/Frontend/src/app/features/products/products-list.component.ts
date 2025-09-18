import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { ArticuloListItemDto } from '../../core/models/articulo';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { ArticuloTiendaService } from '../../core/services/articulo-tienda.service';
import { ArticuloTiendaListItemDto } from '../../core/models/articulo-tienda';

@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.sass'],
})
export class ProductsListComponent implements OnInit {
  
  items: ArticuloListItemDto[] = [];
  loading = false;

  constructor(
    private api: ArticuloTiendaService, 
    private cart: CartService, 
    public auth: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.api.list().subscribe({
      next: (d: ArticuloTiendaListItemDto[]) => {
        this.items = d.map(x => ({
          id: x.articuloId,
          codigo: x.articuloCodigo,
          descripcion: x.articuloDescripcion,
          precio: x.precio,
          imagenUrl: x.articuloImagenUrl,
          stock: x.stock,
          tiendaSucursal: x.tiendaSucursal,
          tiendaId: x.tiendaId,
        }));
        this.loading = false;
      },
      error: () => this.loading = false,
    });
  }
  
  addToCart(item: ArticuloListItemDto) {
    const res = this.cart.add(item, 1);
    if (res.ok) {
      this.toastr.success(`Añadido: ${item.descripcion}`, 'Carrito');
    } else if (res.reason === 'out_of_stock') {
      this.toastr.error(`Ya no hay stock disponible de "${item.descripcion}".`, 'Sin stock');
    } else if (res.reason === 'limited') {
      this.toastr.warning(
        `Solo hay ${res.max} pza(s) de "${item.descripcion}". Se ajustó al máximo.`,
        'Stock limitado'
      );
    }
  }

  get nombreUsuario(): string {
    return this.auth.snapshot?.nombreCompleto ?? 'Cliente';
  }
}
