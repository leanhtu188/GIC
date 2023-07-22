using Core;

namespace Tests
{
    public class InterestTests : IClassFixture<InterestServiceFixture>
    {
        readonly InterestServiceFixture fixture;
        IInterestRateService InterestRateService => fixture.InterestRateService;
        public InterestTests(InterestServiceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Can_override_interest_date()
        {
            var overlapInterestRate = InterestRule.New("RULE04", "20230615", 5.00);
            InterestRateService.Add(overlapInterestRate);
            Assert.Contains(overlapInterestRate, InterestRateService.InterestRules);
        }
        [Fact]
        public void Can_get_interest_by_date()
        {
            var date = new DateTime(2023, 06, 30);
            Assert.True(InterestRateService.InterestRules[2] == InterestRateService.GetInterestRuleBy(date));
        }
    }
}
