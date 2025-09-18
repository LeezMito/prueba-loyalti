import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass'],
})
export class LoginComponent {
  email = '';
  password = '';
  loading = false;
  error?: string;

  constructor(private auth: AuthService, private router: Router) {}

  submit() {
    this.error = undefined;
    this.loading = true;
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: (_) => this.router.navigateByUrl('/'),
      error: (err) => {
        this.error = err?.error?.error || 'Credenciales inválidas';
        this.loading = false;
      },
    });
  }
}
