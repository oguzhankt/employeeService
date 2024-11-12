using EmployeeService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EmployeeService.Services;


public class AuthGuardService : IAuthGuardService
{
    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AuthGuardUser> _userManager;
    private readonly IUserRefreshTokenService _userRefreshTokenService;

    public AuthGuardService(
        IOptions<List<Client>> optionsClient,
        ITokenService tokenService, 
        UserManager<AuthGuardUser> userManager,
        IUserRefreshTokenService userRefreshTokenService
        )
    {
        _clients = optionsClient.Value;
        _tokenService = tokenService;
        _userManager = userManager;
        _userRefreshTokenService = userRefreshTokenService;
    }

    public async Task<Token?> CreateTokenAsync(Login loginDto)
    {
        if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return null;

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return null;
        }
        var token = _tokenService.CreateToken(user);

        var userRefreshToken =  _userRefreshTokenService.GetUserRefreshTokenById(user.Id);

        if (userRefreshToken == null)
        {
            await _userRefreshTokenService.AddUserRefreshToken(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }
        
        return token;
    }

    public ClientToken? CreateTokenByClient(ClientLogin clientLoginDto)
    {
        var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

        if (client == null)
        {
            return null;
        }

        var clientToken = _tokenService.CreateTokenByClient(client);

        return clientToken;
    }

    public async Task<Token?> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefreshToken = _userRefreshTokenService.GetUserRefreshTokenByCode(refreshToken);

        if (existRefreshToken == null)
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

        if (user == null)
        {
            return null;
        }

        var createdToken = _tokenService.CreateToken(user);

        existRefreshToken.Code = createdToken.RefreshToken;
        existRefreshToken.Expiration = createdToken.RefreshTokenExpiration;

        return createdToken;
    }

    public bool RevokeRefreshToken(string refreshToken)
    {
        var existRefreshToken = _userRefreshTokenService.GetUserRefreshTokenByCode(refreshToken);
        if (existRefreshToken == null)
        {
            return false;
        }

        _userRefreshTokenService.RemoveUserRefreshTokenById(existRefreshToken.UserId);

        return true;
    }
}
