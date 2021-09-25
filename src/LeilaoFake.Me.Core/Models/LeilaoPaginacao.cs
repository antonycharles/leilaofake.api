using System.Collections.Generic;
using System.Linq;

namespace LeilaoFake.Me.Core.Models
{
    public class LeilaoPaginacao
    {
        public LeilaoPaginacao(int? porPagina = 10, int? pagina = 1, string order = null, string search = null, string leiloadoPorId = null)
        {
            PorPagina = porPagina != null ? porPagina.Value : 10;
            Pagina = pagina != null ? pagina.Value : 1;
            Order = order;
            Search = search;
            LeiloadoPorId = leiloadoPorId != null ? leiloadoPorId : null;
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
                var properties = typeof(Leilao).GetProperties();
                if(value != null && properties.Where(x => x.Name.ToLower() == value.ToLower()).Any())
                    _order = value;
                else
                    _order = "titulo";
            }
        }
        public string LeiloadoPorId { get; set; }
        public string Search { get; set; }
        public string SearchDb => this.Search != null ? "%" + this.Search + "%" : null;
        public bool IsPublico => this.LeiloadoPorId == null ? true : false;
        public int Pagina { get; set; }
        public int? Total { get; set; }

        public IList<Leilao> Resultados { get; set; }
    }
}