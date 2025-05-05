using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EduTrack.Services
{
    public class AuthService : IAuthService
    {
        private static List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = HashPassword("admin123"),
                CreatedAt = DateTime.Now
            }
        };

        private static int _nextId = 2;

        public User Login(LoginViewModel model)
        {
            var hashedPassword = HashPassword(model.Password);
            return _users.FirstOrDefault(u => 
                u.Email.ToLower() == model.Email.ToLower() && 
                u.PasswordHash == hashedPassword);
        }

        public User Register(RegisterViewModel model)
        {
            // Check if email already exists
            if (_users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
            {
                return null;
            }

            var user = new User
            {
                Id = _nextId++,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                CreatedAt = DateTime.Now
            };

            _users.Add(user);
            return user;
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        private static string HashPassword(string password)
        {
            // This is a simple hash for demo purposes only
            // In a real application, use a proper password hashing library with salt
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
