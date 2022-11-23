using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static BankApp.Bank;
using static BankApp.Auth;
using static System.Collections.Specialized.BitVector32;

namespace BankApp
{
    public static class ValidationHelper
    {
        public static bool ValidateKeyboardCommandInput(this string[] input, Actions action)
        {
            if (input.Length < ActionText[action].RequestLength)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[action].InputFormat}\", ensuring there are spaces between each element");
                //Not ideal
                return false;
            }
            if (input.Length > ActionText[action].RequestLength)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[action].InputFormat}\" {(action == Actions.Create ? "and hyphenate any multi part names" : string.Empty)}");
                return false;
            }
            return true;
        }

        public static (bool Valid, int? Pin) ValidatePin(string pin)
        {
            if (pin.Length > 8 || pin.Length < 4)
            {
                return (false, null);
            }
            var valid = int.TryParse(pin, out int intPin);
            return (valid, intPin);
        }

        public static decimal? ValidateTransactionInput(this string input)
        {
            if (!decimal.TryParse(input, out var amount))
            {
                Console.WriteLine($"Failed to parse transaction ammount, please ensure that the ammount is only numeric and does not include a currency sign");
                return null;
            }
            return amount;
        }

        public static Account? ValidateAcc(string accNo)
        {
            Account? account = null;

            if (!Users.Any(u => u.UserAccounts.Any(a =>
            {
                if (a.AccountNumber == accNo)
                {
                    activeUser = u;
                    account = a;
                    return true;
                }
                return false;
            }
            )))
            {
                Console.WriteLine("Account not found, please check your account number or create an account to get started");
                return null;
            }
            return account;
        }
    }
}
