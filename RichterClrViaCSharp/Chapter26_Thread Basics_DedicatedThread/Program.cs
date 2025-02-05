namespace Chapter26_Thread_Basics_DedicatedThread
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main thread: starting a dedicated thread to do an asynchronous operation");
            Thread dedicatedThread = new Thread(ComputeBoundOp);
            dedicatedThread.Start(5);
            Console.WriteLine("Main thread: Doing other work here...");
            Thread.Sleep(10000); // Simulating other work (10 seconds)
            dedicatedThread.Join(); // Wait for thread to terminate
            Console.WriteLine("Hit <Enter> to end this program...");
            Console.ReadLine();
        }

        // This method's signature must match the ParameterizedThreadStart delegate
        private static void ComputeBoundOp(object? state)
        {
            // This method is executed by a dedicated thread
            Console.WriteLine("In ComputeBoundOp: state={0}", state);
            Thread.Sleep(1000); // Simulates other work (1 second)
            // When this method returns, the dedicated thread dies
        }
    }
}
