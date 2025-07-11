using System;
using System.Collections.Generic;
using System.Globalization;

namespace ONS.PortalMQDI.Shared.Extensions
{
    public static class StringExtensions
    {
        public static List<DateTime> GeneratePastMonths(this string startDateString, int numberOfMonths)
        {
            if (DateTime.TryParseExact(startDateString, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                List<DateTime> dates = new List<DateTime>();

                for (int i = 0; i < numberOfMonths; i++)
                {
                    dates.Add(startDate.AddMonths(-i));
                }

                return dates;
            }

            return new List<DateTime>();
        }

        public static DateTime ConvertStringToDate(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("A string de entrada não deve ser nula ou vazia.");
            }

            if (DateTime.TryParseExact(input + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Formato de data inválido.");
            }
        }


        public static string ConvertToAnomeReferencia(this string input)
        {
            return input.Replace("/", "-").Replace(@"\", "-");
        }

        public static string ConvertAnomeReferenciaToDate(this string input)
        {
            return input.Replace("-", "/");
        }

        public static string ConvertStringToDateString(this string input)
        {
            DateTime dt = DateTime.Parse(input);
            return dt.ToString("yyyy-MM-dd");
        }
    }
}
