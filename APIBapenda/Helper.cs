using System.Globalization;

namespace APIBapenda
{
    public class Helper
    {

        public static string ToTitleCase(string str)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(str.ToLower());
        }

        public class APIResponse<T>
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public T? Data { get; set; }
        }

        public class EnumClass
        {
            public int EnumId { get; set; }
            public string EnumName { get; set; } = string.Empty;
            public string EnumDescription { get; set; } = string.Empty;
        }
    }
}
