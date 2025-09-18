import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ClienteArticuloCreateDto } from '../models/cliente-articulo';
import { ClienteArticuloItemDto } from '../models/cliente-articulo';

@Injectable({ providedIn: 'root' })
export class ClienteArticuloService {
  private base = `${environment.apiUrl}/clienteArticulo`;
  constructor(private http: HttpClient) {}

  create(dto: ClienteArticuloCreateDto): Observable<void> {
    return this.http.post<void>(this.base, dto);
  }

  delete(clienteId: number, articuloId: number, fechaISO: string): Observable<void> {
    const params = new HttpParams()
      .set('clienteId', clienteId)
      .set('articuloId', articuloId)
      .set('fecha', fechaISO);
    return this.http.delete<void>(this.base, { params });
  }

  listByCliente(clienteId: number): Observable<ClienteArticuloItemDto[]> {
    return this.http.get<ClienteArticuloItemDto[]>(`${this.base}/by-cliente/${clienteId}`);
  }
}