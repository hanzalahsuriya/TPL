using System;
using System.Threading.Tasks;

namespace TPL.Tasks
{
    public class ExceptionHandling
    {
        public static void Main1()
        {
            ObservingAndGettingException();
        }

        public static void ObservingAndGettingException()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                throw new InvalidOperationException("invalid argument exception");
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                throw new AccessViolationException("invalid value exception");
            });

            try
            {
                // now as soon as we do WaitAll, we are observing exception and it will thrown and caught
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException err)
            {
                foreach (var e in err.InnerExceptions)
                {
                    Console.WriteLine($"{e.GetType()} - {e.Message}");
                }

                // only handle specific exception i.e. in this case AccessViolationException but other exception will propagate in stack
                err.Handle((e) => 
                {
                    if(e is AccessViolationException)
                    {
                        // do something...
                        return true;
                    }
                    return false;
                });
            }
        }

        // exceptions are ignored in this case
        public static void ThrowingExceptionInsideTask()
        {
            // exception are not even captured
            var t1 = Task.Factory.StartNew(() =>
            {
                throw new System.Exception("invalid argument exception");
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                throw new System.Exception("invalid value exception");
            });
        }
    }
}