using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace LeilaoFake.Me.Api.Token
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfigurations _tokenConfig;

        public TokenService(TokenConfigurations tokenConfigurations)
        {
            _tokenConfig = tokenConfigurations;
        }

        public LoginResponse GenerateToken(Usuario user)
        {
            DateTime dataCriacao = DateTime.UtcNow;
            DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(_tokenConfig.Seconds);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenConfig.Issuer,
                Audience = _tokenConfig.Audience,
                Subject = CreateClaimsIndentity(user),
                Expires = dataExpiracao,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponse(dataCriacao, dataExpiracao, tokenHandler.WriteToken(token), new UsuarioResponse(user,null,null));
        }

        private ClaimsIdentity CreateClaimsIndentity(Usuario user)
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Nome.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

            return claimsIdentity;
        }
    }
}