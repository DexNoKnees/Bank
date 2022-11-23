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
        public string Password { get; set; }
        public int Pin { get; set; }
        public string Email { get; set; }

        public Account(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
            AccountNumber = $"{accountNo}{new Random().Next(99999999)}";
            accountNo++;
        }
    }
}
