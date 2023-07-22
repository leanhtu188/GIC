using System.Globalization;

namespace Core
{
    internal class Utility
    {
        public class Guard
        {
            public static void Against(bool predicate, string? parameter =null, string? message = null)
            {
                if (predicate) throw new ArgumentException(message ?? "Invalid argument", parameter ?? string.Empty);
            }
        }
        public class Culture
        {
            public static CultureInfo ENUS = new("en-US");
        }
        public static DateTime ToDate(string date)
            => DateTime.TryParseExact(date, "yyyyMMdd", Culture.ENUS, DateTimeStyles.None, out var parsed)
            ? parsed : throw new ArgumentException("Date should be in the format yyyyMMdd", nameof(date));

        public static TransactionType ToTransactionType(string type) => type switch
        {
            "D" => TransactionType.Deposit,
            "d" => TransactionType.Deposit,
            "W" => TransactionType.Withdrawal,
            "w" => TransactionType.Withdrawal,
            _ => throw new ArgumentException("Invalid transaction type", nameof(type))
        };
    }
}
