//class Program
//{
//    static void Main(string[] args)
//    {

//        ThreadStart threadStart = new ThreadStart(Method);
//        Thread thread = new Thread(threadStart);
//        thread.Start();

//        Thread ThisThread = Thread.CurrentThread;
//        Console.WriteLine($"main: {ThisThread.GetHashCode()}");
//    }

//    static void Method()
//    {
//        Thread ThisThread = Thread.CurrentThread;
//        Console.WriteLine($"method: {ThisThread.GetHashCode()}");
//        //for (int i = 0; i < 1000; i++)
//        //{
//        //    Console.WriteLine("\t\t\tHello in thread");
//        //}
//    }
//}


// Foreground поток - приложение будет работать 10 секунд, даже после завершения Main
//class Program
//{
//    static void Main()
//    {
//        // Foreground поток по умолчанию
//        Thread foregroundThread = new Thread(WorkForever);
//        foregroundThread.Start();

//        Console.WriteLine("Main завершен, но приложение работает...");
//        Console.WriteLine("Закройте консоль чтобы увидеть разницу");
//    }

//    static void WorkForever()
//    {
//        for (int i = 0; i < 10; i++)
//        {
//            Console.WriteLine($"Foreground: {i}");
//            Thread.Sleep(1000);
//        }
//    }
//}

//  Background поток - приложение закроется сразу после Main, поток не успеет завершиться!
class Program
{
    static void Main()
    {
        // Background поток
        Thread backgroundThread = new Thread(WorkForever);
        backgroundThread.IsBackground = true; // ⭐ Вот это важно!
        backgroundThread.Start();

        Console.WriteLine("Main завершен, приложение закроется сразу");
    }

    static void WorkForever()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Background: {i}");
            Thread.Sleep(1000);
        }
    }
}
