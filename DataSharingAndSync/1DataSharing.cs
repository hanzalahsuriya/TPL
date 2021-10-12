using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TPL.DataSharingAndSync
{
    public class DataSharingAccount
    {
        private object padlock = new object();
        public int Balance{get;set;}

        public void Deposit(int amount)
        {
            lock (padlock)
            {
                // it's not thread safe as output won't be consistent so make it thread safe we add a lock
                Balance += amount;
            } 
        }

        public void Withdraw(int amount)
        {
            lock (padlock)
            {
                Balance -= amount;
            }            
        }
    }

    public class DataSharingSyncUsingLock
    {
        public static void Main1()
        {
            var account = new DataSharingAccount();
            var tasks = new List<Task>();
            for(var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => 
                {
                    for(var j = 0; j < 1000; j++)
                    {
                        account.Deposit(1000);
                    }
                    
                }));

                tasks.Add(Task.Factory.StartNew(() => 
                {
                    for(var j = 0; j < 1000; j++)
                    {
                        account.Withdraw(1000);
                    } 
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"current balance: {account.Balance}");
        }
    }
}