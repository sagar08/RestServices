using JwtAuthWithRefreshToken.Models;

namespace JwtAuthWithRefreshToken.Services.Contracts
{
    public interface IAuthService
    {
        AuthenticateResponse Authenticate(AuthenticateModel model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
    }
}