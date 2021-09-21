using System.Collections.Generic;
using System.Linq;

namespace LeilaoFake.Me.Core.Models
{
    public class UsuarioPaginacao
    {
        public UsuarioPaginacao(int? porPagina, int? pagina, string order = null)
        {
            PorPagina = porPagina != null ? porPagina.Value : 10;
            Pagina = pagina != null ? pagina.Value : 1;
            Order = order;
        }

        private int _porPagina;
        public int PorPagina { 
            get
            {
                return _porPagina;
            } 
            set
            {
                if(value < 50)
                    _porPagina = value;
                else
                    _porPagina = 50;
            }
        }

        private string _order;
        public string Order{
            get
            {
                return _order;
            }
            set
            {
                var properties = typeof(Usuario).GetProperties();
                if(value != null && properties.Where(x => x.Name.ToLower() == value.ToLower()).Any())
                    _order = value;
                else
                    _order = "nome";
            }
        }

        public int Pagina { get; set; }
        public int? Total { get; set;}

        public IList<Usuario> Resultados { get; set; }


    }
}