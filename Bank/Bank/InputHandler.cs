using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static BankApp.Bank;
using static BankApp.ValidationHelper;
using static BankApp.Auth;
using System.ComponentModel.Design.Serialization;

namespace BankApp
{
    public static class InputHandler
    {

        public static void HandleNumpad(string input, ref bool quit, ref bool numMode)
        {
            switch (input)
            {
                case "4":
                    quit = true;
                    break;
                case "3":
                    Help(true, false);
                    break;
                case "1":
                    Console.WriteLine("Account withdrawals.");
                    Console.WriteLine("Please enter your account number");

                    var withdrawAcc = ValidateAcc(Console.ReadLine());
                    if (withdrawAcc == null)
                    {
                        break;
                    }

                    Console.WriteLine("Please enter the withdrawal amount");
                    var withdrawAmount = Console.ReadLine().ValidateTransactionInput();
                    if (withdrawAmount == null)
                    {
                        break;
                    }

                    Withdraw(withdrawAcc, withdrawAmount.Value);
                    break;
                case "2":
                    Console.WriteLine("Account deposits.");
                    Console.WriteLine("Please enter your account number");

                    var depositAcc = ValidateAcc(Console.ReadLine());
                    if (depositAcc == null)
                    {
                        break;
                    }

                    Console.WriteLine("Please enter the deposit amount");
                    var depositAmount = Console.ReadLine().ValidateTransactionInput();
                    if (depositAmount == null)
                    {
                        break;
                    }

                    Deposit(depositAcc, depositAmount.Value);
                    break;
                case "5" when activeUser?.IsAdmin ?? false:
                    Console.WriteLine("tbc");
                    break;
                case "6" when activeUser?.IsAdmin ?? false:
                    Console.WriteLine("tbc");
                    break;
                case "7" when activeUser?.IsAdmin ?? false:
                    Console.WriteLine("tbc");
                    break;
                case "10" when activeUser?.IsAdmin ?? false:
                    Console.WriteLine("tbc");
                    break;
                case "11" when activeUser?.IsAdmin ?? false:
                    Setup(ref quit, ref numMode);
                    break;
                case "98769876":
                    Setup(ref quit, ref numMode);
                    break;
                default:
                    Console.WriteLine($"Command not recognised, enter \"3\" for help");
                    break;
            }
        }

        public static void HandleKeyboard(string input, ref bool quit, ref bool numMode)
        {
            switch (input)
            {
                case "quit":
                    quit = true;
                    break;
                case "help":
                    Help(false, true);
                    break;
                case string when input.Contains("create"):
                    var create = input.Split(' ');

                    if (!create.ValidateKeyboardCommandInput(Actions.Create))
                    {
                        break;
                    }

                    var createAmount = create[3].ValidateTransactionInput();
                    if (createAmount == null)
                    {
                        break;
                    }

                    Create($"{create[1]} {create[2]}", createAmount.Value);
                    break;
                case string when input.Contains("withdraw"):
                    var withdraw = input.Split(' ');

                    if (!withdraw.ValidateKeyboardCommandInput(Actions.Withdraw))
                    {
                        break;
                    }

                    var withdrawAcc = ValidateAcc(withdraw[1]);
                    if (withdrawAcc == null)
                    {
                        break;
                    }

                    var withdrawAmount = withdraw[3].ValidateTransactionInput();
                    if (withdrawAmount == null)
                    {
                        break;
                    }

                    Withdraw(withdrawAcc, withdrawAmount.Value);
                    break;
                case string when input.Contains("deposit"):
                    var deposit = input.Split(' ');

                    if (!deposit.ValidateKeyboardCommandInput(Actions.Withdraw))
                    {
                        break;
                    }

                    var depositAcc = ValidateAcc(deposit[1]);
                    if (depositAcc == null)
                    {
                        break;
                    }

                    var depositAmount = deposit[3].ValidateTransactionInput();
                    if (depositAmount == null)
                    {
                        break;
                    }

                    Deposit(depositAcc, depositAmount.Value);
                    break;
                case string when input.Contains("delete") && (activeUser?.IsAdmin ?? false):
                    Console.WriteLine("tbc");
                    break;
                case string when input.Contains("suspend") && (activeUser?.IsAdmin ?? false):
                    Console.WriteLine("tbc");
                    break;
                case string when input.Contains("unsuspend") && (activeUser?.IsAdmin ?? false):
                    Console.WriteLine("tbc");
                    break;
                case string when input.Contains("manage") && (activeUser?.IsAdmin ?? false):
                    Console.WriteLine("tbc");
                    break;
                case "zxcvzxcv":
                    Setup(ref quit, ref numMode);
                    break;
                case "4321":
                    numMode = true;
                    Console.WriteLine($"Switching to numpad {(activeUser?.IsAdmin ?? false ? "admin " : "")}mode.");
                    Help(numMode, activeUser?.IsAdmin ?? false);
                    break;
                default:
                    Console.WriteLine($"Command not recognised, you are currently in Keyboard mode type \"help\" for a list of available commands");
                    break;
            }
        }
    }
}
