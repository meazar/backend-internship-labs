using System;
using System.Runtime.InteropServices;

namespace Task08_Collections
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("---Collections in C#---");

            // List<T> is a generic collection that can hold items of any specified type in a dynamic array.
            Console.WriteLine("\nList:");

            List<int> numbers = new List<int>() { 1, 2, 3, 4 };
            Console.WriteLine("Initial List: " + string.Join(", ", numbers));
            numbers.Add(5);
            Console.WriteLine("Updated List: " + string.Join(", ", numbers));
            Console.WriteLine("Element at index 2: " + numbers[2]);
            // loop through the list
            Console.WriteLine("Looping through the list:");
            foreach(var num in numbers)
            {
                Console.WriteLine(num);
            }

            List<string> fruits = new List<string>();
            fruits = new List<string>() { "Apple", "Banana", "Cherry" };
            Console.WriteLine("Fruits List: " + string.Join(", ", fruits));

            // Add() add an item to the end of the list
            fruits.Add("Mango");
            Console.WriteLine("After Adding Mango: " + string.Join(", ", fruits));

            // Insert() inserts an item at a specified index
            fruits.Insert(1, "Orange");
            Console.WriteLine("After Inserting Orange at index 1: " + string.Join(", ", fruits));

            // Remove() removes the first occurrence of a specific object
            fruits.Remove("Banana");
            Console.WriteLine("After removing Banana: " + string.Join(", ", fruits));

            // RemoveAt() removes the item at the specified index
            fruits.RemoveAt(2);
            Console.WriteLine("After removing item at index 2: " + string.Join(", ", fruits));

            // Count property gets the number of elements in the List
            Console.WriteLine("Number of fruits in the list: " + fruits.Count);


            // Dictionary<TKey, TValue> is a collection of key-value pairs. 
            // It is used to store data that can be quickly looked up by key.
            // like JSON objects or hash maps
            // Keys must be unique, but values can be duplicated.
            Console.WriteLine("\nDictionary:");
            Dictionary<int, string> students = new Dictionary<int, string>(); // Creating a dictionary
            students.Add(101, "Aadarsha"); // Adding key-value pairs
            students.Add(103, "Chirag");
            students.Add(102, "Divya");
            Console.WriteLine("Students Dictionary:");
            foreach (var student in students)
            {
                Console.WriteLine("ID: " + student.Key + ", Name: " + student.Value);
            }
            // Access value by key
            Console.WriteLine("Student with ID 102: " + students[102]);

            // ContainsKey() checks if a key exists in the dictionary
            Console.WriteLine("Contains key 103: " + students.ContainsKey(103));

            // Remove() removes the key-value pair with the specified key
            students.Remove(101);
            Console.WriteLine("After removing student with ID 101:");
            foreach (var kvp in students)
            {
                Console.WriteLine("ID: " + kvp.Key + ", Name: " + kvp.Value);
            }

            // Queue<T> is a collection that represents a first-in, first-out (FIFO) queue of objects.
            // Elements are added to the end of the queue and removed from the front.
            // Useful for scenarios like task scheduling or breadth-first search algorithms.
            Console.WriteLine("\nQueue:");
            Queue<string> tasks_queue = new Queue<string>(); // Creating a queue
            // Enqueue() adds an item to the end of the queue
            tasks_queue.Enqueue("Task1");
            tasks_queue.Enqueue("Task2");
            tasks_queue.Enqueue("Task3");
            tasks_queue.Enqueue("Task4");
            Console.WriteLine("Initial Queue:");
            foreach (var task in tasks_queue)
            {
                Console.WriteLine(task);
            }
            // Dequeue() removes and returns the item at the front of the queue
            string firstTask = tasks_queue.Dequeue();
            Console.WriteLine("Dequeued: " + firstTask);
            Console.WriteLine(tasks_queue.Count + " tasks remaining in the queue. Remaining tasks are: ");
            foreach (var task in tasks_queue)
            {
                Console.WriteLine(task);
            }

            // Peek() returns the item at the front of the queue without removing it
            string nextTask = tasks_queue.Peek();
            Console.WriteLine("Next task to be processed(front element): " + nextTask);


            // Stack<T> is a collection that represents a last-in, first-out (LIFO) stack of objects.
            // Elements are added and removed from the top of the stack.
            // Useful for scenarios like undo/redo functionality or depth-first search algorithms.
            Console.WriteLine("\nStack:");
            Stack<string> books_stack = new Stack<string>(); // Creating a stack
            // Push() adds an item to the top of the stack
            books_stack.Push("Book1");
            books_stack.Push("Book2");
            books_stack.Push("Book3");
            books_stack.Push("Book4");
            Console.WriteLine("Initial Stack:");
            foreach (var book in books_stack)
            {
                Console.WriteLine(book);
            }
            // Pop() removes and returns the item at the top of the stack
            string topBook = books_stack.Pop();
            Console.WriteLine("Popped: " + topBook);
            Console.WriteLine(books_stack.Count + " books remaining in the stack. Remaining books are: ");
            foreach (var book in books_stack)
            {
                Console.WriteLine(book);
            }
            // Peek() returns the item at the top of the stack without removing it
            string nextBook = books_stack.Peek();
            Console.WriteLine("Next book to be read(top element): " + nextBook);

            // HashSet<T> is a collection that contains no duplicate elements and has no particular order.
            // It is useful for scenarios where we need to ensure uniqueness, such as storing a list of unique items.
            // HashSet provides high-performance set operations like union, intersection, and difference with the provided methods like UnionWith, IntersectWith, and ExceptWith.
            // It is implemented using a hash table.
            // Elements in a HashSet are not stored in any particular order. HashSet is implemented using a hash table, which provides fast lookups and insertions.
            // HashSet is not index-based, so elements cannot be accessed by index.
            // HashSet is not thread-safe for concurrent read and write operations.
            // If multiple threads need to access a HashSet concurrently, synchronization mechanisms should be used.
            // HashSet is part of the System.Collections.Generic namespace.
            // HashSet is a generic collection that can hold items of any specified type.
            Console.WriteLine("\nHashSet:");
            HashSet<int> set = new HashSet<int>(); // creating a HashSet

            // Add() adds an item to the HashSet
            set.Add(1);
            set.Add(2);
            set.Add(3);
            set.Add(2); // duplicate, will not be added
            Console.WriteLine("HashSet elements:");
            foreach (var item in set)
            {
                Console.WriteLine(item);
            }

            // Contains() checks if an item exists in the HashSet
            Console.WriteLine("HashSet contains 2: " + set.Contains(2));
            Console.WriteLine("HashSet contains 4: " + set.Contains(4));

            // Remove() removes an item from the HashSet
            set.Remove(2); 
            Console.WriteLine("After removing 2, HashSet elements:");
            foreach (var item in set)
            {
                Console.WriteLine(item);
            }

        }
    }
}