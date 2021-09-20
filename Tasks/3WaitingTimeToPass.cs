using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL.Tasks
{
    public class WaitingTimeToPass
    {
        public static void Main1()
        {
            //    SleepVsSpin();
            WaitingOnCancellation();
        }

        public static void SleepVsSpin()
        {
            var t = new Task(() => 
            {   
                // pauses the current execution of the task and also let scheduler pick another task i.e. schedular does the context switching
                Thread.Sleep(1000);

                // put the processor in a tight loop defined by number of iteration i.e. no context switching. good for locks
                // you don't give up your place in execution tasks
                Thread.SpinWait(10);
                // SpinWait.SpinUntil() // conditionally

            });
            t.Start();
        }

        public static void WaitingOnCancellation()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = new Task(() => 
            {
                // this will wait for the task to be cancelled
                var cancelled = token.WaitHandle.WaitOne(1000);
                Console.WriteLine(cancelled ? "You win" : "You lose");
            }, token);
            task.Start();
            Console.ReadKey();
            cts.Cancel();
        }

        public static void WaitingForTaskToBeCancelled()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = new Task(() => 
            {
                // this will wait for the task to be cancelled
                token.WaitHandle.WaitOne();
            }, token);
            task.Start();
            Console.ReadKey();
            cts.Cancel();
        }
    }
}