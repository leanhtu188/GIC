namespace Core
{
    public record Statement
    {
        public int Month { get; }
        public int Year { get; }
        public StatementLine InterestCredit { get; }
        public Account Account { get;}
        public IReadOnlyList<StatementLine> Lines { get; }

        public Statement(
            int month,
            int year,
            Account account,
            IEnumerable<Transaction> transactions,
            Transaction interestCredit)
        {
            Month = month;
            Year = year;
            Account = account;
            Lines = transactions
                .Select(x => new StatementLine(
                    x.Date,
                    x.TransactionId,
                    x.Type,
                    x.Amount,
                    account.GetBalanceUntilTransaction(x)))
                .ToList();
            InterestCredit = new StatementLine(interestCredit.Date,
                string.Empty,
                TransactionType.Interest,
                interestCredit.Amount,
                account.GetEodBalanceOn(interestCredit.Date) + interestCredit.Amount);
        }
    }

    public record StatementLine
    {
        public DateTime Date { get; set; }
        public string TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public StatementLine(DateTime date, string transactionId,
            TransactionType transactionType, double amount, double balance)
        {
            Date = date;
            TransactionId = transactionId;
            TransactionType = transactionType;
            Amount = amount;
            Balance = balance;
        }
    }
}
