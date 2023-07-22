using Core;

namespace Tests
{
    public class InterestServiceFixture
    {
        public IInterestRateService InterestRateService { get; }
        public InterestServiceFixture()
        {
            InterestRateService = new InterestRateService();
            InterestRateService.Add(InterestRule.New("RULE01", "20230101", 1.95));
            InterestRateService.Add(InterestRule.New("RULE02", "20230520", 1.90));
            InterestRateService.Add(InterestRule.New("RULE03", "20230615", 2.20));
        }
    }
}
