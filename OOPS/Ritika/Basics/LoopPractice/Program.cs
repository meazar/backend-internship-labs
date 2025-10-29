using System;
using LoopPractice;
namespace Basics;

class Program
{
    static void Main(string[] args)
    {
        Multiplication mul = new Multiplication();
        mul.Multiplications();

        SumNaturalNumber snn = new SumNaturalNumber();
        snn.Sum();

        FactorialNumber fn = new FactorialNumber();
        fn.FactorialNumbers();
    }
}
