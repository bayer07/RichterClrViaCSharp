namespace Chapter27_AsynchronousOperations_Tasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ComputeBoundOp, 5); // Calling QueueUserWorkItem
            new Task(ComputeBoundOp, 5).Start(); // Equivalent of preceding using Task
            Task.Run(() => ComputeBoundOp(5));
            Console.ReadKey();
        }

        private static void ComputeBoundOp(object? state)
        {
            // This method is executed by a dedicated thread
            Console.WriteLine("In ComputeBoundOp: state={0}", state);
            Thread.Sleep(1000); // Simulates other work (1 second)
            // When this method returns, the dedicated thread dies
        }
    }
}
