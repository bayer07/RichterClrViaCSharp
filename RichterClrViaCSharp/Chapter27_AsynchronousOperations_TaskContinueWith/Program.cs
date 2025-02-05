namespace Chapter27_AsynchronousOperations_TaskContinueWith
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create and start a Task, continue with another task
            Task<Int32> t = Task.Run(() => Sum(CancellationToken.None, 6));
            // ContinueWith returns a Task but you usually don't care
            Task cwt = t.ContinueWith(task => Console.WriteLine("The sum is: " + task.Result));
            Console.ReadKey();
        }

        private static Int32 Sum(CancellationToken ct, Int32 n)
        {
            Int32 sum = 0;
            for (; n > 0; n--)
            {
                // The following line throws OperationCanceledException when Cancel
                // is called on the CancellationTokenSource referred to by the token
                ct.ThrowIfCancellationRequested();
                checked { sum += n; } // if n is large, this will throw System.OverflowException
            }
            return sum;
        }
    }
}
