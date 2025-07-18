using MonPDLib;
using MonPDLib.Lib;

namespace MonPDReborn.Models
{
    public class LoginVM
    {
        public class Index
        {
            public CViewModel.LoginRow Row { get; set; } = new CViewModel.LoginRow();
            public Index()
            {

            }
        }

        public static UserLogin DoLogin(Index input)
        {
            return UserLogin.DoLogin(input.Row.Username, input.Row.Password);
        }

        public class CViewModel
        {
            public class LoginRow
            {
                public string Username { get; set; } = "";
                public string Password { get; set; } = "";
            }
        }
    }
}
