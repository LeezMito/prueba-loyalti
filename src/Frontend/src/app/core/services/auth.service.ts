import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest, AuthState } from '../models/auth';
import { environment } from '../../environments/environment';

const LS_KEY = 'inventory_auth_v1';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private state$ = new BehaviorSubject<AuthState>(this.read());
  constructor(private http: HttpClient) {}
  private read(): AuthState {
    const raw = localStorage.getItem(LS_KEY);
    return raw ? JSON.parse(raw) : {};
  }
  private write(state: AuthState) {
    localStorage.setItem(LS_KEY, JSON.stringify(state));
    this.state$.next({ ...state });
  }
  get snapshot(): AuthState {
    return this.state$.value;
  }
  select(): Observable<AuthState> {
    return this.state$.asObservable();
  }
  register(body: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${environment.apiUrl}/auth/register`, body)
      .pipe(tap((res) => this.saveAuth(res)));
  }
  login(body: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${environment.apiUrl}/auth/login`, body)
      .pipe(tap((res) => this.saveAuth(res)));
  }
  logout() {
    localStorage.removeItem(LS_KEY);
    this.state$.next({});
  }
  private saveAuth(res: AuthResponse) {
    this.write({
      token: res.accessToken,
      email: res.email,
      clienteId: res.clienteId,
      nombreCompleto: res.nombreCompleto,
    });
  }
}
