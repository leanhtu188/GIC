namespace Core
{
    public class StatementService : IStatementService
    {
        private readonly AccountService _accountService;
        private readonly InterestRateService _interestRateService;
        private const int DEFAULT_YEAR = 2023;
        public StatementService(AccountService accountService, InterestRateService interestRateService)
        {
            _accountService = accountService;
            _interestRateService = interestRateService;
        }

        public Statement? GetStatement(string accountId, int month, int year = DEFAULT_YEAR)
        {
            var transactions = new List<Transaction>();

            var account = _accountService.Accounts.FirstOrDefault(x => x.Id == accountId);

            if (account is not null)
            {
                transactions = account.Transactions
                    .Where(x => x.Date.Month == month && x.Date.Year == year)
                    .OrderBy(x => x.Date)
                    .ToList();

                var firstDayOfMonth = new DateTime(year, month, 1);
                //Set the last day of month as the first day of the next month
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1);

                var transactionDates = _interestRateService.InterestRules
                    .Where(x => x.Date > firstDayOfMonth && x.Date < lastDayOfMonth)
                    .Select(x => x.Date)
                    .Append(firstDayOfMonth)
                    .Append(lastDayOfMonth).ToList();
                transactionDates.AddRange(transactions.Select(x => x.Date));
                transactionDates = transactionDates
                    .Distinct()
                    .Order().ToList();

                double interest = 0;

                interest = transactionDates
                    .Zip(transactionDates.Skip(1),
                    (date1, date2) =>
                    {
                        //The difference between the timespan is at the start of the day, so we compensate by adding a -1 to the date
                        date2 = date2.AddDays(-1);
                        var interestRule = _interestRateService.GetInterestRuleBy(date2)!;
                        var balance = account.GetEodBalanceOn(date2);
                        //Preserve the full duration of the interest accrued
                        var value = ((date2 - date1).TotalDays + 1)
                        * interestRule.Rate / 100
                        * balance;

                        return value;
                    })
                    .Sum();

                interest /= 365;
                Console.WriteLine($"Interest accrued: {interest}");
                //Add a statement as a separate transaction line
                var interestCredit = new Transaction(account.Id, lastDayOfMonth.AddDays(-1), TransactionType.Interest, interest);

                return new Statement(month, year, account, transactions, interestCredit);
            }
            return null;
        }
    }
}
