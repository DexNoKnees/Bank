using BankApp;
using System;
using System.Security.Cryptography;
using static BankApp.Bank;
using static BankApp.ValidationHelper;

internal class Program
{
    private static void Main(string[] args)
    {
        Bank.Accounts = new List<Account>();
        var quit = false;
        var numMode = true;
        Console.WriteLine("Welcome, type help for comands");
        do
        {
            var read = Console.ReadLine().ToLower();
            if (!numMode)
            {
                Console.WriteLine("You are in keyboard mode.");
                switch (read)
                {
                    case "4321":
                        numMode = true;
                        Console.WriteLine("Switching to numpad mode.");
                        Console.WriteLine("\"1\" for account withdrawals");
                        Console.WriteLine("\"2\" for account deposits");
                        break;
                    case "quit":
                        quit = true;
                        break;
                    case "help":
                        Help(numMode);
                        break;
                    case string when read.Contains("create"):
                        var create = read.Split(' ');
                        var createValidation = create.ValidateAmount(Actions.Create);
                        if (createValidation.Passed)
                        {
                            Create($"{create[1]} {create[2]}", createValidation.Amount);
                        }
                        break;
                    case string when read.Contains("withdraw"):
                        var withdraw = read.Split(' ');
                        var withdrawValidation = withdraw.ValidateAmount(Actions.Withdraw);
                        if (withdrawValidation.Passed)
                        {
                            Withdraw(withdraw[1], withdrawValidation.Amount!);
                        }
                        break;
                    case string when read.Contains("deposit"):
                        var deposit = read.Split(' ');
                        var depositValidation = deposit.ValidateAmount(Actions.Deposit);
                        if (depositValidation.Passed)
                        {
                            Deposit(deposit[1], depositValidation.Amount);
                        }
                        break;
                    default:
                        Console.WriteLine($"Command not recognised, you are currently in {(numMode ? "Num pad mode" : "Keyboard mode")}, type help for a list of available commands");
                        break;
                }
            }
            else
            {
                Console.WriteLine("You are in num pad mode.");
                Console.WriteLine("\"1\" for account withdrawals");
                Console.WriteLine("\"2\" for account deposits");
                switch (read)
                {
                    case "1234":
                        numMode = false;
                        Console.WriteLine("Switching to keyboard mode.");
                        break;
                    case "quit":
                        quit = true;
                        break;
                    case "help":
                        Help(numMode);
                        break;
                    case "1":
                        Console.WriteLine("Account withdrawals.");
                        Console.WriteLine("Please enter your account number");
                        var withdrawalAccNo = Console.ReadLine();
                        var withdrawalAccount = Accounts.FirstOrDefault(a => a.AccountNumber == withdrawalAccNo);
                        if (withdrawalAccount == null)
                        {
                            Console.WriteLine("Account not found, please check your account number or create an account to get started");
                            return;
                        }
                        Console.WriteLine("Please enter the withdrawal amount");
                        var withdrawalAmount = Console.ReadLine();
                        if(!decimal.TryParse(withdrawalAmount, out var withdrawalAmountParse))
                        {
                            Console.WriteLine($"Failed to parse {ActionText[Actions.Withdraw].AmountText}, please ensure that the ammount is only numeric and does not include a currency sign");
                            return;
                        }
                        if (withdrawalAccount!.Balance < withdrawalAmountParse)
                        {
                            Console.WriteLine("lmao you broke boi");
                            return;
                        }
                        Withdraw(withdrawalAccNo!, withdrawalAmountParse);
                        break;
                    case "2":
                        Console.WriteLine("Account withdrawals.");
                        Console.WriteLine("Please enter your account number");
                        var depositAccNo = Console.ReadLine();
                        var account = Accounts.FirstOrDefault(a => a.AccountNumber == depositAccNo);
                        if (account == null)
                        {
                            Console.WriteLine("Account not found, please check your account number or create an account to get started");
                            return;
                        }
                        Console.WriteLine("Please enter the withdrawal amount");
                        var depositAmount = Console.ReadLine();
                        if (!decimal.TryParse(depositAmount, out var depositAmountParse))
                        {
                            Console.WriteLine($"Failed to parse {ActionText[Actions.Withdraw].AmountText}, please ensure that the ammount is only numeric and does not include a currency sign");
                            return;
                        }
                        Deposit(depositAccNo!, depositAmountParse);
                        break;
                    default:
                        Console.WriteLine($"Command not recognised, you are currently in {(numMode ? "Num pad mode" : "Keyboard mode")}, type help for a list of available commands");
                        break;
                }
            }
        } while (!quit);
    }
}