using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public static class Bank
    {
        public static string Name { get; set; }
        public static List<Account> Accounts { get; set; }
        private static int accountNo = 1;

        public static void Create2(this string[] input)
        {
            if (input.Length < 4)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Create].InputFormat}\", ensuring there are spaces between each element");
                return;
            }
            if (input.Length > 4)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Create].InputFormat}\" and hyphenate any multi part names");
                return;
            }
            var amountText = input[3];
            var parsed = decimal.TryParse(amountText, out var amount);
            if (!parsed)
            {
                Console.WriteLine($"Failed to parse {ActionText[Actions.Create].AmountText}, please ensure that the ammount is only numeric and does not include a currency sign");
                return;
            }
            Accounts.Add(new Account($"{input[1]} {input[2]}", amount, ref accountNo));
            Console.WriteLine($"Account created successfully! Your account number is {Accounts.Last().AccountNumber}");
        }

        public static void Create(string accountName, decimal initialBalance)
        {
            Accounts.Add(new Account(accountName, initialBalance, ref accountNo));
            Console.WriteLine($"Account created successfully! Your account number is {Accounts.Last().AccountNumber}");
        }

        public static void Withdraw(string accountNumber, decimal amount)
        {
            var account = Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found, please check your account number or create an account to get started");
                return;
            }
            if (account!.Balance < amount)
            {
                Console.WriteLine("lmao you broke boi");
                return;
            }
            else
            {
                account.Balance -= amount;
                Console.WriteLine($"Your remaining balance is {account.Balance}");
            }
        }

        public static void Withdraw2(this string[] input)
        {
            if (input.Length < 3)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Withdraw].InputFormat}\", ensuring there are spaces between each element");
                return;
            }
            if (input.Length > 3)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Withdraw].InputFormat}\"");
                return;
            }
            var amountText = input[2];
            var parsed = decimal.TryParse(amountText, out var amount);
            var account = Accounts.First(a => a.AccountNumber == input[1]);
            if (account == null)
            {
                Console.WriteLine("Account not found, please check your account number or create an account to get started");
                return;
            }
            if (!parsed)
            {
                Console.WriteLine($"Failed to parse {ActionText[Actions.Withdraw].AmountText}, please ensure that the ammount is only numeric and does not include a currency sign");
                return;
            }
            if (account!.Balance < amount)
            {
                Console.WriteLine("lmao you broke boi");
                return;
            }
            else
            {
                account.Balance -= amount;
                Console.WriteLine($"Your remaining balance is {account.Balance}");
            }
        }

        public static void Deposit(string accountNumber, decimal amount)
        {
            var account = Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found, please check your account number or create an account to get started");
                return;
            }
            account.Balance += amount;
            Console.WriteLine($"Your new balance is {account.Balance}");
        }

        public static void Deposit2(this string[] input)
        {
            if (input.Length < 3)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Deposit].InputFormat}\", ensuring there are spaces between each element");
                return;
            }
            if (input.Length > 3)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Deposit].InputFormat}\"");
                return;
            }
            var amountText = input[2];
            var parsed = decimal.TryParse(amountText, out var amount);
            var account = Accounts.FirstOrDefault(a => a.AccountNumber == input[1]);
            if (account == null)
            {
                Console.WriteLine("Account not found, please check your account number or create an account to get started");
                return;
            }
            if (!parsed)
            {
                Console.WriteLine($"Failed to parse {ActionText[Actions.Deposit].AmountText}, please ensure that the ammount is only numeric and does not include a currency sign");
                return;
            }
            account.Balance += amount;
            Console.WriteLine($"Your new balance is {account.Balance}");
        }

        public static void Help(bool numMode)
        {
            Console.WriteLine($"You are currently in {(numMode ? "Num pad mode" : "Keyboard mode")}");
            Console.WriteLine("The following are a list of commands for this interface (not case sensitive)");
            Console.WriteLine("\"1234\": Switches to an interface made for keyboard users");
            Console.WriteLine("In keyboard mode, these are the available comands:");
            Console.WriteLine($"\"Create Dave Dungus 100\": Creates a new account, use the form \"{ActionText[Actions.Create].InputFormat}\"");
            Console.WriteLine($"\"Withdraw 12345678 100\": Withdraws money from an existing account, use the form \"{ActionText[Actions.Withdraw].InputFormat}\"");
            Console.WriteLine($"\"Deposit 12345678 100\": Deposits money into an existing account, use the form \"{ActionText[Actions.Deposit].InputFormat}\"");
            Console.WriteLine();
            Console.WriteLine("\"4321\": Switches to an interface made for numpad users");
            Console.WriteLine();
            Console.WriteLine("\"Help\": Displays the available commands for the interface");
            Console.WriteLine("\"Quit\": Quits the interface");
        }

        public static void Quit(ref bool quit)
        {
            Console.WriteLine("Later skater");
            quit = true;
        }

        public enum Actions
        {
            Create,
            Withdraw,
            Deposit
        }

        public static Dictionary<Actions, ActionTextVariant> ActionText = new Dictionary<Actions, ActionTextVariant>()
        {
            { Actions.Create, new ActionTextVariant("%Create% %account holder% %starting balance%", "starting balance", 4) },
            { Actions.Withdraw, new ActionTextVariant("%Withdraw% %account number% %amount to withdraw%", "withdraw amount", 3) },
            { Actions.Deposit, new ActionTextVariant("%Deposit% %account number% %amount to deposit%", "deposit amount", 3) }
        };

        public class ActionTextVariant
        {
            public string InputFormat;
            public string AmountText;
            public int RequestLength;

            public ActionTextVariant(string inputFormat, string amountText, int requestLength)
            {
                InputFormat = inputFormat;
                AmountText = amountText;
                RequestLength = requestLength;
            }
        }
    }
}
