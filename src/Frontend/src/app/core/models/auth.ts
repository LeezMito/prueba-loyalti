export interface RegisterRequest {
  email: string;
  password: string;
  nombre: string;
  apellidos: string;
  direccion?: string | null;
}
export interface LoginRequest { email: string; password: string; }
export interface AuthResponse {
  accessToken: string;
  clienteId: number;
  email: string;
  nombreCompleto: string;
}
export interface AuthState {
  token?: string;
  email?: string;
  clienteId?: number;
  nombreCompleto?: string;
}
