using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using EmployeeService.Helpers;
using EmployeeService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeService.Services;


public class TokenService : ITokenService
{
    private readonly UserManager<AuthGuardUser> _userManager;

    private readonly CustomToken _tokenOption;

    public TokenService(UserManager<AuthGuardUser> userManager, IOptions<CustomToken> options)
    {
        _userManager = userManager;
        _tokenOption = options.Value;
    }

    private string CreateRefreshToken()

    {
        var numberByte = new Byte[32];

        using var rnd = RandomNumberGenerator.Create();

        rnd.GetBytes(numberByte);

        return Convert.ToBase64String(numberByte);
    }

    private IEnumerable<Claim> GetClaims(AuthGuardUser userApp, List<String> audiences)
    {
        var userList = new List<Claim> {
        new Claim(ClaimTypes.NameIdentifier,userApp.Id),
        new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
        new Claim(ClaimTypes.Name,userApp.UserName),
        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        return userList;
    }

    private IEnumerable<Claim> GetClaimsByClient(Client client)
    {
        var claims = new List<Claim>();
        claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
        new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());

        return claims;
    }

    public Token CreateToken(AuthGuardUser userApp)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
        var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
        var securityKey = SignHelper.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOption.Issuer,
            expires: accessTokenExpiration,
             notBefore: DateTime.Now,
             claims: GetClaims(userApp, _tokenOption.Audience),
             signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(jwtSecurityToken);

        var newToken = new Token
        {
            AccessToken = token,
            RefreshToken = CreateRefreshToken(),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration,
        };

        return newToken;
    }

    public ClientToken CreateTokenByClient(Client client)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);

        var securityKey = SignHelper.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOption.Issuer,
            expires: accessTokenExpiration,
             notBefore: DateTime.Now,
             claims: GetClaimsByClient(client),
             signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(jwtSecurityToken);

        var clientToken = new ClientToken
        {
            AccessToken = token,

            AccessTokenExpiration = accessTokenExpiration,
        };

        return clientToken;
    }
}
