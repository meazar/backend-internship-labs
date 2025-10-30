namespace Task09_ClassesObjects.Models;
public class Account
{
    public string? Name;
    public string? AccountNumber;
    private double? balance;

    public Account(string name, string accountNumber, double initialBalance)
    {
        Name = name;
        AccountNumber = accountNumber;
        balance = initialBalance;
    }
    public void Deposit(double amount)
    {
        balance += amount;
        Console.WriteLine($"{amount} deposited successfully. Current balance: {balance}");
    }
    public void Withdraw(double amount)
    {
        if (amount <= balance)
        {
            balance -= amount;
            Console.WriteLine($"{amount} withdrawn successfully. Current balance: {balance}");
        }
        else
            Console.WriteLine("Insufficient balance.");
    }
    public void DisplayBalance()
    {
        Console.WriteLine($"Account: {AccountNumber}, Current balance: {balance}");
    }   
}