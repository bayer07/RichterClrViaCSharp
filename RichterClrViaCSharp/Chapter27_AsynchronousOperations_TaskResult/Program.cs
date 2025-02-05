namespace Chapter27_AsynchronousOperations_TaskResult
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a Task (it does not start running now)
            Task<Int32> t = new Task<Int32>(n => Sum((Int32)n), 1000000000);
            // You can start the task sometime later
            t.Start();
            // Optionally, you can explicitly wait for the task to complete
            t.Wait(); // FYI: Overloads exist accepting timeout/CancellationToken
            // You can get the result (the Result property internally calls Wait)
            Console.WriteLine("The Sum is: " + t.Result); // An Int32 value
        }

        private static Int32 Sum(Int32 n)
        {
            Int32 sum = 0;
            for (; n > 0; n--)
                checked { sum += n; } // if n is large, this will throw System.OverflowException
            return sum;
        }
    }
}
