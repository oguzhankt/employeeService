using EmployeeService.Models;

namespace EmployeeService.Services;

public class UserRefreshTokenService : IUserRefreshTokenService
{
    private readonly UserDb _dbContext;

    public UserRefreshTokenService( UserDb dbContext )
    {
        _dbContext = dbContext;
    }
    
    public UserRefreshToken? GetUserRefreshTokenById(string id)
    {
        var foundUser = _dbContext.UserRefreshToken.Find(id);
        return foundUser;
    }

    public UserRefreshToken? GetUserRefreshTokenByCode(string code)
    {
        var foundUser = _dbContext.UserRefreshToken.Where(c => c.Code == code).FirstOrDefault();
        return foundUser;
    }

    public async Task<bool> AddUserRefreshToken(UserRefreshToken userRefreshToken)
    {
        try
        {
            _dbContext.Add(userRefreshToken);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public bool RemoveUserRefreshTokenById(string id)
    {
        var foundUser = _dbContext.UserRefreshToken.Find(id);

        if (foundUser == null)
        {
            return false;
        }
            
        _dbContext.Remove(foundUser);
        _dbContext.SaveChanges();
        return true;
    }
}