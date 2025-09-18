import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ArticuloDetailDto, ArticuloListItemDto } from '../models/articulo';

@Injectable({ providedIn: 'root' })
export class ArticuloService {

  private base = `${environment.apiUrl}/articulos`;

  constructor(private http: HttpClient) {}

  list(): Observable<ArticuloListItemDto[]> {
    return this.http.get<ArticuloListItemDto[]>(this.base);
  }
  
  get(id: number): Observable<ArticuloDetailDto> {
    return this.http.get<ArticuloDetailDto>(`${this.base}/${id}`);
  }

}
