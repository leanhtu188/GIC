using Core;

namespace Tests
{
    public class StatementServiceFixture
    {
        public IStatementService StatementService { get;}
        public StatementServiceFixture()
        {
            var AccountService = new AccountService();
            AccountService.AddTransaction(Transaction.New("AC001", "20230505", "D", 100.00));
            AccountService.AddTransaction(Transaction.New("AC001", "20230601", "D", 150.00));
            AccountService.AddTransaction(Transaction.New("AC001", "20230626", "W", 20.00));
            AccountService.AddTransaction(Transaction.New("AC001", "20230626", "W", 100.00));

            var InterestRateService = new InterestRateService();
            InterestRateService.Add(InterestRule.New("RULE01", "20230101", 1.95));
            InterestRateService.Add(InterestRule.New("RULE02", "20230520", 1.90));
            InterestRateService.Add(InterestRule.New("RULE03", "20230615", 2.20));

            StatementService = new StatementService(AccountService, InterestRateService);   
        }
    }
}
