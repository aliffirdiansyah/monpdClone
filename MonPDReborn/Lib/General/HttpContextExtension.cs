using MonPDLib.General;

namespace MonPDReborn.Lib.General
{
    public static class HttpContextExtensions
    {
        public static bool IsUserLoggedIn(this HttpContext context)
        {
            if (context == null) return false;

            string sessUserId = GetUserSession(context);

            return !string.IsNullOrEmpty(sessUserId);
        }
        public static string GetUserSession(this HttpContext context)
        {
            return context.Session.GetString(Utility.SESSION_USER) ?? "";
        }
        public static int GetUserRole(this HttpContext context)
        {
            return context.Session.GetInt32(Utility.SESSION_ROLE) ?? 0;
        }
    }
}
