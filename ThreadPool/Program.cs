class Program
{
    public static void Main()
    {
        ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);

        // Получаем максимальное количество потоков
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);

        // Получаем минимальное количество потоков
        ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);

        Console.WriteLine("=== ThreadPool Information ===");
        Console.WriteLine($"Доступно рабочих потоков: {workerThreads}");
        Console.WriteLine($"Доступно I/O потоков: {completionPortThreads}");
        Console.WriteLine($"Максимум рабочих потоков: {maxWorkerThreads}");
        Console.WriteLine($"Максимум I/O потоков: {maxCompletionPortThreads}");
        Console.WriteLine($"Минимум рабочих потоков: {minWorkerThreads}");
        Console.WriteLine($"Минимум I/O потоков: {minCompletionPortThreads}");
    }

}

