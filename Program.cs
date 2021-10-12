using System;
using TPL.Tasks;
using TPL.DataSharingAndSync;
namespace TPL
{
    class Program
    {
        static void Main(string[] args)
        {
            // CreateStartingTask.Main1();
            // CancellingTask.Main1();
            // WaitingTimeToPass.Main1();
            // WaitingForTasks.Main1();
            //ExceptionHandling.Main1();
            
            
            // DataSharingSyncUsingLock.Main1();    // using padlock lock
            // InterlockDataSharingSyncUsingLock.Main1();   // using interlock
            // SpinlockDataSharingSyncUsingLock.Main1();
            // MutexlockDataSharingSyncUsingLock.Main1();
            // ReaderWriterLock.Main1();
            ReaderWriterExamplle1.Main1();

            Console.WriteLine("Hello World!");
        }
    }
}
