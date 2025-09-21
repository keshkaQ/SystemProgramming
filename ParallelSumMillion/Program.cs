using System.Diagnostics;
class Program
{
    static void Main()
    {
        int[] nums = Enumerable.Range(0, 100_000_000).ToArray();
        long total = 0;

        var watch = Stopwatch.StartNew();
        foreach (var x in nums)
        {
            total += x;
        }
        watch.Stop();
        Console.WriteLine($"Обычный цикл {watch.ElapsedMilliseconds} ms");

        total = 0;
        watch = Stopwatch.StartNew();

        // First type parameter is the type of the source elements
        // Second type parameter is the type of the thread-local variable (partition subtotal)
        Parallel.ForEach<int, long>(
            nums, // source collection
            () => 0, // method to initialize the local variable
            (j, loop, subtotal) => // method invoked by the loop on each iteration
            {
                subtotal += j; //modify local variable
                return subtotal; // value to be passed to next iteration
            },
            // Method to be executed when each partition has completed.
            // finalResult is the final value of subtotal for a particular partition.
            (finalResult) => Interlocked.Add(ref total, finalResult));

        watch.Stop();
        Console.WriteLine($"Параллельный цикл {watch.ElapsedMilliseconds} ms");

        Console.WriteLine($"The total from Parallel.ForEach is {total:N0}");
    }
}
