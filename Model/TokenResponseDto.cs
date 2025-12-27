namespace WebApplicationDemo.Model
{
    /// <summary>
    /// Class hold the access and refresh token info
    /// </summary>
    public class TokenResponseDto
    {
        /// <summary>
        /// Get or set the access token.
        /// </summary>
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// Get or set the refresh token
        /// </summary>
        public string RefreshToken { get; set; } = null!;
    }
}
