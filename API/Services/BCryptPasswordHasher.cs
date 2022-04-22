using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {

        public string HashPassword(TUser user, string password)
        {
            var result = BCrypt.Net.BCrypt.HashPassword(password);
            return result;
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword))
            {
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }
    }
}