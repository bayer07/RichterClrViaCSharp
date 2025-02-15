﻿using System.Text;

namespace Chapter27_AsynchronousOperations_TaskFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task parent = new Task(() =>
            {
                var cts = new CancellationTokenSource();
                var tf = new TaskFactory<Int32>(cts.Token, TaskCreationOptions.AttachedToParent,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                // This task creates and starts 3 child tasks
                var childTasks = new[] {
                    tf.StartNew(() => Sum(cts.Token, 10000)),
                    tf.StartNew(() => Sum(cts.Token, 20000)),
                    tf.StartNew(() => Sum(cts.Token, Int32.MaxValue)) // Too big, throws OverflowException
                };
                // If any of the child tasks throw, cancel the rest of them
                for (Int32 task = 0; task < childTasks.Length; task++)
                    childTasks[task].ContinueWith(
                        t => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
                // When all children are done, get the maximum value returned from the
                // nonfaulting/canceled tasks. Then pass the maximum value to another
                // task that displays the maximum result
                tf.ContinueWhenAll(
                        childTasks,
                        completedTasks =>
                            completedTasks.Where(t => t.Status == TaskStatus.RanToCompletion).Max(t => t.Result),
                        CancellationToken.None)
                    .ContinueWith(t => Console.WriteLine("The maximum is: " + t.Result),
                        TaskContinuationOptions.ExecuteSynchronously);
            });

            // When the children are done, show any unhandled exceptions too
            parent.ContinueWith(p => {
                // I put all this text in a StringBuilder and call Console.WriteLine just once
                // because this task could execute concurrently with the task above & I don't
                // want the tasks' output interspersed
                StringBuilder sb = new StringBuilder(
                    "The following exception(s) occurred:" + Environment.NewLine);
                foreach (var e in p.Exception.Flatten().InnerExceptions)
                    sb.AppendLine(" " + e.GetType().ToString());
                Console.WriteLine(sb.ToString());
            }, TaskContinuationOptions.OnlyOnFaulted);
            // Start the parent Task so it can start its children
            parent.Start();

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
