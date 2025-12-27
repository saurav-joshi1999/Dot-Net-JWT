namespace WebApplicationDemo.Model
{
    public class UserDto
    {
        /// <summary>
        /// Get or set the username
        /// </summary>
        public string UserName { get; set; } = String.Empty;

        /// <summary>
        /// Get or set the hash password
        /// </summary>
        public string Password { get; set; } = String.Empty;

        /// <summary>
        /// Get or set the role
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
