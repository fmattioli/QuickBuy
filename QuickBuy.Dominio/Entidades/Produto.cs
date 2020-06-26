using System;
using System.Collections.Generic;
using System.Text;

namespace QuickBuy.Dominio.Entidades
{
    public class Produto : Entidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string NomeArquivo { get; set; }

        public override void Validate()
        {

            if (string.IsNullOrEmpty(Nome))
            {
                AdicionarCritica("Nome não foi informado");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AdicionarCritica("Descricao não foi informado");
            }

            if (Preco == 0)
            {
                AdicionarCritica("Preco não foi informado");
            }
        }
    }
}
