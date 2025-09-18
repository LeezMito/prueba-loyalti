import { Routes } from '@angular/router';
import { ProductsListComponent } from './features/products/products-list.component';
import { CartComponent } from './features/cart/cart.component';
import { LoginComponent } from './features/auth/login.component';
import { RegisterComponent } from './features/auth/register.component';
import { CheckoutComponent } from './features/cart/checkout.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'products', pathMatch: 'full' },
  { path: 'products', component: ProductsListComponent, canActivate: [authGuard] },
  { path: 'cart', component: CartComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'checkout', 
    loadComponent: () => 
      import('./features/cart/checkout.component').then(m => m.CheckoutComponent),
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: 'products' }
];