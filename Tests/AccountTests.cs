using Core;
using System.Collections;

namespace Tests
{
    public class AccountTests : IClassFixture<AccountServiceFixture>
    {
        readonly AccountServiceFixture AccountServiceFixture;
        IAccountService AccountService => AccountServiceFixture.AccountService;
        public AccountTests(AccountServiceFixture accountServiceFixture)
        {
            AccountServiceFixture = accountServiceFixture;
        }

        [Fact]
        public void Can_add_new_transactions()
        {
            AccountService.AddTransaction(
                Transaction.New("AC002", "20230507", "D", 200));
            Assert.True(AccountService.Accounts.Count == 2);
        }

        [Theory, ClassData(typeof(TestInvalidTransactions))]
        public void Cannot_have_negative_balance(Transaction transaction)
        {
            Assert.ThrowsAny<Exception>(() => {
                AccountService.AddTransaction(transaction);
            });
        }

        [Fact]
        public void Can_add_proper_ID()
        {
            var transactionOnDate = Transaction.New("AC001", "20230626", "D", 200);
            AccountService.AddTransaction(transactionOnDate);
            Assert.True(transactionOnDate.TransactionId == "20230626-3");
        }
    }

    public class TestInvalidTransactions : IEnumerable<object[]>
    {
        private readonly List<object[]> _transactions = new()
        {
            new object[] {Transaction.New("AC002", "20230101","W", double.MaxValue) },
            new object[] {Transaction.New("AC003", "20230101","W", 100) }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _transactions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
