using Core;
using Spectre.Console;

namespace ConsoleApp
{
    internal class DefineInterestRatesRule : IModule
    {
        public string Name => "Define Interest Rule";
        private readonly InterestRateService _interestRateService;

        public DefineInterestRatesRule(InterestRateService interestRateService)
        {
            _interestRateService = interestRateService;
        }

        public void Handle()
        {
            string? input;
            Console.WriteLine("Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format (or enter blank to go back to main menu):");
            do
            {
                input = Console.ReadLine();
                try
                {
                    if(!string.IsNullOrWhiteSpace(input))
                    {
                        var args = input.Split('|');
                        if (args.Length != 3) 
                        {
                            Console.WriteLine("Unable to parse input. Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format \r\n(or enter blank to go back to main menu):\r\n");
                            continue;
                        }
                        var interest = InterestRule.New(args[1], args[0], double.Parse(args[2]));
                        _interestRateService.Add(interest);
                        DisplayInterestRules(_interestRateService.InterestRules);
                        break;
                    }
                }catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }while(!string.IsNullOrWhiteSpace(input));
        }

        private static void DisplayInterestRules(IEnumerable<InterestRule> interestRules)
        {
            var table = new Table();
            table.AddColumn("Date");
            table.AddColumn("Rule Id");
            table.AddColumn("Rate (%)");

            foreach (var interestRule in interestRules)
            {
                table.AddRow(interestRule.Date.ToString("yyyyMMdd"),
                    interestRule.Id, interestRule.Rate.ToString("F2"));
            }
            AnsiConsole.Write(table);
        }
    }
}
