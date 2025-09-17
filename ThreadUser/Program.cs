class Program
{
    static void Main()
    {
        ThreadStart lis = new ThreadStart(LisenerClient);
        Thread LisenerThread = new Thread(lis);

        LisenerThread.IsBackground = false;
        LisenerThread.Start();

        Console.WriteLine("Сервер запущен. Для выхода нажмите ctrl + c");
        Console.ReadLine();
    }
    static void LisenerClient()
    {
        int counter = 0;
        while (true)
        {
            Console.WriteLine("\n>>> Введите 'connect' и нажмите Enter, чтобы подключить нового пользователя (или 'exit' для выхода):");
            string input = Console.ReadLine(); // Ждём именно строку
            ParameterizedThreadStart UserDel = new ParameterizedThreadStart(UserThreadFunk);
            Thread UserWorkThread = new Thread (UserDel);
            UserWorkThread.Start(counter.ToString());
            counter++;
        }
    }
    static void UserThreadFunk(object a)
    {
        string username = (string)a;
        Console.WriteLine($"\n>>> Пользователь #{username} подключился <<<");
        while (true)
        {
            switch(int.Parse(Console.ReadLine()))
            {
                case 0:
                    Console.WriteLine("# {0} подписался на новости", username);
                    break;
                case 1:
                    Console.WriteLine("# {0} начал чат ", username);
                    break;
                case 2:
                    Console.WriteLine("# {0} купил продукцию в магазине", username);
                    break;
                case 3:
                    Console.WriteLine("# {0} отправил письмо", username);
                    break;
            }
        }
    }
}
