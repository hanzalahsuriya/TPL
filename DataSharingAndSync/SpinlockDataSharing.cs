using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TPL.DataSharingAndSync
{
    // public class TestException : Exception
    // {
    //     public TestException(List<SpintLockAccount> accounts) : base(message: $"sdfsdfsdfsd sdfsdf {accounts}")
    //     {
            
    //     }
    // }


    public class SpintLockAccount
    {
        private int _balance;
        public int Balance 
        { 
            get => _balance; 
            set { _balance = value; }
        }

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }
    }

    public class SpinlockDataSharingSyncUsingLock
    {
        public static void Main1()
        {
            // var lst = new List<SpintLockAccount>()
            // {
            //     new SpintLockAccount(), new SpintLockAccount(), new SpintLockAccount(), new SpintLockAccount()
            // };

            // throw new TestException(lst);


            var account = new SpintLockAccount();
            var tasks = new List<Task>();
            SpinLock sl = new SpinLock();
            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            account.Deposit(1000);
                        }
                        finally
                        {
                            if(lockTaken) sl.Exit();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            account.Withdraw(1000);
                        }
                        finally
                        {
                            if(lockTaken) sl.Exit();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"current balance: {account.Balance}");
        }
    }
}