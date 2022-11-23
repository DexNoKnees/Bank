using BankApp;
using System;
using System.Security.Cryptography;
using static BankApp.Bank;
using static BankApp.ValidationHelper;
using static BankApp.InputHandler;
using static BankApp.Auth;

internal class Program
{
    private static void Main(string[] args)
    {
        Users = new List<User>();
        var quit = false;
        var numMode = true;

        Setup(ref quit, ref numMode);


        do
        {
            var read = Console.ReadLine().ToLower();

            switch (numMode)
            {
                case true:
                    HandleNumpad(read, ref quit, ref numMode);
                    break;
                case false:
                    HandleKeyboard(read, ref quit, ref numMode);
                    break;
            }
        } while (!quit);
    }
}