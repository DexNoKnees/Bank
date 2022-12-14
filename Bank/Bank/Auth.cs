using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics.Contracts;
using static BankApp.Bank;

namespace BankApp
{
    public static class Auth
    {
        public static User? activeUser = null;

        public class User
        {
            public List<Account> UserAccounts = new();
            public string Username;
            public string Password;
            public int UserId;
            public string Email;
            public bool IsAdmin;

            public User() { }

            public User(int? userId = null)
            {
                if (userId == null)
                {
                    UserId = int.Parse($"{userNo}{new Random().Next(99999999)}");
                }
            }

            public User(string accountName, decimal initialBalance, int pin)
            {
                UserId = int.Parse($"{userNo}{new Random().Next(99999999)}");

                UserAccounts.Add(new Account(accountName, initialBalance, pin));
            }

            public override string ToString()
            {
                var printUser = $"UserId: {UserId}, UserName: {Username} {(IsAdmin ? " Admin": string.Empty)}\n";
                List<string> printAccounts = new List<string>();
                foreach (var account in UserAccounts)
                {
                    printAccounts.Add("\t" + account.ToString() + "\n");
                }
                return printUser + string.Join(null, printAccounts);
            }
        }

        public static void Login()
        {
            Console.WriteLine("Please enter your account number ");


            //var account = accounts.FirstOrDefault(a => a.AccountNumber == accNo
        }
        public static void Logout()
        {
            activeUser = null;
            Console.WriteLine("byebye");

            //var account = accounts.FirstOrDefault(a => a.AccountNumber == accNo
        }

        public static bool AdminNumpadLogin()
        {
            Console.WriteLine("Please enter your admin account number");
            var accNo = Console.ReadLine();
            User adminUser = null;
            if (!Users.Any(u => u.UserAccounts.Any(a =>
                {
                    if (a.AccountNumber == accNo && u.IsAdmin)
                    {
                        adminUser = u;
                        return true;
                    }
                    return false;
                })))
            {
                Console.WriteLine("Your account number is either incorrect or not an admin account. Please reselect your mode and try again or contact an administrator.");
                return false;
            }
            Console.WriteLine("Please enter your admin passcode");
            var attempts = 0;
            while (attempts < 3)
            {
                if (Console.ReadLine() == adminUser.Password)
                {
                    Console.WriteLine($"Welcome! {adminUser.Username}");
                    activeUser = adminUser;
                    return true;
                }
                else
                {
                    Console.WriteLine("Passcode incorrect, please try again.");
                    attempts++;
                }
            }
            Console.WriteLine("Too many incorrect password attempts, please contact your administrator.");
            return false;
        }

        public static bool AccountAuth(this Account account)
        {
            Console.WriteLine("Please enter your account pin");
            var attempts = 0;
            while (attempts < 3)
            {
                int.TryParse(Console.ReadLine(), out int pin);
                if (pin == account.Pin)
                {
                    Console.WriteLine($"Welcome! {account.Name}");
                    return true;
                }
                else
                {
                    Console.WriteLine("Pin incorrect, please try again.");
                    attempts++;
                }
            }
            Console.WriteLine("Too many incorrect attempts, please contact your administrator.");
            return false;
        }

        public static void LoginSetup(string username, string password)
        {
            Console.WriteLine("Welcome! To create your account we need to collect some information.");
            Console.WriteLine("Please enter your full name");
            var name = Console.ReadLine();

            password = password.Trim().HashPassword();
            //Bank.Create(username, 0);
        }

        private static string HashPassword(this string password)
        {
            string sSourceData;
            byte[] tmpSource;
            byte[] tmpHash;

            sSourceData = "MySourceData";
            //Create a byte array from source data.
            tmpSource = Encoding.ASCII.GetBytes(sSourceData);

            //Compute hash based on source data.
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            return ByteArrayToString(tmpHash);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
