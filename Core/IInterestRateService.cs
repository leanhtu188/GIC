namespace Core
{
    public interface IInterestRateService
    {
        IReadOnlyList<InterestRule> InterestRules { get; }

        void Add(InterestRule interestRule);
        InterestRule? GetInterestRuleBy(DateTime date);
    }
}