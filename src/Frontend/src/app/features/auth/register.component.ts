import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass'],
})
export class RegisterComponent {
  email = '';
  password = '';
  nombre = '';
  apellidos = '';
  direccion = '';
  loading = false;
  error?: string;
  success?: string;

  constructor(private auth: AuthService, private router: Router) {}

  submit(f: NgForm) {
    this.error = undefined;
    this.success = undefined;

    if (!f.valid) {
      this.error = 'Revisa los campos obligatorios.';
      return;
    }

    this.loading = true;
    this.auth.register({
      email: this.email,
      password: this.password,
      nombre: this.nombre,
      apellidos: this.apellidos,
      direccion: this.direccion,
    }).subscribe({
      next: () => {
        this.loading = false;
        this.success = 'Cuenta creada con éxito 🎉';
        setTimeout(() => this.router.navigateByUrl('/'), 2000);
      },
      error: (err) => {
        this.error = err?.error?.error || 'No se pudo registrar';
        this.loading = false;
      },
    });
  }
}
