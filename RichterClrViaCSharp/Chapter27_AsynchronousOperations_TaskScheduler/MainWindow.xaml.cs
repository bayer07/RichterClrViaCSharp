using System.Windows;
using System.Windows.Input;

namespace Chapter27_AsynchronousOperations_TaskScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskScheduler m_syncContextTaskScheduler;

        public MainWindow()
        {
            // Get a reference to a synchronization context task scheduler
            m_syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Title = "Synchronization Context Task Scheduler Demo";
            Visibility = Visibility.Visible; Width = 600; Height = 100;
            InitializeComponent();
        }

        private CancellationTokenSource m_cts;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (m_cts != null)
            { // An operation is in flight, cancel it
                m_cts.Cancel();
                m_cts = null;
            }
            else
            {
                // An operation is not in flight, start it
                Title = "Operation running";
                m_cts = new CancellationTokenSource();
                // This task uses the default task scheduler and executes on a thread pool thread
                Task<Int32> t = Task.Run(() => Sum(m_cts.Token, 20000), m_cts.Token);
                // These tasks use the sync context task scheduler and execute on the GUI thread
                t.ContinueWith(task => Title = "Result: " + task.Result,
                    CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion,
                    m_syncContextTaskScheduler);
                t.ContinueWith(task => Title = "Operation canceled",
                    CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled,
                    m_syncContextTaskScheduler);
                t.ContinueWith(task => Title = "Operation faulted",
                    CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted,
                    m_syncContextTaskScheduler);
            }
            base.OnMouseDown(e);
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