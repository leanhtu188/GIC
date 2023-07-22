namespace Core
{
    public class InterestRule
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public double Rate { get; set; }

        private InterestRule(string id, DateTime date, double rate)
        {
            Id = id;
            Date = date;
            Rate = rate;
        }

        public static InterestRule New(string id, string date, double rate)
        {
            if(rate < 0 || rate > 100) throw new ArgumentOutOfRangeException(nameof(rate));
            return new InterestRule(id, ToDate(date), rate);
        }
    }
}
