import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Pedido } from "../../modelo/pedido";
import { Observable } from "rxjs";
import { UsuarioServico } from "../usuario/usuario.servico";

@Injectable({
  providedIn:'root'
})
export class PedidoServico {
  public _baseURL: string;
  public tokenFull: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, public usuarioServico: UsuarioServico) {
    this._baseURL = baseUrl;
  }
  get headers(): HttpHeaders {
    return new HttpHeaders().append('content-type', 'application/json');

  }

  public efetivarCompra(pedido: Pedido): Observable<number> {
    this.tokenFull = "Bearer" + " " + this.usuarioServico.usuario.token;
    return this.http.post<number>(this._baseURL + "api/pedido", JSON.stringify(pedido), { headers: this.headers.append('Authorization', this.tokenFull) });
  }
}
