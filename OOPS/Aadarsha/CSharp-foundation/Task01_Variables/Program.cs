class Program
{
       static void Main()
    {
        // variables declaration and initialization
        int age = 100;
        double salary = 113.32;
        string name = "Aadarsha";
        dynamic isDev = true;
        const double pi = 3.14159;

        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Age: {age}");
        Console.WriteLine($"Salary: {salary}");
        Console.WriteLine($"Is Developer: {isDev}");
        Console.WriteLine($"Pi: {pi}");

        // value type (stack memory allocation)
        int a = 10;
        int b = a;
        b = 20;
        Console.WriteLine("a: " + a);
        Console.WriteLine("b: " + b);

        // reference type (heap memory allocation)
        int[] arr1 = { 1, 2, 3, 4, 5 };
        int[] arr2 = arr1; // arr2 references the same array as arr1
        arr2[0] = 111;
        Console.WriteLine("arr1[0]: " + arr1[0]);
        Console.WriteLine("arr2[0]: " + arr2[0]);

        // Type Conversion: implicit and Explicit
        // implicit conversion(safe, no data loss, done automatically by compiler)
        int num = 3;
        double intPI = num;
        Console.WriteLine("num: " + num);
        Console.WriteLine("intPI: " + intPI);

        // explicit conversion(possible data loss, done manually by programmer)
        double gpa = 3.9;
        int intGPA = (int)gpa;
        Console.WriteLine("gpa: " + gpa);
        Console.WriteLine("intGPA: " + intGPA);

        // Using Convert or Parse
        string s = "1234";
        int intS = Convert.ToInt32(s);
        int parsedS = int.Parse(s);
        Console.WriteLine("s: " + s);
        Console.WriteLine("intS: " + intS);
        Console.WriteLine("parsedS: " + parsedS);

        // TryParse (safe parsing)
        string s2 = "1234a";
        bool success = int.TryParse(s2, out int intS2);
        if (success)
        {
            Console.WriteLine("s2: " + s2);
            Console.WriteLine("intS2: " + intS2);
        }
        else
        {
            Console.WriteLine($"Failed to parse {s2}.");
        }

        // Boxing and Unboxing
        int val = 500;
        object obj = val; // Boxing
        int unboxedVal = (int)obj; // Unboxing
        Console.WriteLine("val: " + val);
        Console.WriteLine("obj: " + obj);
        Console.WriteLine("unboxedVal: " + unboxedVal);

        // Nullable types
        int? nullableInt = null;
        if (nullableInt.HasValue)
        {
            Console.WriteLine("nullableInt: " + nullableInt.Value);
        }
        else
        {
            Console.WriteLine("nullableInt is null.");
        }

        // Using null-coalescing operator
        int? nullableInt2 = null;
        int nonNullableInt = nullableInt2 ?? -1;
        Console.WriteLine("nonNullableInt: " + nonNullableInt); 


    }
}