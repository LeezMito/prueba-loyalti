
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ClienteItemDto } from '../models/cliente';
@Injectable({ providedIn: 'root' })
export class ClienteService {
  private base = `${environment.apiUrl}/clientes`;
  constructor(private http: HttpClient) {}
  list(): Observable<ClienteItemDto[]> { return this.http.get<ClienteItemDto[]>(this.base); }
}
