namespace LeilaoFake.Me.Core.Models
{
    public class LeilaoImagem
    {
        public int Id { get; private set; }
        public string LeiloadoPorId { get; private set; }
        public string LeilaoId { get; private set; }
        public string Url { get; private set; }

        public LeilaoImagem() {}
        public LeilaoImagem(string leiloadoPorId, string leilaoId, string url)
        {
            LeiloadoPorId = leiloadoPorId;
            LeilaoId = leilaoId;
            Url = url;
        }
    }
}