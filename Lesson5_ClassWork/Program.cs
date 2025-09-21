using System.Collections.Concurrent;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("====== Сумма элементов диапазона ======");
        SumCalculation();

        Console.WriteLine("\n====== Среднее арифметическое с исключением ======");
        AverageCalculation();
    }

    // Сумма элементов от K до L
    static void SumCalculation()
    {
        var nums = Enumerable.Range(0, 1000_000_000).ToArray();
        var min = 100_000_000;
        var max = 990_000_000;

        var watch = Stopwatch.StartNew();
        var withFor = FindSumWithoutParallel(nums, min, max);
        watch.Stop();
        Console.WriteLine($"Обычный цикл: {watch.ElapsedMilliseconds} ms");

        watch = Stopwatch.StartNew();
        var withParallelFor = FindSumWithParallelFor(nums, min, max);
        watch.Stop();
        Console.WriteLine($"Parallel.For: {watch.ElapsedMilliseconds} ms");
    }

    // Среднее арифметическое с исключением диапазона
    static void AverageCalculation()
    {
        int[] array = Enumerable.Range(1, 2_000_000_000).ToArray();
        var startValue = 233_212_245;
        var endValue = 1_467_346_892;

        var watch = Stopwatch.StartNew();
        var withFor = AvgWithoutParallel(array, startValue, endValue);
        watch.Stop();
        Console.WriteLine($"Обычный цикл: {watch.ElapsedMilliseconds} ms");

        watch = Stopwatch.StartNew();
        var withParallel = AvgWithParallel(array, startValue, endValue);
        watch.Stop();
        Console.WriteLine($"Parallel.ForEach: {watch.ElapsedMilliseconds} ms");
    }

    // Методы для первого задания
    static long FindSumWithoutParallel(int[] nums, int min, int max)
    {
        long sum = 0;
        for (int i = min; i < max; i++)
        {
            sum += nums[i];
        }
        return sum;
    }

    static long FindSumWithParallelFor(int[] nums, int min, int max)
    {
        long totalSum = 0;

        Parallel.For(min, max,
            () => 0L,
            (i, loopState, localSum) =>
            {
                return localSum + nums[i];
            },
            localSum =>
            {
                Interlocked.Add(ref totalSum, localSum);
            });

        return totalSum;
    }

    // Методы для второго задания
    static double AvgWithParallel(int[] array, int startValue, int endValue)
    {
        long totalSum = 0;
        long totalCount = 0;

        var range = Partitioner.Create(0, array.Length);
        Parallel.ForEach(range,
            () => (Sum: 0L, Count: 0L),
            (rangePartition, loopState, local) =>
            {
                for (int i = rangePartition.Item1; i < rangePartition.Item2; i++)
                {
                    if (i < startValue || i > endValue)
                    {
                        local.Sum += array[i];
                        local.Count++;
                    }
                }
                return local;
            },
            local =>
            {
                Interlocked.Add(ref totalSum, local.Sum);
                Interlocked.Add(ref totalCount, local.Count);
            });

        return totalSum / (double)totalCount;
    }

    static double AvgWithoutParallel(int[] array, int startValue, int endValue)
    {
        long totalSum = 0;
        long count = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (i < startValue || i > endValue)
            {
                totalSum += array[i];
                count++;
            }
        }
        return totalSum / (double)count;
    }
}