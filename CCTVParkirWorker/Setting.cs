using MonPDLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCTVParkirWorker
{
    public class Setting
    {
        public static string GetUrlApiJasnita()
        {
            var result = "";

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "URL_API_JASNITA").SingleOrDefault();
            if (query != null)
            {
                result = query.Value;
            }

            return result;
        }
        public static string GetUrlApiTelkom()
        {
            var result = "";

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "URL_API_TELKOM").SingleOrDefault();
            if (query != null)
            {
                result = query.Value;
            }

            return result;
        }
        public static string GetUrlApiTokenTelkom()
        {
            var result = "";

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "URL_TOKEN_TELKOM").SingleOrDefault();
            if (query != null)
            {
                result = query.Value;
            }

            return result;
        }
        public static string GetUserJasnita()
        {
            var result = "";

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "USER_JASNITA").SingleOrDefault();
            if (query != null)
            {
                result = query.Value;
            }

            return result;
        }
        public static string GetPassJasnita()
        {
            var result = "";

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "PASS_JASNITA").SingleOrDefault();
            if (query != null)
            {
                result = query.Value;
            }

            return result;
        }
        public static string GetUserTelkom()
        {
            var result = "";

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "USER_TELKOM").SingleOrDefault();
            if (query != null)
            {
                result = query.Value;
            }

            return result;
        }
        public static string GetPassTelkom()
        {
            var result = "";

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "PASS_TELKOM").SingleOrDefault();
            if (query != null)
            {
                result = query.Value;
            }

            return result;
        }
        public static int GetIntervalDay()
        {
            var result = 0;

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "INTERVAL_DAY").SingleOrDefault();
            if (query != null)
            {
                result = Convert.ToInt32(query.Value);
            }

            return result;
        }
        public static int GetIntervalDelay()
        {
            var result = 0;

            var context = DBClass.GetContext();
            var query = context.Settings.Where(x => x.Properti == "INTERVAL_DELAY").SingleOrDefault();
            if (query != null)
            {
                result = Convert.ToInt32(query.Value);
            }

            return result;
        }
    }
}
