using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursionMethod
{
    class Factorial
    {
        public long Factor(int n)
        {
            if (n == 0 || n == 1)
                return 1;
            else
                return n * Factor(n - 1);

        }

        

    }
    

}
