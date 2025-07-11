using System;
using System.ComponentModel;

namespace ONS.PortalMQDI.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                var attrs = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs?.Length > 0)
                {
                    return attrs[0].Description;
                }
            }

            return value.ToString();
        }

    }
}
