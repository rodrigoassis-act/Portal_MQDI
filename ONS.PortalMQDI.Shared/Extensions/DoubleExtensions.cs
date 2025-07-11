using System;
using System.Globalization;

namespace ONS.PortalMQDI.Shared.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Arredonda o valor double fornecido para duas casas decimais e o converte para uma string.
        /// </summary>
        /// <param name="input">O valor double para arredondar.</param>
        /// <returns>Uma string representando o valor arredondado.</returns>
        public static string RoundToTwoDecimalPlaces(this double input)
        {
            double roundedValue = Math.Round(input, 2);
            return roundedValue.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}
