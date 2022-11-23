using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static BankApp.Auth;
using static BankApp.ValidationHelper;

namespace BankApp
{
    public static class Bank
    {
        public static string Name { get; set; }
        public static List<User> Users { get; set; }
        public static int accountNo = 1;
        public static int userNo = 1;
        private const string path = "../../../test.json";

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

        public static void Create(string accountName, decimal initialBalance)
        {
            Console.WriteLine("Please create a 4 to 8 digit pin");
            var pinValidation = ValidatePin(Console.ReadLine());
            if (!pinValidation.Valid)
            {
                Console.WriteLine($"Failed to parse pin, please ensure that the pin is only numeric is between 4 and 8 digits.");
                return;
            }
            if (activeUser == null)
            {
                Users.Add(new User(accountName, initialBalance));
                activeUser = Users.Last();
            }
            else
            {
                activeUser.UserAccounts.Add(new Account(accountName, initialBalance));
            }
            Console.WriteLine($"Account created successfully! Your account number is {activeUser.UserAccounts.Last().AccountNumber}");
        }

        public static void Withdraw(Account account, decimal amount)
        {

            if (account.Balance < amount)
            {
                Console.WriteLine("lmao you broke boi");
                return;
            }

            account.Balance -= amount;
            Console.WriteLine($"Your remaining balance is {account.Balance}");
        }

        public static void Deposit(Account account, decimal amount)
        {
            account.Balance += amount;
            Console.WriteLine($"Your new balance is {account.Balance}");
        }

        //public static void Deposit2(this string[] input)
        //{
        //    if (input.Length < 3)
        //    {
        //        Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Deposit].InputFormat}\", ensuring there are spaces between each element");
        //        return;
        //    }
        //    if (input.Length > 3)
        //    {
        //        Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[Actions.Deposit].InputFormat}\"");
        //        return;
        //    }
        //    var amountText = input[2];
        //    var parsed = decimal.TryParse(amountText, out var amount);
        //    var account = Accounts.FirstOrDefault(a => a.AccountNumber == input[1]);
        //    if (account == null)
        //    {
        //        Console.WriteLine("Account not found, please check your account number or create an account to get started");
        //        return;
        //    }
        //    if (!parsed)
        //    {
        //        Console.WriteLine($"Failed to parse {ActionText[Actions.Deposit].AmountText}, please ensure that the ammount is only numeric and does not include a currency sign");
        //        return;
        //    }
        //    account.Balance += amount;
        //    Console.WriteLine($"Your new balance is {account.Balance}");
        //}

        public static void Help(bool numMode, bool admin)
        {
            switch (numMode, admin)
            {
                case (true, false):
                    Console.WriteLine("\"1\" for account withdrawals");
                    Console.WriteLine("\"2\" for account deposits");
                    Console.WriteLine("\"5\" for account summary");
                    Console.WriteLine("\"3\" for help");
                    Console.WriteLine("\"4\" to quit");
                    break;
                case (true, true):
                    Console.WriteLine($"You are currently in num pad admin mode");
                    Console.WriteLine("\"1\" for account withdrawals");
                    Console.WriteLine("\"2\" for account deposits");
                    Console.WriteLine("\"3\" for help");
                    Console.WriteLine("\"4\" to quit");
                    Console.WriteLine("\"5\" to delete accounts (tbc)");
                    Console.WriteLine("\"6\" to suspend accounts (tbc)");
                    Console.WriteLine("\"7\" to un-suspend accounts (tbc)");
                    Console.WriteLine("\"10\" to list accounts (tbc)");
                    Console.WriteLine("\"11\": Set up the interface in a different mode");
                    break;
                case (false, false):
                    Console.WriteLine("The following are a list of commands for this interface (not case sensitive)");
                    Console.WriteLine($"\"Create Dave Dungus 100\": Creates a new account, use the form \"{ActionText[Actions.Create].InputFormat}\"");
                    Console.WriteLine($"\"Withdraw 12345678 100\": Withdraws money from an existing account, use the form \"{ActionText[Actions.Withdraw].InputFormat}\"");
                    Console.WriteLine($"\"Deposit 12345678 100\": Deposits money into an existing account, use the form \"{ActionText[Actions.Deposit].InputFormat}\"");
                    Console.WriteLine();
                    Console.WriteLine("\"4321\": Switches to an interface made for numpad users");
                    Console.WriteLine();
                    Console.WriteLine("\"Help\": Displays the available commands for the interface");
                    Console.WriteLine("\"Quit\": Quits the interface");
                    break;
                case (false, true):
                    Console.WriteLine($"You are currently in Keyboard admin mode");
                    Console.WriteLine("The following are a list of commands for this interface (not case sensitive)");
                    Console.WriteLine($"\"Create Dave Dungus 100\": Creates a new account, use the form \"{ActionText[Actions.Create].InputFormat}\"");
                    Console.WriteLine($"\"Withdraw 12345678 100\": Withdraws money from an existing account, use the form \"{ActionText[Actions.Withdraw].InputFormat}\"");
                    Console.WriteLine($"\"Deposit 12345678 100\": Deposits money into an existing account, use the form \"{ActionText[Actions.Deposit].InputFormat}\"");
                    Console.WriteLine("\"Manage\" to manage accounts (tbc)");
                    Console.WriteLine("\"Delete\" to delete accounts (tbc)");
                    Console.WriteLine("\"Suspend\" to suspend accounts (tbc)");
                    Console.WriteLine("\"Unsuspend\" to un-suspend accounts (tbc)");
                    Console.WriteLine("\"List\" to list accounts for this user");
                    Console.WriteLine();
                    Console.WriteLine("\"4321\": Switches to an interface made for numpad users");
                    Console.WriteLine();
                    Console.WriteLine("\"Help\": Displays the available commands for the interface");
                    Console.WriteLine("\"Quit\": Quits the interface");
                    Console.WriteLine("\"SetUp\": Set up the interface in a different mode");
                    break;
            }
        }

