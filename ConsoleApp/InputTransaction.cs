using Core;
using Spectre.Console;

namespace ConsoleApp
{
    internal class InputTransaction : IModule
    {
        private readonly AccountService _repository;

        public InputTransaction(AccountService repository)
        {
            _repository = repository;
        }

        public string Name => "Input Transactions";
        private readonly string _prompt = "Please enter transaction details in <Date>|<Account>|<Type>|<Amount> format (or enter blank to go back to main menu):";
        public void Handle()
        {
            string? input;
            Console.WriteLine(_prompt);
            do
            {
                input = Console.ReadLine();
                try
                {
                    if(!string.IsNullOrWhiteSpace(input))
                    {
                        var args = input.Split('|');
                        if(args.Length != 4)
                        {
                            Console.WriteLine("Unable to parse input. Please enter details in the format <Date>|<Account>|<Type>|<Amount>");
                            continue;
                        }
                        var transaction = Transaction.New(args[1], args[0], args[2], double.Parse(args[3]));
                        var accountStatement = _repository.AddTransaction(transaction);
                        DisplayStatement(accountStatement);
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Amount needs to be in decimal");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

            } while(!string.IsNullOrEmpty(input));

        }

        private static void DisplayStatement(Account account)
        {
            Console.WriteLine($"Account {account.Id}");
            var table = new Table();
            table.AddColumn("Date");
            table.AddColumn("Txn Id");
            table.AddColumn("Type");
            table.AddColumn("Amount");

            foreach(var transaction in account.Transactions)
            {
                table.AddRow(transaction.Date.ToString("yyyyMMdd"),
                    transaction.TransactionId, transaction.Type.ToString()[..1], transaction.Amount.ToString("F2"));
            }
            AnsiConsole.Write(table);
        }
    }
}
