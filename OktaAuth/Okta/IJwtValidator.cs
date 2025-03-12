using System.IdentityModel.Tokens.Jwt;

namespace OktaAuth.Okta
{
    public interface IJwtValidator
    {
        Task<JwtSecurityToken> ValidateToken(string token, CancellationToken ct = default(CancellationToken));
    }
}