        public static void Quit(ref bool quit)
        {
            Console.WriteLine("Later skater");
            quit = true;

            var users = JsonConvert.SerializeObject(Users);
            File.WriteAllText(path, users);
        }

        public static void InitialiseUsers()
        {
            Console.WriteLine("Setting up existing users");

            var userJson = File.ReadAllText(path);
            Users = JsonConvert.DeserializeObject<List<User>>(userJson);
        }

        public static void Setup(ref bool quit, ref bool numMode)
        {
            Console.WriteLine("Welcome to ROLLER Banking, please choose the mode you wish to use today:");
            Console.WriteLine("\"1\" for ATM mode");
            Console.WriteLine("\"2\" for ATM admin mode");
            Console.WriteLine("\"qwer\" for keyboard mode");
            Console.WriteLine("\"asdf\" for keyboard Admin mode");
            Console.WriteLine("\"3\" to quit");
            var repeat = false;

            do
            {
                var read = Console.ReadLine().ToLower();
                switch (read)
                {
                    case "1":
                        Console.WriteLine("You have selected ATM mode, please note this is a customer facing mode and there will be no prompts for leaving this mode once engaged. Once engaged, enter \"98769876\" to exit this mode.");
                        Console.WriteLine("Enter \"9\" to confirm ATM mode.");
                        repeat = true;
                        break;
                    case "9":
                        Console.WriteLine("Entering customer facing ATM mode.");
                        Logout();
                        Console.Clear();
                        Help(true, false);
                        numMode = true;
                        repeat = false;
                        break;
                    case "2":
                        Console.WriteLine("You have selected ATM mode, this mode is intented for admin use on a numpad device, enter \"98769876\" to exit this mode.");
                        repeat = !AdminNumpadLogin();
                        numMode = true;
                        break;
                    case "3":
                        Console.WriteLine("Seeya!");
                        repeat = false;
                        Quit(ref quit);
                        break;
                    case "qwer":
                        Console.WriteLine("Switching to keyboard mode, please note this is a customer facing mode and there will be no prompts for leaving this mode once engaged. Once engaged, enter \"zxcvzxcv\" to exit this mode.");
                        Console.WriteLine("Enter \"8\" to confirm customer facing keyboard mode.");
                        repeat = true;
                        break;
                    case "8":
                        Console.WriteLine("Entering customer facing keyboard mode.");
                        numMode = false;
                        repeat = false;
                        Logout();
                        Console.Clear();
                        Help(false, false);
                        break;
                    case "asdf":
                        Console.WriteLine("Switching to keyboard Admin mode.");
                        repeat = !AdminNumpadLogin();
                        numMode = false;
                        break;
                    default:
                        Console.WriteLine($"Command not recognised, please try again");
                        repeat = true;
                        break;
                }
            } while (repeat);
        }
    }
}
