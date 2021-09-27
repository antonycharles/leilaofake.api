using System;

namespace LeilaoFake.Me.Api.Responses
{
    public class LoginResponse
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }
        public UsuarioResponse Usuario { get; set; }

        public LoginResponse(DateTime created, DateTime expiration, string token, UsuarioResponse usuario){
            Authenticated = true;
            Created = created.ToString("yyyy-MM-dd HH:mm:ss");
            Expiration = expiration.ToString("yyyy-MM-dd HH:mm:ss");
            AccessToken = token;
            Message = "OK";
            Usuario = usuario;
        }
    }
}