using System.Diagnostics;
Console.Title = "Список процессов";
Process[] processes = Process.GetProcesses();
Console.WriteLine(" {0,-40}{1,-10}", "Имя.процесса: ", "PID: ");
foreach (Process p in processes)
    Console.WriteLine(" {0,-40}{1,-10}", p.ProcessName, p.Id);

