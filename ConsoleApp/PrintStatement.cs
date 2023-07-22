using Core;
using Spectre.Console;

namespace ConsoleApp
{
    internal class PrintStatement : IModule
    {
        public string Name => "Print Statement";
        private readonly StatementService _statementService;

        public PrintStatement(StatementService statementService)
        {
            _statementService = statementService;
        }

        public void Handle()
        {
            string? input;
            Console.WriteLine("Please enter account and month to generate the statement <Account>|<Month> (or enter blank to go back to main menu):");
            do
            {
                input = Console.ReadLine();
                try
                {
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        var args = input.Split('|');
                        if(args.Length != 2)
                        {
                            Console.WriteLine("Unable to parse input. Please enter account and month to generate the statement <Account>|<Month>\r\n(or enter blank to go back to main menu):\r\n");
                            continue;
                        }
                        var statement = _statementService.GetStatement(args[0], Convert.ToInt32(args[1]));

                        if (statement is null) Console.WriteLine($"No account with ID {args[0]} found");
                        else
                        {
                            DisplayStatement(statement);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            } while (!string.IsNullOrEmpty(input));
        }

        private static void DisplayStatement(Statement statement)
        {
            Console.WriteLine($"Account {statement.Account.Id}");
            var table = new Table();
            table.AddColumn("Date");
            table.AddColumn("Txn Id");
            table.AddColumn("Type");
            table.AddColumn("Amount");
            table.AddColumn("Balance");

            foreach(var statementLine in statement.Lines)
            {
                table.AddRow(
                    statementLine.Date.ToString("yyyyMMdd"),
                    statementLine.TransactionId,
                    statementLine.TransactionType.ToString()[..1],
                    statementLine.Amount.ToString("F2"),
                    statementLine.Balance.ToString("F2")
                );
            }
            table.AddRow(
                statement.InterestCredit.Date.ToString("yyyyMMdd"),
                statement.InterestCredit.TransactionId,
                statement.InterestCredit.TransactionType.ToString()[..1],
                statement.InterestCredit.Amount.ToString("F2"),
                statement.InterestCredit.Balance.ToString("F2")
            );
            AnsiConsole.Write(table);
        }
    }

}
