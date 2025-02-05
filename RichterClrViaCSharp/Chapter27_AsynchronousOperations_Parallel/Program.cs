namespace Chapter27_AsynchronousOperations_Parallel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // One thread performs all this work sequentially
            for (Int32 i = 0; i < 3; i++) DoWork(i);

            Parallel.For(0, 3, i => DoWork(i));

            var collection = new List<int> { 1, 2, 3 };
            // One thread performs all this work sequentially
            foreach (var item in collection) DoWork(item);

            // The thread pool's threads process the work in parallel
            Parallel.ForEach(collection, item => DoWork(item));

            // One thread executes all the methods sequentially
            Method1();
            Method2();
            Method3();

            // The thread pool’s threads execute the methods in parallel
            Parallel.Invoke(
                () => Method1(),
                () => Method2(),
                () => Method3());
        }

        private static void Method3()
        {
            Console.WriteLine("Method3");
        }

        private static void Method2()
        {
            Console.WriteLine("Method2");
        }

        private static void Method1()
        {
            Console.WriteLine("Method1");
        }

        private static void DoWork(int i)
        {
            Console.WriteLine($"DoWork {i}");
        }
    }
}
