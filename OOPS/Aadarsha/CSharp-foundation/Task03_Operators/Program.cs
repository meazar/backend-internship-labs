using System;

namespace Task03_Operators
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // arithmetic operators
            Console.WriteLine("Arithmetic Operators:");
            int a = 10, b = 3;
            Console.WriteLine($"Addition: {a} + {b} = {a + b}");
            Console.WriteLine($"Subtraction: {a} - {b} = {a - b}");
            Console.WriteLine($"Multiplication: {a} * {b} = {a * b}");
            Console.WriteLine($"Division: {a} / {b} = {a / b}");
            Console.WriteLine($"Modulus(Remainder): {a} % {b} = {a % b}");

            // assignment operators
            Console.WriteLine("\nAssignment Operators:");
            int x = 5;
            Console.WriteLine($"Initial value: x = {x}");
            x += 2; // x = x + 2       
            Console.WriteLine($"After x += 2: x = {x}");
            x *= 3; // x = x * 3 
            Console.WriteLine($"After x *= 3: x = {x}");

            // comparison operators
            Console.WriteLine("\nComparison Operators:");
            Console.WriteLine($"{a} == {b}: {a == b}");
            Console.WriteLine($"{a} != {b}: {a != b}");
            Console.WriteLine($"{a} > {b}: {a > b}");
            Console.WriteLine($"{a} < {b}: {a < b}");
            Console.WriteLine($"{a} >= {b}: {a >= b}");
            Console.WriteLine($"{a} <= {b}: {a <= b}");

            // logical operators
            Console.WriteLine("\nLogical Operators:");
            bool isSunny = true, isWeekend = false;
            Console.WriteLine($"isSunny && isWeekend: {isSunny && isWeekend}");
            Console.WriteLine($"isSunny || isWeekend: {isSunny || isWeekend}");
            Console.WriteLine($"!isSunny: {!isSunny}"); // Negation
            Console.WriteLine($"!isWeekend: {!isWeekend}"); // Negation

            // unary operators
            Console.WriteLine("\nUnary Operators:");
            int count = 5;
            Console.WriteLine($"Initial count: {count}");
            Console.WriteLine($"Post-increment: count++ = {count++} (now count = {count})"); // after first use value is incremented
            count = 5; // reset count
            Console.WriteLine($"Pre-increment: ++count = {++count}"); // value is incremented before use
            count = 5; // reset count
            Console.WriteLine($"Post-decrement: count-- = {count--} (now count = {count})"); // after first use value is decremented
            count = 5; // reset count
            Console.WriteLine($"Pre-decrement: --count = {--count}"); // value is decremented before use

            // bitwise operators
            Console.WriteLine("\nBitwise Operators:");
            int p = 5; // 0101
            int q = 3; // 0011
            Console.WriteLine($"Bitwise AND: {p} & {q} = {p & q}"); // 0001
            Console.WriteLine($"Bitwise OR: {p} | {q} = {p | q}");  // 0111
            Console.WriteLine($"Bitwise XOR: {p} ^ {q} = {p ^ q}"); // 0110
            Console.WriteLine($"Bitwise NOT: ~{p} = {~p}"); // 1010 (inverted bits) 
            Console.WriteLine($"Left Shift: {p} << 1 = {p << 1}"); // 1010
            Console.WriteLine($"Right Shift: {p} >> 1 = {p >> 1}"); // 0010

            // ternary operator
            Console.WriteLine("\nTernary Operator:");
            int age = 20;
            string eligibility = (age >= 18 ) ? "Eligible to vote" : "Not eligible to vote";
            Console.WriteLine($"Age: {age} - {eligibility}");

            // null-coalescing operator
            Console.WriteLine("\nNull-Coalescing Operator:");
            string? name = null;
            string displayName = name ?? "Guest";
            Console.WriteLine($"Welcome, {displayName}!");

            // null-coalescing assignment operator
            Console.WriteLine("\nNull-Coalescing Assignment Operator:");
            string? country = null;
            country ??= "Nepal";
            Console.WriteLine($"Country: {country}");
        }
    }
}