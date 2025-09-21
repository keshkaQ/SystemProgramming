class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Синхронизация мьютексом:");
        Counter c = new Counter();

        Thread[] threads = new Thread[5];
        for (int i = 0; i < threads.Length; ++i)
        {
            threads[i] = new Thread(c.UpdateFields);
            threads[i].Start();
        }

        for (int i = 0; i < threads.Length; ++i)
            threads[i].Join();

        Console.WriteLine("Count: {0}\n\n", c.Count);

    }

    class Counter
    {
        int count;
        Mutex m = new Mutex(false, "SYNC_MUTEX");

        public int Count
        {
            get { return count; }
        }

        public void UpdateFields()
        {
            for (int i = 0; i < 100; ++i)
            {
                m.WaitOne();
                ++count;
                m.ReleaseMutex();
            }
        }
    }

}