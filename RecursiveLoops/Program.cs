using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveLoops
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test 1:");
            TestOne();
            Console.ReadLine();

            Console.WriteLine("_____________________________________\nTest 2:");
            TestTwo();
            Console.ReadLine();

            Console.WriteLine("_____________________________________\nTest 3:");
            TestThree();
            Console.ReadLine();
        }

        /// <summary>
        /// Simple increment method.
        /// </summary>
        /// <param name="t">input</param>
        /// <returns>t+1</returns>
        static int addOne(int t) => t + 1;

        /// <summary>
        /// Simple Less than comparison.
        /// </summary>
        /// <param name="a">left operand</param>
        /// <param name="b">right operand</param>
        /// <returns>a < b</returns>
        static bool lessThan(int a, int b) => a < b;

        /// <summary>
        /// Simple Test that prints the number 0 to 9
        /// </summary>
        static void TestOne()
        {
            Action<int> print = new Action<int>(delegate (int t) 
            {
                Console.WriteLine(t);
            });

            RecursiveForLoop<int>(print, 0, 10, lessThan, addOne);
        }

        /// <summary>
        /// Execute a For Loop recursively by providing the action to be done at each provided index.
        /// </summary>
        /// <typeparam name="T">A consistent data type must be used throughout</typeparam>
        /// <param name="action">Body of the loop</param>
        /// <param name="index">starting index when this method is initially called by the user</param>
        /// <param name="limit">ending point of the loop</param>
        /// <param name="comparator">comparison between changing index and the unchanging limit</param>
        /// <param name="increment">function which alters the value of index at the end of each loop</param>
        static void RecursiveForLoop<T>(Action<T> action, T index, T limit, Func<T, T, bool> comparator, Func<T, T> increment)
        {
            if (!comparator(index, limit))
                return;

            action(index);
            index = increment(index);

            RecursiveForLoop<T>(action, index, limit, comparator, increment);
        }
        
        /// <summary>
        /// Test a loop that accepts addtional parameters.
        /// Prints the numbers 0-9 followed by my name.
        /// </summary>
        static void TestTwo()
        {
            Action<int, object[]> print = new Action<int, object[]>(delegate (int t, object[] args) { Console.WriteLine("{0} {1}", t, args[0]); });
            

            RecursiveForLoop<int>(print, 0, 10, lessThan, addOne, "Jason");
        }

        /// <summary>
        /// Execute a For Loop recursively by providing the action to be done at each provided index.
        /// </summary>
        /// <typeparam name="T">A consistent data type must be used throughout</typeparam>
        /// <param name="action">Body of the loop, accepting additional parameters provided to this method</param>
        /// <param name="index">starting index when this method is initially called by the user</param>
        /// <param name="limit">ending point of the loop</param>
        /// <param name="comparator">comparison between changing index and the unchanging limit</param>
        /// <param name="increment">function which alters the value of index at the end of each loop</param>
        static void RecursiveForLoop<T>(Action<T, object[]> action, T index, T limit, Func<T, T, bool> comparator, Func<T, T> increment, params object[] args)
        {
            if (!comparator(index, limit))
                return;

            action(index, args);
            index = increment(index);

            RecursiveForLoop<T>(action, index, limit, comparator, increment, args);
        }
        
        /// <summary>
        /// Test using a function that can pass data between iterations.
        /// prints the first 10 fibonacci numbers
        /// </summary>
        static void TestThree()
        {
            Func<int, object[], object[]> fibonacci = new Func<int, object[], object[]>(delegate (int n, object[] args)
            {
                int a = (int)args[0];
                int b = (int)args[1];

                Console.WriteLine(a + b);
                return new object[] { b, a + b };
            });
            

            RecursiveForLoop(fibonacci, 0, 10, lessThan, addOne, 0, 1);
        }


        /// <summary>
        /// Execute a For Loop recursively by providing the action to be done at each provided index.
        /// </summary>
        /// <typeparam name="T">A consistent data type must be used throughout</typeparam>
        /// <param name="action">Body of the loop, accepting additional parameters provided to this method</param>
        /// <param name="index">starting index when this method is initially called by the user</param>
        /// <param name="limit">ending point of the loop</param>
        /// <param name="comparator">comparison between changing index and the unchanging limit</param>
        /// <param name="increment">function which alters the value of index at the end of each loop</param>
        static object[] RecursiveForLoop<T>(Func<T, object[], object[]> action, T index, T limit, Func<T, T, bool> comparator, Func<T, T> increment, params object[] args)
        {
            if (!comparator(index, limit))
                return args;


            args = action(index, args);
            index = increment(index);

            return RecursiveForLoop<T>(action, index, limit, comparator, increment, args);
        }
    }
}
