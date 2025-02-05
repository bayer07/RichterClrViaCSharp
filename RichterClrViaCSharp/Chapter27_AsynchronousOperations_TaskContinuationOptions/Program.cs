namespace Chapter27_AsynchronousOperations_TaskContinuationOptions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            // Create and start a Task, continue with multiple other tasks
            Task<Int32> t = Task.Run(() => Sum(CancellationToken.None, 1000), cts.Token);
            cts.Cancel();
            // Each ContinueWith returns a Task but you usually don't care
            t.ContinueWith(task => Console.WriteLine("The sum is: " + task.Result),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            t.ContinueWith(task => Console.WriteLine("Sum threw: " + task.Exception.InnerException),
                TaskContinuationOptions.OnlyOnFaulted);
            t.ContinueWith(task => Console.WriteLine("Sum was canceled"),
                TaskContinuationOptions.OnlyOnCanceled);
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
