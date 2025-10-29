using System;
using Basics;
class Program
{
    static void Main(String[] args)
    {
        SwappingNumbers swp = new SwappingNumbers();
        swp.SwappingNumber();

        SwapWithThirdVariable swtv = new SwapWithThirdVariable();
        swtv.SwapWithThirdVariables();

        OddOrEven oe = new OddOrEven();
        oe.OddEven();

        LargestNumber ln = new LargestNumber();
        ln.LargestNumbers();

    }
}