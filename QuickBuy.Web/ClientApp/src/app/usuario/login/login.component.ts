import { Component, OnInit } from "@angular/core";
import { Usuario } from "../../modelo/usuario";
import { Router, ActivatedRoute } from "@angular/router";
import { UsuarioServico } from "../../servicos/usuario/usuario.servico";


@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]


})
export class LoginComponent implements OnInit {

  public usuario;
  public retornUrl: string;
  public mensagem: string;
  public ativarSpinner: boolean;

  constructor(private router: Router, private activatedRouter: ActivatedRoute, private usuarioServico: UsuarioServico) {

  }

  ngOnInit(): void {
    this.retornUrl = this.activatedRouter.snapshot.queryParams['returnUrl'];
    this.usuario = new Usuario();
  }

  entrar(): void {
    this.ativarSpinner = true;
    this.usuarioServico.verificarUsuario(this.usuario)
      .subscribe(
        usuario_json => {
          //retorno sem erros
          this.usuarioServico.usuario = usuario_json;
          if (this.retornUrl == null) {
            this.router.navigate(['/']);
          }
          else {
            this.router.navigate([this.retornUrl]);
          }
        },
        err => {
          console.log(err.error);
          this.mensagem = err.error;
          this.ativarSpinner = false;
        }
      );

  }
}
