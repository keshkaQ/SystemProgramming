using HW4_AccountsJSON;

class Program
{
    static async Task Main()
    {
        var accountManager = new AccountManager("accounts.json");
        var task1 = Task.Run(() => accountManager.UpdateAccountWithMonitor("Alice", 100));
        var task2 = Task.Run(() => accountManager.UpdateAccountWithLock("Mike", 300));
        var task3 = Task.Run(() => accountManager.UpdateAccountWithSemaphore("John", -100));
        var task4 = Task.Run(() => accountManager.UpdateAccountWithMutex("Anna", -150));
        var task5 = Task.Run(() => accountManager.UpdateAccountWithWaitHandle("Bob", -200));

        await Task.WhenAll(task1, task2, task3, task4, task5);
        Console.WriteLine("Обновления завершены.");
    }
}


