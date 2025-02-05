namespace Chapter27_AsynchronousOperations_ChildTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<Int32[]> parent = new Task<Int32[]>(() =>
            {
                var results = new Int32[3]; // Create an array for the results
                // This tasks creates and starts 3 child tasks
                new Task(() => results[0] = Sum(1), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = Sum(2), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = Sum(3), TaskCreationOptions.AttachedToParent).Start();
                // Returns a reference to the array (even though the elements may not be initialized yet)
                return results;
            });
            // When the parent and its children have run to completion, display the results
            var cwt = parent.ContinueWith(
                parentTask => Array.ForEach(parentTask.Result, Console.WriteLine));
            // Start the parent Task so it can start its children
            parent.Start();
            Console.ReadKey();
        }

        private static Int32 Sum(Int32 n)
        {
            Int32 sum = 0;
            for (; n > 0; n--)
            {
                checked { sum += n; } // if n is large, this will throw System.OverflowException
            }
            return sum;
        }
    }
}
