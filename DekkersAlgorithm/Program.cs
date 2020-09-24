using System;
using System.Threading;
using System.Threading.Tasks;

namespace DekkersAlgorithm
{
  class Program
  {
    public static int Data = 0;
    public static bool[] RequestSection = new bool[] { false, false };
    public static int TaskId = 0;

    static void Main(string[] args)
    {
      Random rand = new Random();
      ThreadPool.SetMinThreads(2, 2);

      var task1 = Task.Run(async () =>
      {
        RequestSection[0] = true;
        while (RequestSection[1])
        {
          if (TaskId != 0)
          {
            RequestSection[0] = false;
            while (TaskId != 0)
            {
              await Task.Delay(25);
            }
            RequestSection[0] = true;
          }
        }
        int d = Data;
        await Task.Delay(rand.Next(100, 201));
        d++;
        Data = d;
        TaskId = 1;
        RequestSection[0] = false;
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Data {d}");
      });

      var task2 = Task.Run(async () =>
      {
        RequestSection[1] = true;
        while (RequestSection[0])
        {
          if (TaskId != 1)
          {
            RequestSection[1] = false;
            while (TaskId != 1)
            {
              await Task.Delay(25);
            }
            RequestSection[1] = true;
          }
        }
        await Task.Delay(rand.Next(100, 201));
        int d = Data;
        d++;
        Data = d;
        TaskId = 0;
        RequestSection[1] = false;
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Data {d}");
      });

      Task.WaitAll(task1, task2);
    }
  }
}
