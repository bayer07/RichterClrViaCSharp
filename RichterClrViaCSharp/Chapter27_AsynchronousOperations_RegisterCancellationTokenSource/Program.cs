namespace Chapter27_AsynchronousOperations_RegisterCancellationTokenSource
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("Canceled 1"));
            cts.Token.Register(() => Console.WriteLine("Canceled 2"));
            // To test, let's just cancel it now and have the 2 callbacks execute
            cts.Cancel();

            Console.ReadKey();
        }
    }
}
