using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPDLib.Lib
{
    public class UserLogin
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public decimal? RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime? InsertDate { get; set; }
        public string InsertBy { get; set; } = null!;
        public static UserLogin DoLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Username is required.");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password is required.");
            }

            var generatedPassword = GeneratedPassword(username, password);

            var context = DBClass.GetContext();
            var userLogin = context.MUserLogins
                .Where(u => u.Username == username && u.Password == generatedPassword)
                .Select(u => new UserLogin
                {
                    Username = u.Username,
                    Password = u.Password,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    RoleName = u.RoleName,
                    InsertDate = u.InsertDate,
                    InsertBy = u.InsertBy
                })
                .FirstOrDefault();

            if (userLogin == null)
            {
                throw new Exception("Invalid username or password.");
            }

            return userLogin;
        }


        public static string GeneratedPassword(string user, string pass)
        {
            System.Security.Cryptography.TripleDESCryptoServiceProvider des = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            System.Security.Cryptography.MD5CryptoServiceProvider MD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            des.Key = MD5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(user.ToUpper().Trim()));
            des.Mode = System.Security.Cryptography.CipherMode.ECB;
            byte[] Buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(pass);
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
    }
}
