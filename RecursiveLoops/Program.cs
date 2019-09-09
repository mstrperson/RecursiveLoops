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
        /// takes an integer and returns the next integer.
        /// </summary>
        /// <param name="t">input</param>
        /// <returns>t+1</returns>
        static int addOne(int t) => t + 1;

        /// <summary>
        /// Simple Less than comparison.
        /// this method takes two integers and answers
        /// the question 'is a less than b?'
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
            /* 
             * print is a variable which represents a 'void' method.
             * 
             * the 'Action<int>' portion tells me that this method takes
             * a single parameter which is an integer.
             * 
             * the 'delegate' keyword declares an anonymous function
             * encapsulated in the 'Action<int>' object.
             * 
             * this particular Action is boring.  It prints the number 
             * 't' to the Console on its' own line.
             */
            Action<int> print = new Action<int>(delegate (int t) 
            {
                Console.WriteLine(t);
            });
            
            /*
             * execute a RecursiveForLoop using the 'print' action as
             * the body of the loop, over the integers [0, 10) using
             * the method 'lessThan' for comparison and 'addOne' as 
             * the incrementor.
             */
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
            /* 
             * end condition.  if the 'index' and 'limit' fail the 'comparator' condition,
             * then end the loop!
             */
            if (!comparator(index, limit))
                return;

            // execute the 'action' command for the current 'index' value.
            action(index);
            /* 
             * 'increment' the 'index' 
             * (the words seem to imply an integer context, but that isn't actually required)
             */
            index = increment(index);

            // call the next iteration of the loop!
            RecursiveForLoop<T>(action, index, limit, comparator, increment);
        }
        
        /// <summary>
        /// Test a loop that accepts addtional parameters.
        /// Prints the numbers 0-9 followed by my name.
        /// </summary>
        static void TestTwo()
        {
            /*
             * another boring Action, this time accepting an integer counting variable
             * and an arbitrary list of addtional parameters to pass into the for loop.
             * in this case, those parameters have no way to be returned /out/ of the
             * 'print' Action because /Actions/ represent 'void' methods.
             * 
             * this action takes 'args' a list of parameters passed from the RecursiveForLoop
             * call into the call to the /print/ Action.
             */
            Action<int, object[]> print = new Action<int, object[]>(delegate (int t, object[] args) 
            {
                Console.WriteLine("{0} {1}", t, args[0]);
            });

            /*
             * This call of the RecursiveForLoop method includes an additonal parameter "Jason"
             * this becomes the only element in the 'args' array which is then passed into 
             * the 'print' Action.
             * 
             * the result is calling an anonymously declared method (named 'print') which
             * prints the integers 0-9 followed by my name, "Jason"
             */
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
        /// <param name="args">list of arguments passed into the 'action'</param>
        static void RecursiveForLoop<T>(Action<T, object[]> action, T index, T limit, Func<T, T, bool> comparator, Func<T, T> increment, params object[] args)
        {
            if (!comparator(index, limit))
                return;

            /*
             * Here we see the addition of the 'args' parameter into the 
             * 'action' scope.  However, since it is passed-by-value there 
             * is no way for changes made within the 'action' scope to 
             * propogate back into this scope (and thus into the scope of 
             * the next iteration!
             */
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
            /*
             * The difference between 'Func' and 'Action' is that 'Func' (or /Function/)
             * returns data (rather than /Action/ which represents a 'void' method).
             * in this Test we provide a method which not only takes an array of arguments,
             * but also /returns/ an array of arguments to be passed to the next itteration
             * of the method.
             * 
             * This obscenely abstract function prints an ostensibly 'Fibonacci' number and
             * returns the seeds to produce the /next/ number in the sequence.
             * 
             * ***Abstraction note***
             * the implicit understanding here is that the argument list returned by this
             * Function should be the the modified values of the same list of arguments.
             * However, logically, this is not required.  This method could return an
             * entirely different set of arguments to pass to the next iteration.  In fact,
             * it does not even need to be the same /size/ list of arguments (if you are
             * clever enough to handle that!)
             * 
             * P.S. That's an enough abstraction to make even /my/ eyes spin @_@
             */
            Func<int, object[], object[]> fibonacci = 
                new Func<int, object[], object[]>
                (
                    delegate (int n, object[] args)
                    {
                        int a = (int)args[0];
                        int b = (int)args[1];

                        Console.WriteLine(a + b);
                        return new object[] { b, a + b };
                    }
                );
            

            RecursiveForLoop(fibonacci, 0, 10, lessThan, addOne, 0, 1);
        }


        /// <summary>
        /// Execute a For Loop recursively by providing the action to be done at each provided index.
        /// </summary>
        /// <typeparam name="T">A consistent data type must be used throughout</typeparam>
        /// <param name="action">Body of the loop, accepting additional parameters provided to this
        /// method.  This method returns the modified list of args to be passed along to the next iteration.</param>
        /// <param name="index">starting index when this method is initially called by the user</param>
        /// <param name="limit">ending point of the loop</param>
        /// <param name="comparator">comparison between changing index and the unchanging limit</param>
        /// <param name="increment">function which alters the value of index at the end of each loop</param>
        /// <param name="args">list of arguments passed into the 'action'</param>
        /// <returns>the altered contents of the 'args' list of parameters</returns>
        static object[] RecursiveForLoop<T>(Func<T, object[], object[]> action, T index, T limit, Func<T, T, bool> comparator, Func<T, T> increment, params object[] args)
        {
            if (!comparator(index, limit))
                return args;

            /*
             * the only difference here, is the passing of modified 'args'
             * back into this scope so that they may be passed along to the
             * next iteration of this 'loop'
             */
            args = action(index, args);
            index = increment(index);

            return RecursiveForLoop<T>(action, index, limit, comparator, increment, args);
        }
    }
}
