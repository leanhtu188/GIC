namespace Core
{
    public class Account
    {
        public string Id { get; }
        public double Balance => _transactions.Sum(x => x.Value);

        private List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private Account(string id)
        {
            Id = id;
        }
        public double GetBalanceUntilTransaction(Transaction transaction)
        {
            return _transactions
                .OrderBy(x => x.Date)
                .ThenBy(x => x.Id)
                .TakeWhile(x => x != transaction)
                .Sum(x => x.Value) + transaction.Value;
        }
        public double GetEodBalanceOn(DateTime date) {
            return _transactions.Where(x => x.Date <= date).Sum(x => x.Value);
        }
        public void Add(Transaction transaction) {
            transaction.Id = _transactions.Count(x => x.Date == transaction.Date) + 1;
            if(transaction.Type == TransactionType.Deposit) _transactions.Add(transaction);
            else if(transaction.Type == TransactionType.Withdrawal)
            {
                //Check enough balance
                if(Balance >= transaction.Amount) _transactions.Add(transaction);
                else throw new InvalidOperationException("Balance too low to withdraw");
            }
            _transactions = _transactions.OrderBy(x => x.Date).ToList();
        }
        public static Account New(Transaction transaction)
        {
            if (transaction.Type == TransactionType.Withdrawal && transaction.Amount > 0) throw new ArgumentException("Cannot create an account with negative balance");
            var newAccount = new Account(transaction.AccountId);
            transaction.Id = 1;
            newAccount.Add(transaction);
            return newAccount;
        }
    }
}
