using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuickBuy.Dominio.Entidades
{
    public abstract class Entidade
    {
        private List<string> _mensagensValidacao { get; set; }
        private List<string> _mensagemValidacao
        {
            get
            {
                return _mensagensValidacao ?? (_mensagensValidacao = new List<string>());
            }
        }
        protected void LimparMensagensValidacao()
        {
            _mensagemValidacao.Clear();
        }

        protected void AdicionarCritica(string mensagem)
        {
            _mensagemValidacao.Add(mensagem);
        }

        public string ObterMensagensValidacao()
        {
            return string.Join(". ", _mensagemValidacao); 
        }
        public abstract void Validate();
        public bool EhValido
        {
            get
            {
                return !_mensagemValidacao.Any(); 
            }
        }
    }
}
