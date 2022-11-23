using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankApp.Auth;
using static BankApp.Bank;

namespace BankApp
{
    public class Account
    {
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public int Pin { get; set; }

        public Account(string name, decimal balance, int pin)
        {
            Name = name;
            Balance = balance;
            Pin = pin;
            AccountNumber = $"{accountNo}{new Random().Next(99999999)}";
            accountNo++;
        }
        public override string ToString()
        {

            return $"Account number: {AccountNumber}, Account holder: {Name}, Balance: {Balance}";
        }
    }
}
