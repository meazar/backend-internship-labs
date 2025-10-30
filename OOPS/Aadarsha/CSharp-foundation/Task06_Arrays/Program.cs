using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("---Array in C#:---");

        int[] numbers = { 1, 2, 7, 4, 5, 9, 7 };
        // sum of all array elements
        int sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            sum += numbers[i];
        }
        Console.WriteLine($"\nSum of all numbers: {sum}");

        // finding maximum element
        int max = numbers[0];
        foreach (int num in numbers)
        {
            if (num > max)
            {
                max = num;
            }
        }
        Console.WriteLine($"\nMaximum number: {max}");

        // finding minimum element
        int min = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] < min)
            {
                min = numbers[i];
            }
        }
        Console.WriteLine($"\nMinimum number: {min}");

        // count even and odd numbers
        int evenCount = 0;
        int oddCount = 0;
        foreach (int num in numbers)
        {
            if (num % 2 == 0)
            {
                evenCount++;
            }
            else
            {
                oddCount++;
            }
        }
        Console.WriteLine($"\nEven numbers count: {evenCount}");
        Console.WriteLine($"\nOdd numbers count: {oddCount}");

        // reverse the array without using built-in methods
        Console.WriteLine("\nOriginal array:");
        foreach (int x in numbers)
        {
            Console.WriteLine(x);
        }
        Console.WriteLine("\nReversed array:");
        int[] reversedNumbers = new int[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            reversedNumbers[i] = numbers[numbers.Length - 1 - i];
        }
        foreach (int x in reversedNumbers)
        {
            Console.WriteLine(x);
        }

        // create and print a 2D array
        Console.WriteLine("\n2D Array:");
        int[,] matrix =
        {
            { 1, 2, 3, 4 },
            { 4, 5, 6, 7 },
            { 7, 8, 9, 10 },
        };
        for (int i =0; i < matrix.GetLength(0); i++)  // GetLength(0) returns the number of rows
        {
            for (int j = 0; j < matrix.GetLength(1); j++) // GetLength(1) returns the number of columns
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n---------------------------------------------------------\n");

        int[] scores = { 85, 90, 78, 92, 88 };

        Console.WriteLine("\nScores using for loop:");
        for (int i = 0; i < scores.Length; i++)
        {
            Console.WriteLine(scores[i]);
        }

        Console.WriteLine("\nScores using foreach loop:");
        foreach (int score in scores)
        {
            Console.WriteLine(score);
        }

        // Array.IndexOf to find the index of a specific score
        Console.WriteLine("\nIndexOf operator:");
        Console.WriteLine($"Index of score 92: {Array.IndexOf(scores, 92)}");

        // Array.ForEach method is used to perform an action on each element of the array with a lambda expression without modifying the original array
        Console.WriteLine("\nScores using Array.ForEach method to increase marks by 1:");
        Array.ForEach(scores, score => Console.WriteLine(score + 1));

        // string.Join to concatenate array elements into a single string with a specified separator
        Console.WriteLine("\nScores using string.Join method:");
        Console.WriteLine(string.Join(", ", scores));

        // LINQ is used to perform queries on arrays and collections, providing a more functional programming approach and
        // enabling operations like filtering, projection, and aggregation. Here, we use it to iterate through the array and print each score.
        Console.WriteLine("\nScores using LINQ:");
        System.Linq.Enumerable.ToList(scores).ForEach(score => Console.WriteLine(score));

        // Array.ConvertAll is used to convert each element of the array using a specified conversion function without modifying the original array.
        // Here, we use it to print half of each score while returning the original score.
        Console.WriteLine("\nScores using Array.ConvertAll method:");
        Array.ConvertAll(scores, score => { Console.WriteLine(score * 0.5); return score; });

        // Span<T> provides a type-safe and memory-safe representation of a contiguous region of arbitrary memory.
        Console.WriteLine("\nScores using Span<T>:");
        Span<int> scoreSpan = scores;
        foreach (var score in scoreSpan)
        {
            Console.WriteLine(score);
        }

        // Parallel.ForEach is used to perform parallel iterations over a collection, allowing for concurrent processing of elements.
        // This can improve performance for large datasets by utilizing multiple threads.
        // Thread safety is ensured as each iteration works on a separate copy of the data.
        // Here, we use it to print each score in parallel.
        // Note: The order of output may vary due to the concurrent nature of parallel processing.
        Console.WriteLine("\nScores using Parallel.ForEach:");
        System.Threading.Tasks.Parallel.ForEach(scores, score => Console.WriteLine(score));

        // ArraySegment<T> is used to create a segment or a slice of an existing array without copying the data.
        // This allows for efficient manipulation and access to a portion of the array.
        // Here, we use it to print each score from the original array.
        Console.WriteLine("\nScores using ArraySegment<T>:");
        ArraySegment<int> segment = new ArraySegment<int>(scores);
        foreach (var score in segment)
        {
            Console.WriteLine(score);
        }

        // Buffer.BlockCopy is used to copy a specified number of bytes from one array to another.
        // It is a low-level operation that provides efficient memory copying.
        // Here, we use it to copy the scores array into a new array and print each score.
        // Note: The destination array must be of the same type and size to avoid runtime errors.
        Console.WriteLine("\nScores using Buffer.BlockCopy:");
        int[] copiedScores = new int[scores.Length];
        Buffer.BlockCopy(scores, 0, copiedScores, 0, scores.Length * sizeof(int));
        foreach (var score in copiedScores)
        {
            Console.WriteLine(score);
        }

        // Memory<T> provides a type-safe and memory-safe representation of a contiguous region of memory.
        // It is similar to Span<T> but can be used in asynchronous programming scenarios.
        // Here, we use it to print each score from the original array.
        // Note: Memory<T> can be used in async methods, while Span<T> cannot.
        Console.WriteLine("\nScores using Memory<T>:");
        Memory<int> scoreMemory = scores;
        foreach (var score in scoreMemory.Span)
        {
            Console.WriteLine(score);
        }

        // Array.FindAll is used to find all elements in an array that match a specified condition.
        // Here, we use it to return all scores greater than or equal to 90.
        // This effectively creates a copy of the original array.
        // Note: The original array remains unchanged.
        Console.WriteLine("\nScores using Array.FindAll method:");
        int[] allScores = Array.FindAll(scores, score => score >= 90);
        foreach (var score in allScores)
        {
            Console.WriteLine(score);
        }

        // Array.Reverse is used to reverse the order of elements in an array.
        // Here, we create a copy of the original array and reverse it before printing.
        // Note: The original array remains unchanged.
        Console.WriteLine("\nScores using Array.Reverse method:");
        int[] reversedScores = (int[])scores.Clone();
        Array.Reverse(reversedScores);
        foreach (var score in reversedScores)
        {
            Console.WriteLine(score);
        }

        // Array.Sort is used to sort the elements of an array in ascending order.
        // Here, we create a copy of the original array and sort it before printing.
        // Note: The original array remains unchanged.
        Console.WriteLine("\nScores using Array.Sort method:");
        int[] sortedScores = (int[])scores.Clone();
        Array.Sort(sortedScores);
        foreach (var score in sortedScores)
        {
            Console.WriteLine(score);
        }

        // Array.Copy is used to copy elements from one array to another.
        // Here, we create a new array and copy the elements of the original array into it before printing.
        // Note: The original array remains unchanged.
        Console.WriteLine("\nScores using Array.Copy method:");
        int[] copiedArrayScores = new int[scores.Length];
        Array.Copy(scores, copiedArrayScores, scores.Length);
        foreach (var score in copiedArrayScores)
        {
            Console.WriteLine(score);
        }
    }
}