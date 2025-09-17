class Program
{
    static void Main(string[] args)
    {
        ThreadStart TS = new ThreadStart(Method);
        Thread T = new Thread(TS);
        Console.WriteLine("Сейчас будет запущен 1-й поток");
        T.Start();
        Thread.Sleep(200);
        Console.WriteLine("Ожидание завершения работы 2-го потока");
        T.Join();
        Console.WriteLine("Завершение работы 1-го потока");

        Console.ReadKey();
    }
    static void Method()
    {
        Console.WriteLine("2-й поток работает");
        Thread.Sleep(2000);
        Console.WriteLine("2-й поток завершил работу");
    }
}
