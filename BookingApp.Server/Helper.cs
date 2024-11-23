using System.Security.Cryptography;
using System.Text;

namespace BookingApp.Server
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Converts a DateTime to UTC format. If the input is null, returns null.
        /// </summary>
        /// <param name="dateTime">The DateTime to be converted.</param>
        /// <returns>Nullable DateTime in UTC format.</returns>
        public static DateTime? ConvertToUtc(DateTime? dateTime)
        {
            if (dateTime == null)
                return null;

            // Ensure the DateTime is in UTC format
            return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
        }
    }
    public static class  HasPassword
    {
        public static string Sha1(string input)
        {
            using (SHA1 shA1 = SHA1.Create())
                return BitConverter.ToString(shA1.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
        }
    }

    public static class DapperHelper
    {
        public static string DebugSql(string query, object parameters)
        {
            foreach (var prop in parameters.GetType().GetProperties())
            {
                var value = prop.GetValue(parameters);
                var formattedValue = value is string || value is DateTime
                    ? $"'{value}'"
                    : value?.ToString() ?? "NULL";
                query = query.Replace($"@{prop.Name}", formattedValue);
            }
            return query;
        }
    }
   
}
