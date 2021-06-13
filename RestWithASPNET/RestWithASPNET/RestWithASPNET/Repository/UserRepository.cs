using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLContext _context;

        public UserRepository(MySQLContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(UserVO userVO)
        {
            var pass = ComputeHash(userVO.Password, new SHA256CryptoServiceProvider());
            return _context.Users.FirstOrDefault(u => (u.UserName == userVO.UserName) && (u.Password == pass));
        }

        public User ValidateCredentials(string userName)
        {
            return _context.Users.FirstOrDefault(u => (u.UserName == userName));
        }

        public bool RevokeToken(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => (u.UserName == userName));
            if (user is null) return false;
            user.RefreshToken = null;
            _context.SaveChanges();
            return true;
        }

        private string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }

        public User RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(u => u.Id.Equals(user.Id))) return null;
            var result = _context.Users.SingleOrDefault(u => u.Id.Equals(user.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return result;
        }

        
    }
}
