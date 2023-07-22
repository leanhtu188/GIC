namespace Core
{
    public class AccountService : IAccountService
    {
        private readonly List<Account> _accounts = new();
        public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

        public Account AddTransaction(Transaction transaction)
        {
            var account = _accounts.FirstOrDefault(x => x.Id == transaction.AccountId);
            if (account is not null)
            {
                _accounts.First(x => x.Id == transaction.AccountId).Add(transaction);
            }
            else
            {
                account = Account.New(transaction);
                _accounts.Add(account);
            }
            return account;
        }

    }
}
