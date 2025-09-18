import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ArticuloTiendaItemDto } from '../models/articulo-tienda';
import { ArticuloItemDto } from '../models/articulo';

@Injectable({ providedIn: 'root' })
export class ArticuloTiendaService {
  private base = `${environment.apiUrl}/articuloTienda`;

  constructor(private http: HttpClient) {}

  list(): Observable<ArticuloItemDto[]> {
    return this.http.get<ArticuloItemDto[]>(this.base);
  }
}
