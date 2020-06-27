using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickBuy.Web.ViewModel
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        //um ou nenhum pedido
        //public virtual ICollection<Pedido> Pedidos { get; set; }
        public string Nivel { get; set; }
        public string Token { get; set; }
    }
}
