using System.Collections.Generic;
using System.Threading;

/*
 * This is by no means a safe solution to my problems, oh well.
 */
class GThreadPool
{
    public static int MAX_CONCURRENT_THREADS = System.Environment.ProcessorCount;
    public static bool ShutdownRequested = false;

    private static Queue<Thread> ThreadQueue = new Queue<Thread>();
    private static List<Thread> ActiveThreads = new List<Thread>();

    public static void Poll()
    {
        List<Thread> finishedThreads = new List<Thread>();
        foreach (Thread thread in ActiveThreads)
        {
            if (!thread.IsAlive)
            {
                finishedThreads.Add(thread);
            }
        }
        finishedThreads.ForEach(t => ActiveThreads.Remove(t));

        if (ThreadQueue.Count > 0 && ActiveThreads.Count < MAX_CONCURRENT_THREADS)
        {
            Thread nextInLine = ThreadQueue.Dequeue();
            Enqueue(nextInLine, true);
        }
    }

    public static void Clear()
    {
        ShutdownRequested = true;
        ThreadQueue.Clear();

        foreach (Thread thread in ActiveThreads)
        {
            thread.Join(1);
        }

        // Assume all threads are terminated by this point.
        ActiveThreads.Clear();
        ShutdownRequested = false;
    }

    public static void Enqueue(Thread thread, bool runImmediately = false)
    {
        if (runImmediately)
        {
            thread.Start();
            ActiveThreads.Add(thread);
        }
        else
        {
            ThreadQueue.Enqueue(thread);
        }
    }
}