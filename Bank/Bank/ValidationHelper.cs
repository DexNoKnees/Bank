using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static BankApp.Bank;

namespace BankApp
{
    public static class ValidationHelper
    {
        public static (bool Passed, decimal Amount) ValidateAmount(this string[] input, Actions action)
        {
            if (input.Length < ActionText[action].RequestLength)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[action].InputFormat}\", ensuring there are spaces between each element");
                //Not ideal
                return (false, 0);
            }
            if (input.Length > ActionText[action].RequestLength)
            {
                Console.WriteLine($"Incorrect number of elements dectected, please use the format \"{ActionText[action].InputFormat}\" {(action == Actions.Create ? "and hyphenate any multi part names" : string.Empty)}");
                return (false, 0);
            }
            var amountText = input[ActionText[action].RequestLength - 1];
            var parsed = decimal.TryParse(amountText, out var amount);
            if (!parsed)
            {
                if (input.Length == ActionText[action].RequestLength)
                {
                    Console.WriteLine($"Failed to parse {ActionText[action].AmountText}, please ensure that the ammount is only numeric and does not include a currency sign");
                    return (false, 0);
                }
            }
            return (parsed, amount);
        }
    }
}
