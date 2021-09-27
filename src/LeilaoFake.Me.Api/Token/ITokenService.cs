using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Core.Models;

namespace LeilaoFake.Me.Api.Token
{
    public interface ITokenService
    {
         LoginResponse GenerateToken(Usuario user);
    }
}