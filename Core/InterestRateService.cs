namespace Core
{
    public class InterestRateService : IInterestRateService
    {
        private readonly List<InterestRule> _interestRules = new();

        public IReadOnlyList<InterestRule> InterestRules => _interestRules.AsReadOnly();

        public void Add(InterestRule interestRule)
        {
            var existingIndex = _interestRules.FindIndex(x => x.Date == interestRule.Date);
            if (existingIndex != -1)
            {
                _interestRules[existingIndex] = interestRule;
            }
            else
            {
                if (_interestRules.Any(x => x.Id == interestRule.Id)) throw new ArgumentException($"{nameof(InterestRule)} must have an unique ID");
                _interestRules.Add(interestRule);
            }
        }

        public InterestRule? GetInterestRuleBy(DateTime date)
        {
            return _interestRules.OrderBy(x => x.Date).LastOrDefault(x => x.Date < date);
        }
    }
}
