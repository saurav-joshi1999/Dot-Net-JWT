namespace WebApplicationDemo.Model
{
    /// <summary>
    /// The class hold the refresh token with user id.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// Get or set the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Get or set the refresh token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
