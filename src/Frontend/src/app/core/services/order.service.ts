import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Injectable } from "@angular/core";
import { map } from "rxjs";

@Injectable({ providedIn: 'root' })
export class OrderService {
  private base = `${environment.apiUrl}/checkout`;

  constructor(private http: HttpClient) {}

  createOrder() {
    return this.http.post<{ success: boolean; errors?: any[] }>(this.base, {}).pipe(
      map((res) => {
        if (!res.success) {
          const detail =
            res.errors?.map((e: any) =>
              `• ${e.codigo} (${e.descripcion}): solicitado ${e.requested}, disponible ${e.available}`
            ).join('\n') || 'Stock insuficiente.';

          throw { message: 'No se pudo confirmar la compra', detail, api: res } as any;
        }

        return {
          orderId: Math.floor(Math.random() * 900000) + 100000,
          createdAt: new Date().toISOString(),
        };
      })
    );
  }
}
