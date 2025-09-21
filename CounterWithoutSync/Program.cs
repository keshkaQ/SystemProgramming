class  Program
{
    static void Main(string[] args)
    {
        Thread[] threads = new Thread[5];
        for (int a = 0; a < 10;a++)
        {
            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(delegate ()
                {
                    for (int j = 1; j <= 1_000_000; ++j)
                        ++Counter.count;
                });
                threads[i].Start();
            }

            for (int i = 0; i < threads.Length; ++i)
                threads[i].Join();

            Console.WriteLine("Без синхронизации");
            Console.WriteLine("counter = {0}", Counter.count);

            // сбрасываю счетчик
            Counter.count = 0;
            threads = new Thread[5];

            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(delegate ()
                {
                    for (int j = 1; j <= 1_000_000; ++j)
                        Interlocked.Increment(ref Counter.count);
                });
                threads[i].Start();
            }

            for (int i = 0; i < threads.Length; ++i)
                threads[i].Join();

            Console.WriteLine("С помощью Interlocked");
            Console.WriteLine("counter = {0}", Counter.count);
            Counter.count = 0;
        }    

    
    }
    class Counter
    {
        public static int count;
    }
}