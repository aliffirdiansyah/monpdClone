using System.ComponentModel;
using System.Reflection;

namespace MonPDReborn.Lib.General
{
    public static class Extension
    {
        public class EnumClass
        {
            public int EnumId { get; set; }
            public string EnumName { get; set; } = string.Empty;
            public string EnumDescription { get; set; } = string.Empty;
        }

        public class SelectListItemCustom
        {
            public int Value { get; set; }
            public string Text { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public bool Selected { get; set; } = false;
        }

        public class SelectListItemCustom2
        {
            public string Value { get; set; } = "";
            public string Text { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public bool Selected { get; set; } = false;
        }

        /// <summary>
        /// Get the description of an enum value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// Convert an enum to a list of SelectListItemCustom.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<SelectListItemCustom> ToEnumList<TEnum>()
        where TEnum : struct, Enum
        {
            var type = typeof(TEnum);
            var items = new List<SelectListItemCustom>();

            foreach (var value in Enum.GetValues(type))
            {
                var field = type.GetField(value.ToString() ?? "");
                var description = field?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();

                items.Add(new SelectListItemCustom
                {
                    Value = Convert.ToInt32(value),
                    Text = value.ToString() ?? "",
                    Description = description ?? "",
                });
            }

            return items;
        }

        public static EnumClass ConvertEnumToEnumClass<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            EnumClass enumClass = new EnumClass();

            // Mengambil EnumId (nilai integer dari enum)
            enumClass.EnumId = Convert.ToInt32(enumValue);

            // Mengambil EnumName (nama string dari enum)
            enumClass.EnumName = enumValue.ToString();

            // Mengambil EnumDescription dari atribut kustom (jika ada)
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field != null)
            {
                enumClass.EnumDescription = field?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field?.ToString() ?? "";
            }

            return enumClass;
        }
    }
}
