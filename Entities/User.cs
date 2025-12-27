using System.ComponentModel.DataAnnotations;

namespace WebApplicationDemo.Entities
{
    public class User
    {
        /// <summary>
        /// Get or set the user id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or set the username
        /// </summary>
        public string UserName { get; set; } = String.Empty;

        /// <summary>
        /// The hash password
        /// </summary>
        public string HashPassword { get; set; } = String.Empty;

        [Required]
        public string Role { get; set; } = null!;

        /// <summary>
        /// The refresh token
        /// </summary>
        public string RefreshTokenVal { get; set; } = string.Empty;

        /// <summary>
        /// The refresh token expiry
        /// </summary>
        public DateTime RefreshTokenExpiry { get; set; }
    }
}