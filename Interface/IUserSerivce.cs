using WebApplicationDemo.Entities;
using WebApplicationDemo.Model;

namespace WebApplicationDemo.Interface
{
    /// <summary>
    /// The User Service Interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="userDto">The user data transfer object</param>
        /// <returns>The register user</returns>
        Task<User?> Register(UserDto userDto);

        /// <summary>
        /// Login the user and return the token
        /// </summary>
        /// <param name="userDto">The user data transfer object</param>
        /// <returns>The access and refresh token</returns>
        public Task<TokenResponseDto?> Login(UserDto userDto);

        /// <summary>
        /// The update the token using refresh token validation.
        /// </summary>
        /// <param name="refreshTokenRequest">The refresh token with user id</param>
        /// <returns>The new access token with refresh token</returns>
        public Task<TokenResponseDto?> UpdateToken(RefreshTokenRequestDto refreshTokenRequest);
    }
}
