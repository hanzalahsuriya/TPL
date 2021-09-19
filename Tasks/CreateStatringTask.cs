using System;
using System.Threading.Tasks;

namespace TPL.Tasks
{
    public class CreateStartingTask
    {
        public static void Main1()
        {
            //Creating and Starting Tasks
            CreateStartingTask t1 = new CreateStartingTask();
            
            // one way to create a task is using factory
            Task.Factory.StartNew(() => Work("£££")); // thread 1
            
            // can create a new task using Task Object
            var task = new Task(() => Work("$$$"));
            task.Start();
            
            // passing argument (object type) to Task Factory
            Task.Factory.StartNew(WorkWithObjectParam, "+++");

            // start task by passing argument of type object
            var task1 = new Task(WorkWithObjectParam, "---");
            task1.Start();

            // task via factory to return a value
            Task<int> taskFactoryWithResult = Task.Factory.StartNew<int>(GetLength, "sample text....");
            Console.WriteLine($"string length is {taskFactoryWithResult.Result}");

            // task to return a value
            Task<int> taskWithResult = new Task<int>(GetLength, "sample text");
            taskWithResult.Start();
            Console.WriteLine($"string length is {taskWithResult.Result}");

            Work("Main");          // thread 3 (main thread)    

            Console.ReadKey();
        }

        public static int GetLength(object text)
        {
            return text.ToString().Length;
        }

        public static void WorkWithObjectParam(object o)
        {
            Work(o.ToString());
        }

        public static void Work(string text)
        {
            int i = 1000;
            while(i-- > 0)
            {
                Console.WriteLine($"{i}. {text}");
            }
        }
    }
}