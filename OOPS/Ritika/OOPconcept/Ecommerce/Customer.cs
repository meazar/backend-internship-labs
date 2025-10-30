using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class Customer
    {
        public string Name {  get; set; }

        public Customer(string name)
        {
            Name = name;
        }
        public virtual double GetDiscount(double totalAmount)
        {
            return 0;
        }

    }

    public class PremiumCustomer: Customer
    {
        public PremiumCustomer(string name): base(name) { }

        public override double GetDiscount(double totalAmount)
        {
            return totalAmount * 0.10;
        }
    }

    public class VIPCustomer: Customer
    {
        public VIPCustomer(string name): base(name) { }
        public override double GetDiscount(double totalAmount)
        {
            return totalAmount*0.25;
        }


    }
}
