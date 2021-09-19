namespace LeilaoFake.Me.Core.Models
{
    public class UsuarioPaginacao
    {
        public UsuarioPaginacao(int? porPagina, int? pagina)
        {
            PorPagina = porPagina != null ? porPagina.Value : 10;
            Pagina = pagina != null ? pagina.Value : 1;
        }

        public int PorPagina { get; set; }
        public int Pagina { get; set; }


    }
}