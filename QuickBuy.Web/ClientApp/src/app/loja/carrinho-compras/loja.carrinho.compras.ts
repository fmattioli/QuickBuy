import { Component, OnInit } from "@angular/core";
import { Produto } from "../../modelo/produto";



export class LojaCarrinhoCompras implements OnInit {
  

  public produtos: Produto[] = [];
  ngOnInit(): void {

  }

  constructor() {

  }

  public adicionar(produto: Produto) {
    var produtoLocalStorage = localStorage.getItem("produtoLocalStorage");
    if (!produtoLocalStorage) {
      this.produtos.push(produto);

    }
    else {
      //caso existam produtos
      this.produtos = JSON.parse(produtoLocalStorage);
      this.produtos.push(produto);

    }
    localStorage.setItem("produtoLocalStorage", JSON.stringify(this.produtos));
  }

  public obterProdutos(): Produto[] {
    var produtoLocalStorage = localStorage.getItem("produtoLocalStorage");
    if (produtoLocalStorage) {
      return JSON.parse(produtoLocalStorage);
    }
  }

  public removerProduto(produto: Produto) {
    var produtoLocalStorage = localStorage.getItem("produtoLocalStorage");
    if (produtoLocalStorage) {
      this.produtos = JSON.parse(produtoLocalStorage);
      this.produtos = this.produtos.filter(p => p.id != produto.id);
      localStorage.setItem("produtoLocalStorage", JSON.stringify(this.produtos));
    }
  }

  atualizar(produtos: Produto[]) {
    localStorage.setItem("produtoLocalStorage", JSON.stringify(produtos));
  }
}
