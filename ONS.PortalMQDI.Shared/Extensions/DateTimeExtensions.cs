using System;

public static class DateTimeExtensions
{
    /// <summary>
    /// Verifica se a data está no passado.
    /// </summary>
    /// <param name="dateTime">Data para verificar.</param>
    /// <returns>Verdadeiro se a data estiver no passado; caso contrário, falso.</returns>
    public static bool IsPast(this DateTime dateTime)
    {
        return dateTime < DateTime.Now;
    }

    /// <summary>
    /// Verifica se a data está no futuro.
    /// </summary>
    /// <param name="dateTime">Data para verificar.</param>
    /// <returns>Verdadeiro se a data estiver no futuro; caso contrário, falso.</returns>
    public static bool IsFuture(this DateTime dateTime)
    {
        return dateTime > DateTime.Now;
    }

    /// <summary>
    /// Verifica se a data é hoje.
    /// </summary>
    /// <param name="dateTime">Data para verificar.</param>
    /// <returns>Verdadeiro se a data for hoje; caso contrário, falso.</returns>
    public static bool IsToday(this DateTime dateTime)
    {
        return dateTime.Date == DateTime.Now.Date;
    }

    /// <summary>
    /// Obtém o primeiro dia do mês da data especificada.
    /// </summary>
    /// <param name="dateTime">Data original.</param>
    /// <returns>Primeiro dia do mês.</returns>
    public static DateTime FirstDayOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    /// <summary>
    /// Obtém o último dia do mês da data especificada.
    /// </summary>
    /// <param name="dateTime">Data original.</param>
    /// <returns>Último dia do mês.</returns>
    public static DateTime LastDayOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    /// <summary>
    /// Obtém o primeiro dia da semana da data especificada.
    /// </summary>
    /// <param name="dateTime">Data original.</param>
    /// <param name="startOfWeek">Primeiro dia da semana.</param>
    /// <returns>Primeiro dia da semana.</returns>
    public static DateTime FirstDayOfWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Sunday)
    {
        int diff = (7 + (dateTime.DayOfWeek - startOfWeek)) % 7;
        return dateTime.AddDays(-1 * diff).Date;
    }

    /// <summary>
    /// Obtém o último dia da semana da data especificada.
    /// </summary>
    /// <param name="dateTime">Data original.</param>
    /// <param name="endOfWeek">Último dia da semana.</param>
    /// <returns>Último dia da semana.</returns>
    public static DateTime LastDayOfWeek(this DateTime dateTime, DayOfWeek endOfWeek = DayOfWeek.Saturday)
    {
        int diff = (7 - (endOfWeek - dateTime.DayOfWeek)) % 7;
        return dateTime.AddDays(diff).Date;
    }

    /// <summary>
    /// Converte a data para um formato de string personalizado.
    /// </summary>
    /// <param name="dateTime">Data original.</param>
    /// <param name="format">Formato de string desejado.</param>
    /// <returns>Data em formato de string personalizado.</returns>
    public static string ToCustomFormat(this DateTime dateTime, string format = "dd/MM/yyyy")
    {
        return dateTime.ToString(format);
    }

    /// <summary>
    /// Converte a data para um fuso horário específico.
    /// </summary>
    /// <param name="dateTime">Data original.</param>
    /// <param name="targetTimeZone">Fuso horário alvo.</param>
    /// <returns>Data convertida para o fuso horário alvo.</returns>
    public static DateTime ConvertToTimeZone(this DateTime dateTime, TimeZoneInfo targetTimeZone)
    {
        return TimeZoneInfo.ConvertTime(dateTime, targetTimeZone);
    }
}
