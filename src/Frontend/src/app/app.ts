import { Component, signal } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { CommonModule } from '@angular/common';
import { CartService } from './core/services/cart.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, RouterModule],
  templateUrl: './app.html',
  styleUrl: './app.sass',
})
export class App {
  
  protected readonly title = signal('tienda');
  
  constructor(
    private auth: AuthService, 
    private router: Router,
    public cart: CartService
  ) {}
  
  logout() {
    this.auth.logout();
    this.router.navigateByUrl('/login');
  }
  
  get hideNavbar(): boolean {
    const noNav = ['/login', '/register'];
    return noNav.includes(this.router.url);
  }

  get totalArticulos() {
    return this.cart.snapshot.items.reduce((n, i) => n + i.qty, 0);
  }  
}
