namespace LMS.Infrastructure.Services
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class PasswordHashService : IPasswordHashService
    {
        public string HashPassword(string password)
        {
            return global::BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return global::BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (global::BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}
