using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Account
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }

        public Account(string name, decimal balance, ref int accountNo)
        {
            Name = name;
            Balance = balance;
            AccountNumber = $"{accountNo}{new Random().Next(99999999)}";
            accountNo++;
        }
    }
}
