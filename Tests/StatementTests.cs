using Core;

namespace Tests
{
    public class StatementTests : IClassFixture<StatementServiceFixture>
    {
        readonly StatementServiceFixture Fixture;

        public StatementTests(StatementServiceFixture fixture)
        {
            Fixture = fixture;
        }

        IStatementService StatementService => Fixture.StatementService;

        [Theory]
        [InlineData("AC001",6)]
        public void Can_get_statement(string accountId, int month)
        {
            var statement = StatementService.GetStatement(accountId, month);

            Assert.NotNull(statement);
            Assert.True(statement!.Account.Id == accountId);
            Assert.Equal(0.39, statement.InterestCredit.Amount, 0.01);
        }
    }
}
