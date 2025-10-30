namespace Task09_ClassesObjects.Models
{
    public class BankAccount
    {
        private double balance; // private is an access modifier which restricts access to the field from outside the class
        public double Balance
        {
            get { return balance; } // getter method to access the balance, get keyword is used to define a getter(which is a special method that is called when the property is accessed)
            set // setter method to set the balance, set keyword is used to define a setter(which is a special method that is called when the property is assigned a value)
            {
                if (value >= 0)

                    balance = value;
                else
                    Console.WriteLine("Invalid Balance!.");
            }
        }
        public void ShowBalance()
        {
            Console.WriteLine($"Current Balance: {Balance}");
        }
    }
}

