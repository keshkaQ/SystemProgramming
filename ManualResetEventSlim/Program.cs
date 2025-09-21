class Program
{
    public static void Main()
    {
        var myEvent = new ManualResetEventSlim(false);
        var threadWeWaitFor = new Thread(() =>
        {
            Console.WriteLine("Делаем что-то");
            Thread.Sleep(5000);
            Console.WriteLine("Выполнено");
            myEvent.Set();
        });
        var waitingThread = new Thread(() =>
        {
            Console.WriteLine("Ожидаем другой поток, чтобы что-то сделать");
            myEvent.Wait();
            Console.WriteLine("Другой поток закончил выполнение, продолжаем");
        });
        threadWeWaitFor.Start();
        waitingThread.Start();
    }
}