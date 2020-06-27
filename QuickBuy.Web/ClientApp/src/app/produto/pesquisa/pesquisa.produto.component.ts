import { Component, OnInit } from "@angular/core";
import { Produto } from "../../modelo/produto";
import { ProdutoServico } from "../../servicos/produto/produto.servico";
import { Router } from "@angular/router";

@Component({
  selector: "pesquisa-produto",
  templateUrl: "./pesquisa.produto.component.html",
  styleUrls: ["./pesquisa.produto.component.css"]
})

export class PesquisaProdutoComponent implements OnInit {
  public produtos: Produto[];
    mensagem: string;

  ngOnInit(): void {

  }

  constructor(private produtoServico: ProdutoServico, private router: Router) {
    this.produtoServico.obterTodosProdutos()
      .subscribe(
        produtos => {
          this.produtos = produtos;
        },
        e => {
          console.log(e.error)
        });
  }

  public adicionarProduto() {
    sessionStorage.setItem('produtoSession', "");
    this.router.navigate(['/produto']);
  }

  public deletarProduto(produto: Produto) {
    var retorno = confirm("Deseja realmente deletar o produto selecionado?");
    if (retorno == true) {
      this.produtoServico.deletar(produto).subscribe(
        produtos => {
          this.produtos = produtos;
          console.log(produtos);
        },
        e => {
          this.mensagem = this.retornarMensagemErro(e.status);
        }
      );
    }
  }

  public editarProduto(produto: Produto) {
    sessionStorage.setItem('produtoSession', JSON.stringify(produto));
    this.router.navigate(['/produto']);
  }

  retornarMensagemErro(status: any): string {
    switch (status) {
      case 401:
        return "Você precisa estar logado para excluir um produto!!!";
      case 403:
        return "Você não tem permissão para excluir produtos!!!";
      default:
        return "";
    }
  }
}

