import { Component, OnInit } from "@angular/core";
import { Produto } from "../modelo/produto";
import { ProdutoServico } from "../servicos/produto/produto.servico";
import { Router } from "@angular/router";

@Component({
  selector: "app-produto",
  templateUrl: "./produto.component.html",
  styleUrls: ["./produto.component.css"]
})

export class ProdutoComponent implements OnInit {
  public produto: Produto;
  public arquivoSelecionado: File;
  public ativar_spinner: boolean;
  public mensagem: string;

  constructor(private produtoServico: ProdutoServico, private router: Router) {

  }

  public inputChange(files: FileList) {
    this.arquivoSelecionado = files.item(0);
    this.ativar_spinner = true;
    this.produtoServico.enviarArquivo(this.arquivoSelecionado).subscribe(
      nomeArquivo => {
        this.produto.nomeArquivo = nomeArquivo;
        console.log(nomeArquivo);
        this.ativarEspera(false);
      },
      erro => {
        console.log(erro.error);
        this.ativarEspera(false);
      }
    );
  }
  ngOnInit(): void {
    var produtoSession = sessionStorage.getItem('produtoSession');
    if (produtoSession) {
      this.produto = JSON.parse(produtoSession);
    } else {
      this.produto = new Produto();
    }


  }
  public nome: string;

  public obterNome(): string {
    return "samsung";
  }



  public cadastrar() {
    this.ativarEspera(true);
    this.produtoServico.cadastrar(this.produto)
      .subscribe(
        data => {
          console.log(data);
          this.ativarEspera(false);
          this.router.navigate(['/pesquisar-produto']);
        },
        e => {
          console.log(e.error);
          this.mensagem = e.error;
          this.ativarEspera(false);
        }
      )
  }

  public ativarEspera(ativar: boolean) {
    this.ativar_spinner = ativar;
  }
}
