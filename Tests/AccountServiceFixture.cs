using Core;

namespace Tests
{
    public class AccountServiceFixture
    {
        public IAccountService AccountService { get; }
        public AccountServiceFixture()
        {
            AccountService = new AccountService();
            AccountService.AddTransaction(Transaction.New("AC001", "20230505", "D", 100.00));
            AccountService.AddTransaction(Transaction.New("AC001", "20230601", "D", 150.00));
            AccountService.AddTransaction(Transaction.New("AC001", "20230626", "W", 20.00));
            AccountService.AddTransaction(Transaction.New("AC001", "20230626", "W", 100.00));
        }
    }
}
