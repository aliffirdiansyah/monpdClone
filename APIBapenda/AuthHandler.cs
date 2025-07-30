using MonPDLib;

namespace APIBapenda
{
    public class AuthHandler
    {
        public static bool ValidateUser(string username, string password)
        {
            var context = DBClass.GetContext();
            return context.UserApiBapenda
                .Where(x => 
                x.Username.Trim().ToLower() == username.Trim().ToLower() 
                &&  x.Pass == password).Any();
        }
    }
}
