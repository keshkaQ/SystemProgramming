class Program
{
    static int Sek = 10;

    static void Main(string[] args)
    {
        TimerCallback timercallback = new TimerCallback(TimerTick);
        Timer timer = new Timer(timercallback);
        timer.Change(1000, 1000); 

        Console.ReadKey(); 
    }

    static void TimerTick(object obj)
    {
        Sek--;
        Console.WriteLine(Sek.ToString());
        if (Sek <= 0)
        {
            Timer a = (Timer)obj;
            a.Dispose();
            Console.WriteLine("Timer End");
        }
    }
}