import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ArticuloTiendaListItemDto } from '../models/articulo-tienda';

@Injectable({ providedIn: 'root' })
export class ArticuloTiendaService {
  private base = `${environment.apiUrl}/articuloTienda`;

  constructor(private http: HttpClient) {}

  list(tiendaId?: number, articuloId?: number): Observable<ArticuloTiendaListItemDto[]> {
    let params = new HttpParams();
    if (tiendaId) params = params.set('tiendaId', tiendaId);
    if (articuloId) params = params.set('articuloId', articuloId);

    return this.http.get<ArticuloTiendaListItemDto[]>(this.base, { params });
  }
}
