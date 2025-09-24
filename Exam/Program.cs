////  Найти кол-во чисел больших среднего значения

//// C# использование последовательных и параллельных методов
//// и измерить производительность с помощью BenchmarkDotNet в консольном приложении

//// обычный массив Array, последовательные циклы (без потоков)
//// Parallel.For (each) (без потоков)
//// PLINQ (asParallel)
//// Task.Factory по кол-ву ядер (<>)

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Concurrent;
using System.Numerics;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<CountAboveAverage>();
    }
}

public class CountAboveAverage
{
    private int[] array;
    private List<int> list;
    private readonly int maxValue = 25000; 

    [Params(1_000_000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        list = [.. Enumerable.Range(0, Size).Select(x => random.Next(maxValue))];
        array = list.ToArray();
    }

    [Benchmark]
    public int Parallel_ConcurrentBag()
    {
        double avg = array.AsParallel().Average();
        var bag = new ConcurrentBag<int>();
        Parallel.ForEach(array, item =>
        {
            if (item > avg)
                bag.Add(item);
        });

        return bag.Count;
    }

    [Benchmark]
    public int List_Foreach()
    {
        long total = 0;
        int count = 0;
        foreach (var item in list)
            total += item;
        double avg = (double)total / list.Count;
        foreach (var item in list)
            if (item > avg)
                count++;
        return count;
    }

    [Benchmark]
    public int Array_LINQ()
    {
        double avg = array.Average();
        return array.Count(x => x > avg);
    }

    [Benchmark]
    public int List_LINQ()
    {
        double avg = list.Average();
        return list.Count(x => x > avg);
    }

    [Benchmark]
    public int Parallel_For_Local()
    {
        long totalSum = 0;
        Parallel.For(0, array.Length,
            () => 0L,
            (i, loopstate, localSum) =>
            {
                localSum += array[i];
                return localSum;
            },
            localSum => Interlocked.Add(ref totalSum, localSum));

        double avg = (double)(totalSum / array.Length);

        int totalCount = 0;
        Parallel.For(0, array.Length,
           () => 0,
           (i, loopstate, localCount) =>
           {
               if (array[i] > avg)
                   localCount++;
               return (int)localCount;
           },
           localCount => Interlocked.Add(ref totalCount, localCount));

        return totalCount;
    }

    [Benchmark]
    public int PLINQ_AutoParallel()
    {
        double avg = array.AsParallel().Average();
        return array.AsParallel().Count(x => x > avg);
    }

    [Benchmark]
    public int PLINQ_WithDegreeOfParallelism()
    {
        double avg = array.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).Average();
        return array.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).Count(x => x > avg);
    }

    [Benchmark]
    public int PLINQ_ForceParallel()
    {
        double avg = array.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Average();
        return array.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Count(x => x > avg);
    }

    [Benchmark]
    public int Array_For()
    {
        long sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }

        double avg = (double)sum / array.Length;

        int countAboveAverage = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > avg)
            {
                countAboveAverage++;
            }
        }
        return countAboveAverage;
    }
    [Benchmark]
    public int List_For()
    {
        long sum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }

        double avg = (double)sum / list.Count;

        int countAboveAverage = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] > avg)
            {
                countAboveAverage++;
            }
        }
        return countAboveAverage;
    }
    [Benchmark]
    public int Parallel_ForEach_Partitioner()
    {
        long totalSum = 0;
        Parallel.ForEach(Partitioner.Create(0, array.Length),
            () => 0L,
            (range, state, localSum) =>
            {
                // range - Tuple<int, int> (начало, конец диапазона)
                // state - информация о состоянии цикла (можно прервать)
                // localSum - локальная сумма потока

                for (int i = range.Item1; i < range.Item2; i++)
                    localSum += array[i];
                return localSum;
            },
            // localSum - финальная сумма из одного потока
            localSum => Interlocked.Add(ref totalSum, localSum)); // Потокобезопасное сложение

        double avg = (double)totalSum / array.Length;

        int totalCount = 0;
        Parallel.ForEach(Partitioner.Create(0, array.Length),
            () => 0,
            (range, state, localCount) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    if (array[i] > avg) localCount++;
                return localCount;
            },
            localCount => Interlocked.Add(ref totalCount, localCount));

        return totalCount;
    }

    [Benchmark]
    public int Tasks_Run()
    {
        int processorCount = Environment.ProcessorCount;
        int chunkSize = array.Length / processorCount;

        var sumTasks = new Task<long>[processorCount];
        for (int i = 0; i < processorCount; i++)
        {
            int start = i * chunkSize;
            int end = (i == processorCount - 1) ? array.Length : start + chunkSize;

            sumTasks[i] = Task.Run(() =>
            {
                long localSum = 0;
                for (int j = start; j < end; j++)
                    localSum += array[j];
                return localSum;
            });
        }

        long totalSum = Task.WhenAll(sumTasks).Result.Sum();
        double avg = (double)totalSum / array.Length;

        var countTasks = new Task<int>[processorCount];
        for (int i = 0; i < processorCount; i++)
        {
            int start = i * chunkSize;
            int end = (i == processorCount - 1) ? array.Length : start + chunkSize;

            countTasks[i] = Task.Run(() =>
            {
                int localCount = 0;
                for (int j = start; j < end; j++)
                    if (array[j] > avg) localCount++;
                return localCount;
            });
        }

        return Task.WhenAll(countTasks).Result.Sum();
    }
    [Benchmark]
    public int Tasks_Factory()
    {
        int processorCount = Environment.ProcessorCount;
        int chunkSize = array.Length / processorCount;

        var sumTasks = new Task<long>[processorCount];
        for (int i = 0; i < processorCount; i++)
        {
            int start = i * chunkSize;
            int end = (i == processorCount - 1) ? array.Length : start + chunkSize;

            sumTasks[i] = Task.Factory.StartNew(() =>
            {
                long localSum = 0;
                for (int j = start; j < end; j++)
                    localSum += array[j];
                return localSum;
            });
        }

        long totalSum = Task.WhenAll(sumTasks).Result.Sum();
        double avg = (double)totalSum / array.Length;

        var countTasks = new Task<int>[processorCount];
        for (int i = 0; i < processorCount; i++)
        {
            int start = i * chunkSize;
            int end = (i == processorCount - 1) ? array.Length : start + chunkSize;

            countTasks[i] = Task.Factory.StartNew(() =>
            {
                int localCount = 0;
                for (int j = start; j < end; j++)
                    if (array[j] > avg) localCount++;
                return localCount;
            });
        }

        return Task.WhenAll(countTasks).Result.Sum();
    }
}
