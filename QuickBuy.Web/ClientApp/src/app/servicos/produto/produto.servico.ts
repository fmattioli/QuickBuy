import { Injectable, Inject, inject, OnInit } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { Produto } from "../../modelo/produto";
import { UsuarioServico } from "../usuario/usuario.servico";

@Injectable({
  providedIn: "root"
})
export class ProdutoServico implements OnInit {

  private _baseURL: string;
  public produtos: Produto[];
  private tokenFull: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private usuarioServico: UsuarioServico) {
    this._baseURL = baseUrl;
  }

  ngOnInit(): void {
    this.produtos = [];
  }

  get headers(): HttpHeaders {
    return new HttpHeaders().append('content-type', 'application/json');
    
  }

  get headersFile(): HttpHeaders {
    return new HttpHeaders().append('Authorization', this.tokenFull);

  }


  public cadastrar(produto: Produto): Observable<Produto> {
    this.tokenFull = "Bearer" + " " + this.usuarioServico.usuario.token;
    return this.http.post<Produto>(this._baseURL + "api/produto", JSON.stringify(produto), { headers: this.headers.append('Authorization', this.tokenFull) });
  }

  public salvar(produto: Produto): Observable<Produto> {

    return this.http.post<Produto>(this._baseURL + "api/produto/salvar", JSON.stringify(produto), { headers: this.headers });
  }

  public editar(produto: Produto): Observable<Produto> {

    return this.http.post<Produto>(this._baseURL + "api/produto/atualizarProduto", JSON.stringify(produto), { headers: this.headers });
  }

  public deletar(produto: Produto): Observable<Produto[]> {
    this.tokenFull = "Bearer" + " " + this.usuarioServico.usuario.token;
    return this.http.post<Produto[]>(this._baseURL + "api/produto/deletar", JSON.stringify(produto), { headers: this.headers.append('Authorization', this.tokenFull) });
  }

  public obterTodosProdutos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this._baseURL + "api/produto");
  }

  public obterProduto(produtoId: number): Observable<Produto> {
    return this.http.get<Produto>(this._baseURL + "api/produto");
  }

  public enviarArquivo(arquivoSelecionado: File): Observable<string> {
    this.tokenFull = "Bearer" + " " + this.usuarioServico.usuario.token;
    const formData: FormData = new FormData();
    formData.append("arquivoEnviado", arquivoSelecionado, arquivoSelecionado.name);
    return this.http.post<string>(this._baseURL + "api/produto/enviarArquivo", formData, { headers: this.headersFile});
  }


  private setToken():string {
    this.tokenFull = "Bearer " + sessionStorage.getItem("token");
    return this.tokenFull;
  }
}
