using Newtonsoft.Json;

namespace HW4_AccountsJSON
{
    public class AccountManager
    {
        private readonly string _filePath;
        private readonly object _lockObject = new object(); 
        private readonly Mutex _mutex = new Mutex(); 
        private readonly Semaphore _semaphore = new Semaphore(1, 1);
        private readonly object _fileLock = new object();

        public AccountManager(string filePath)
        {
            _filePath = filePath;
        }
        public void UpdateAccountWithLock(string clientName, decimal amount)
        {
            lock (_lockObject)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Выполняем операцию через lock");
                UpdateAccountInternal(clientName, amount);
            }
        }
        public void UpdateAccountWithMonitor(string clientName, decimal amount)
        {
            Monitor.Enter(_lockObject);
            try
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Выполняем операцию через Monitor");
                UpdateAccountInternal(clientName, amount);
            }
            finally
            {
                Monitor.Exit(_lockObject);
            }
        }
        public void UpdateAccountWithMutex(string clientName, decimal amount)
        {
            _mutex.WaitOne();
            try
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Выполняем операцию через Mutex");
                UpdateAccountInternal(clientName, amount);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
        public void UpdateAccountWithSemaphore(string clientName, decimal amount)
        {
            _semaphore.WaitOne();
            try
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Выполняем операцию через Semaphore");
                UpdateAccountInternal(clientName, amount);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void UpdateAccountWithWaitHandle(string clientName, decimal amount)
        {
            using (var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset))
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Ждём сигнал...");
                Task.Delay(10).Wait();
                waitHandle.Set();
                waitHandle.WaitOne();
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Сигнал получен, продолжаем.");
            }
        }
        private void UpdateAccountInternal(string clientName, decimal amount)
        {
            lock (_fileLock) 
            {
                var accounts = LoadAccounts();
                if (accounts.ContainsKey(clientName))
                {
                    accounts[clientName].Balance += amount;
                }
                else
                {
                    accounts[clientName] = new ClientAccount { Name = clientName, Balance = amount };
                }
                SaveAccounts(accounts);
            }
        }

        private Dictionary<string, ClientAccount> LoadAccounts()
        {
            if (!File.Exists(_filePath))
            {
                var initialAccounts = new Dictionary<string, ClientAccount>
                {
                { "Alice", new ClientAccount { Name = "Alice", Balance = 500.00m } },
                { "Mike", new ClientAccount { Name = "Mike", Balance = 500.00m } },
                { "John", new ClientAccount { Name = "John", Balance = 500.00m } },
                { "Anna", new ClientAccount { Name = "Anna", Balance = 500.00m } },
                { "Bob", new ClientAccount { Name = "Bob", Balance = 500.00m } }
                };
                SaveAccounts(initialAccounts);
                return initialAccounts;

            }
            using (var reader = new StreamReader(_filePath, System.Text.Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<string, ClientAccount>>(json) ?? new Dictionary<string, ClientAccount>();
            }
        }

        private void SaveAccounts(Dictionary<string, ClientAccount> accounts)
        {
            var json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
