namespace Core
{
    public record Transaction
    {
        public int Id { get; set; }
        public string AccountId { get;  }
        public DateTime Date { get; }
        public TransactionType Type { get; }
        public double Amount { get; }
        public string TransactionId => Type == TransactionType.Interest ? string.Empty : Date.ToString("yyyyMMdd") + "-" + Id;
        public double Value => Type == TransactionType.Withdrawal ? -1 * Amount : Amount;
        internal Transaction(string id, DateTime date, TransactionType type, double amount)
        {
            AccountId = id;
            Date = date;
            Type = type;
            Amount = amount;
        }

        public static Transaction New(string accountId, string date, string type, double amount)
        {
            if(amount <= 0) throw new ArgumentException("Amount should be positive", nameof(amount));
            return new Transaction(accountId, ToDate(date), ToTransactionType(type), amount);
        }
        
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Interest
    }
}
