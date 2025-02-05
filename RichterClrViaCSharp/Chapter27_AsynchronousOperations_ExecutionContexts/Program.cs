using System.Collections.Concurrent;

namespace Chapter27_AsynchronousOperations_ExecutionContexts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Put some data into the Main thread's logical call context
            CallContext.SetData("Name", "Jeffrey");
            // Initiate some work to be done by a thread pool thread
            // The thread pool thread can access the logical call context data
            ThreadPool.QueueUserWorkItem(
                state => Console.WriteLine("Name={0}", CallContext.GetData("Name")));
            // Now, suppress the flowing of the Main thread's execution context
            ExecutionContext.SuppressFlow();
            // Initiate some work to be done by a thread pool thread
            // The thread pool thread CANNOT access the logical call context data
            ThreadPool.QueueUserWorkItem(
                state => Console.WriteLine("Name={0}", CallContext.GetData("Name")));
            // Restore the flowing of the Main thread's execution context in case
            // it employs more thread pool threads in the future
            ExecutionContext.RestoreFlow();
            Console.ReadLine();
        }
    }

    public static class CallContext
    {
        private static readonly ConcurrentDictionary<string, AsyncLocal<object>?> State = new();

        /// <summary>
        /// Stores a given object and associates it with the specified name.
        /// </summary>
        /// <param name="name">The name with which to associate the new item in the call context.</param>
        /// <param name="data">The object to store in the call context.</param>
        public static void SetData(string name, object? data)
        {
            State.GetOrAdd(name, static _ => new AsyncLocal<object>()).Value = data;
        }

        /// <summary>
        /// Retrieves an object with the specified name from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="name">The name of the item in the call context.</param>
        /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
        public static object? GetData(string name) =>
            State.TryGetValue(name, out AsyncLocal<object>? data) ? data?.Value : null;
    }
}
