using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplicationDemo.Data;
using WebApplicationDemo.Entities;
using WebApplicationDemo.Interface;
using WebApplicationDemo.Model;

namespace WebApplicationDemo.Service;

/// <summary>
/// User Service class
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    /// User Service with config and context
    /// </summary>
    /// <param name="config">The app-settings config</param>
    /// <param name="dbContext">The db context</param>
    public UserService(IConfiguration config, UserDbContext dbContext)
    {
        _config = config;
        _userDbContext = dbContext;
    }

    #region Public Methods

    /// <summary>
    /// Register the user
    /// </summary>
    /// <param name="userDto">The user data transfer object</param>
    /// <returns>The register user</returns>
    public async Task<User?> Register(UserDto userDto)
    {
        User? alreadyPresentUser =
            await _userDbContext.Users.FirstOrDefaultAsync(user => user.UserName == userDto.UserName);

        if (alreadyPresentUser != null)
            return null;

        User newUser = new User
        {
            UserName = userDto.UserName,
            HashPassword = new PasswordHasher<User>().HashPassword(new User(), userDto.Password),
            Role = userDto.Role
        };

        await _userDbContext.Users.AddAsync(newUser);
        await _userDbContext.SaveChangesAsync();
        return newUser;
    }

    /// <summary>
    /// Login the user and return the token
    /// </summary>
    /// <param name="userDto">The user data transfer object</param>
    /// <returns>The access and refresh token</returns>
    public async Task<TokenResponseDto?> Login(UserDto userDto)
    {
        User? registerUser =
            await _userDbContext.Users.FirstOrDefaultAsync(user => user.UserName == userDto.UserName);

        if (registerUser == null)
            return null;

        PasswordVerificationResult passwordVerificationResult =
            new PasswordHasher<User>().VerifyHashedPassword(registerUser, registerUser.HashPassword,
                userDto.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return null;

        string accessToken =  CreateToken(registerUser);
        string refreshToken = CreateRefreshToken(registerUser);

        return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    /// <summary>
    /// The update the token using refresh token validation.
    /// </summary>
    /// <param name="refreshTokenRequest">The refresh token with user id</param>
    /// <returns>The new access token with refresh token</returns>
    public async Task<TokenResponseDto?> UpdateToken(RefreshTokenRequestDto refreshTokenRequest)
    {
        User? loginUser = await _userDbContext.Users.FindAsync(refreshTokenRequest.UserId);

        if (loginUser == null || loginUser.RefreshTokenVal != refreshTokenRequest.RefreshToken || loginUser.RefreshTokenExpiry < DateTime.UtcNow)
            return null;

        return new TokenResponseDto()
        {
            AccessToken = CreateToken(loginUser),
            RefreshToken = CreateRefreshToken(loginUser)
        };
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Create the token for valid login user
    /// </summary>
    /// <param name="availableUser">The register login user</param>
    /// <returns>The token response dto</returns>
    private string CreateToken(User availableUser)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.Name, availableUser.UserName),
            new(ClaimTypes.Role, availableUser.Role),
            new(ClaimTypes.Sid, availableUser.Id.ToString())
        ];

        SymmetricSecurityKey key =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("AppSettings:Token")!));
        SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken tokenDescriptor = new JwtSecurityToken(
            claims: claims,
            issuer: _config.GetValue<string>("AppSettings:Issuer"),
            audience: _config.GetValue<string>("AppSettings:Audience"),
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    /// <summary>
    /// Create the refresh token for register login user.
    /// </summary>
    /// <param name="registerUser">The login user</param>
    /// <returns>The refresh token</returns>
    private string CreateRefreshToken(User registerUser)
    {
        byte[] randomNumber = new byte[32];
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        string refreshToken = Convert.ToBase64String(randomNumber);
        registerUser.RefreshTokenVal = refreshToken;
        registerUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

        _userDbContext.SaveChangesAsync();
        return refreshToken;
    }

    #endregion

    #region Private Fields

    /// <summary>
    /// The user db context
    /// </summary>
    private readonly UserDbContext _userDbContext;

    /// <summary>
    /// The appSettings config info
    /// </summary>
    private readonly IConfiguration _config;

    #endregion
}