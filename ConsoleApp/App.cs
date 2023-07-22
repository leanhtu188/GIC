using Core;
using Spectre.Console;

namespace ConsoleApp
{
    internal class App
    {
        public bool SessionEnded { get; private set; }

        private readonly List<string> _history = new();
        private readonly List<IModule> _modules;

        private readonly AccountService _accountService;
        private readonly InterestRateService _interestRateService;
        private readonly StatementService _statementService;

        public App()
        {
            //Normally this bit would be dependency injected
            _accountService = new();
            _accountService.AddTransaction(Transaction.New("AC001", "20230505", "D", 100.00));
            _accountService.AddTransaction(Transaction.New("AC001", "20230601", "D", 150.00));
            _accountService.AddTransaction(Transaction.New("AC001", "20230626", "W", 20.00));
            _accountService.AddTransaction(Transaction.New("AC001", "20230626", "W", 100.00));

            _interestRateService = new();
            _interestRateService.Add(InterestRule.New("RULE01", "20230101", 1.95));
            _interestRateService.Add(InterestRule.New("RULE02", "20230520", 1.90));
            _interestRateService.Add(InterestRule.New("RULE03", "20230615", 2.20));

            _statementService = new StatementService(_accountService, _interestRateService);
            //Module actions
            _modules = new()
            {
                new InputTransaction(_accountService),
                new DefineInterestRatesRule(_interestRateService),
                new PrintStatement(_statementService)
            };
        }
        public void Run()
        {
            while (!SessionEnded)
            {
                string prompt;
                if (!_history.Any())
                    prompt = "Welcome to AwesomeGIC! What would you like to do?";
                else prompt = "Is there anything else you'd like to do?";

                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title(prompt)
                    .AddChoices(_modules.Select(x => x.Name).Append("Quit").ToArray()));
                Console.Clear();
                _history.Add(selection);

                if (selection.ToLower() == "quit") Quit();
                else _modules.FirstOrDefault(x => x.Name == selection)!.Handle();
            }
        }

        private void Quit()
        {
            Console.WriteLine("Thank you for banking with AwesomeGIC Bank!");
            Console.WriteLine("Have a nice day!");
            SessionEnded = true;
        }
    }
}
