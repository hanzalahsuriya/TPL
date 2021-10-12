using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TPL.Extensions;

namespace TPL.DataSharingAndSync
{
    public class ReadWriteLockExampleData
    {
        public int Count => data.Count;
        private ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();
        private Dictionary<int, string> data = new Dictionary<int, string>(); 

        public string Read(int key)
        {
            readWriteLock.EnterReadLock();
            try
            {
                return data[key];
            }
            finally
            {
                readWriteLock.ExitReadLock();
            }
        }

        public void Add(int key, string value)
        {
            readWriteLock.EnterWriteLock();
            try
            {
                data.Add(key, value);                
            }
            finally
            {
                readWriteLock.ExitWriteLock();
            }
        }

        public bool AddWithTimeout(int key, string value, int timeout)
        {
            var locked = readWriteLock.TryEnterWriteLock(timeout);
            try
            {
                data.Add(key, value);                
            }
            finally
            {
                if(locked) readWriteLock.ExitWriteLock();
            }
            return locked;
        }

        public void AddOrUpdate(int key, string value)
        {
            readWriteLock.EnterUpgradeableReadLock();
            try
            {
                if(data.ContainsKey(key))
                {
                    var currentValue = data[key];
                    if(currentValue == value)
                    {
                        return;
                    }
                    else
                    {
                        readWriteLock.EnterWriteLock();
                        try
                        {
                            data[key] = value;
                        }
                        finally
                        {
                            readWriteLock.ExitWriteLock();
                        }
                    }
                }
                else
                {
                    readWriteLock.EnterWriteLock();
                    try
                    {
                        data.Add(key, value);
                    }
                    finally
                    {
                        readWriteLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                readWriteLock.ExitUpgradeableReadLock();
            }
        }

        ~ReadWriteLockExampleData()
        {
            if(readWriteLock != null) readWriteLock.Dispose();
        }
    }

    public class ReaderWriterExamplle1
    {
        public static void Main1()
        {
            ReadWriteLockExampleData data = new ReadWriteLockExampleData();
            
            for(var j = 0; j < 5; j++)
            {
                Task.Factory.StartNew(() => {
                    Console.WriteLine($"task {Task.CurrentId} is about to add");
                    data.Add(j, StringExtentions.GenerateRandom(10));
                    Console.WriteLine($"task {Task.CurrentId} added successfully");
                });
            }
            
            for(var i = 0; i < 5; i++)
            {
                Task.Factory.StartNew(() => 
                {
                    Console.WriteLine($"task {Task.CurrentId} is about to read a value");
                    var value = data.Read(i);
                    Console.WriteLine($"task {Task.CurrentId} read successfully with value {value}");
                });
            }
        }
    }
}