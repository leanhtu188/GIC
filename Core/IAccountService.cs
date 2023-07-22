namespace Core
{
    public interface IAccountService
    {
        IReadOnlyCollection<Account> Accounts { get; }

        Account AddTransaction(Transaction transaction);
    }
}