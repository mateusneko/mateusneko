using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Ports;
namespace TaskFlow.Api.Security;
public sealed class JwtTokenService(string key, string issuer, string audience) : ITokenService
{
    public (string Token, DateTime ExpiresAtUtc) Issue(Account account)
    {
        var expires = DateTime.UtcNow.AddHours(2);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience,
            claims: [new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                     new Claim(ClaimTypes.Name, account.Name),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())],
            expires: expires, signingCredentials: credentials);
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
