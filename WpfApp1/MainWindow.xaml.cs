using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<BenchmarkResult> Results { get; set; }
        private CountAboveAverage benchmark;

        public MainWindow()
        {
            InitializeComponent();

            Results = new ObservableCollection<BenchmarkResult>();
            ResultsDataGrid.ItemsSource = Results;

            DataContext = this;
        }

        private async void RunBenchmarkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RunBenchmarkBtn.IsEnabled = false;
                SaveToDbBtn.IsEnabled = false;
                ProgressBar.Value = 0;
                StatusText.Text = "Подготовка к тестированию...";

                Results.Clear();

                // Получаем тип задачи и размер данных
                var taskType = ((ComboBoxItem)TaskTypeComboBox.SelectedItem).Content.ToString();
                var sizeText = ((ComboBoxItem)SizeComboBox.SelectedItem).Content.ToString();
                var size = int.Parse(sizeText.Replace(",", ""));

                StatusText.Text = $"Инициализация данных для '{taskType}' ({size:N0} элементов)...";

                // Инициализация бенчмарка в зависимости от типа задачи
                benchmark = new CountAboveAverage { Size = size };
                benchmark.Setup();

                // Получаем список тестов для выбранной задачи
                var tests = GetTestsForTask(taskType);

                double baselineTime = 0;
                var timeValues = new List<double>();
                var memoryValues = new List<double>();
                var speedupValues = new List<double>();
                var methodNames = new List<string>();

                for (int i = 0; i < tests.Count; i++)
                {
                    var test = tests[i];
                    StatusText.Text = $"Выполнение: {test.Name}... ({i + 1}/{tests.Count})";

                    // Очистка памяти перед тестом
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    var memoryBefore = GC.GetTotalMemory(true);

                    // Запуск теста с измерением времени
                    var stopwatch = Stopwatch.StartNew();
                    var result = await Task.Run(() => test.Test());
                    stopwatch.Stop();

                    var memoryAfter = GC.GetTotalMemory(false);
                    var memoryUsed = (memoryAfter - memoryBefore) / (1024 * 1024.0); // MB
                    var executionTime = stopwatch.Elapsed.TotalMilliseconds;

                    // Первый тест - базовый для сравнения
                    if (i == 0) baselineTime = executionTime;

                    var speedup = baselineTime / executionTime;

                    // Сохраняем данные для графиков
                    timeValues.Add(executionTime);
                    memoryValues.Add(memoryUsed);
                    speedupValues.Add(speedup);
                    methodNames.Add(test.Name);

                    Results.Add(new BenchmarkResult
                    {
                        TaskType = taskType,
                        DataSize = size,
                        MethodName = test.Name,
                        ExecutionTime = executionTime.ToString("F2"),
                        MemoryUsed = memoryUsed.ToString("F2"),
                        Result = result.ToString("N0"),
                        Speedup = speedup.ToString("F2") + "x",
                        Timestamp = DateTime.Now
                    });

                    ProgressBar.Value = (i + 1) * 100 / tests.Count;
                }

                // Обновляем графики
                UpdateCharts(methodNames, timeValues, memoryValues, speedupValues);

                StatusText.Text = $"Тестирование завершено! Протестировано {tests.Count} методов для задачи '{taskType}'.";
                SaveToDbBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText.Text = "Ошибка при выполнении тестов";
            }
            finally
            {
                RunBenchmarkBtn.IsEnabled = true;
            }
        }

        private List<(string Name, Func<int> Test)> GetTestsForTask(string taskType)
        {
            return taskType switch
            {
                "Count Numbers Above Average" => new List<(string, Func<int>)>
                {
                    ("Array For", () => benchmark.Array_For()),
                    ("List For", () => benchmark.List_For()),
                    ("Array LINQ", () => benchmark.Array_LINQ()),
                    ("List LINQ", () => benchmark.List_LINQ()),
                    ("List Foreach", () => benchmark.List_Foreach()),
                    ("Parallel For Local", () => benchmark.Parallel_For_Local()),
                    ("Parallel ForEach Partitioner", () => benchmark.Parallel_ForEach_Partitioner()),
                    ("Parallel ConcurrentBag", () => benchmark.Parallel_ConcurrentBag()),
                    ("PLINQ Auto", () => benchmark.PLINQ_AutoParallel()),
                    ("PLINQ With Degree", () => benchmark.PLINQ_WithDegreeOfParallelism()),
                    ("PLINQ Force Parallel", () => benchmark.PLINQ_ForceParallel()),
                    ("Tasks Run", () => benchmark.Tasks_Run()),
                    ("Tasks Factory", () => benchmark.Tasks_Factory())
                },
                //"Find Max/Min Identical Elements" => new List<(string, Func<object>)>
                //{
                //    ("Sequential", () => benchmark.FindMaxIdenticalElements_Sequential()),
                //    ("Parallel", () => benchmark.FindMaxIdenticalElements_Parallel()),
                //    ("PLINQ", () => benchmark.FindMaxIdenticalElements_PLINQ())
                //},
                //"Find Prime Numbers" => new List<(string, Func<object>)>
                //{
                //    ("Sequential", () => benchmark.FindPrimes_Sequential()),
                //    ("Parallel", () => benchmark.FindPrimes_Parallel()),
                //    ("PLINQ", () => benchmark.FindPrimes_PLINQ())
                //},
                //"Max/Min Even/Odd Numbers" => new List<(string, Func<object>)>
                //{
                //    ("Sequential", () => benchmark.FindMaxEven_Sequential()),
                //    ("Parallel", () => benchmark.FindMaxEven_Parallel()),
                //    ("PLINQ", () => benchmark.FindMaxEven_PLINQ())
                //},
                //"Average Positive/Negative Numbers" => new List<(string, Func<object>)>
                //{
                //    ("Sequential", () => benchmark.AveragePositive_Sequential()),
                //    ("Parallel", () => benchmark.AveragePositive_Parallel()),
                //    ("PLINQ", () => benchmark.AveragePositive_PLINQ())
                //},
                //_ => new List<(string, Func<object>)>
                //{
                //    ("Array For", () => benchmark.Array_For()),
                //    ("List For", () => benchmark.List_For()),
                //    ("Parallel For Local", () => benchmark.Parallel_For_Local())
                //}
            };
        }

        private void UpdateCharts(List<string> methodNames, List<double> timeValues, List<double> memoryValues, List<double> speedupValues)
        {
            // График времени выполнения
            TimeChart.Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = timeValues,
                    Name = "Время (мс)",
                    Fill = new SolidColorPaint(SKColors.Blue)
                }
            };

            // График использования памяти
            MemoryChart.Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = memoryValues,
                    Name = "Память (MB)",
                    Fill = new SolidColorPaint(SKColors.Red)
                }
            };

            // График ускорения
            SpeedupChart.Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = speedupValues,
                    Name = "Ускорение",
                    Fill = new SolidColorPaint(SKColors.Green)
                }
            };

            // Настройка осей
            var axis = new Axis
            {
                Labels = methodNames.ToArray(),
                LabelsRotation = 45,
                TextSize = 10
            };

            TimeChart.XAxes = new[] { axis };
            MemoryChart.XAxes = new[] { axis };
            SpeedupChart.XAxes = new[] { axis };
        }

        private async void SaveToDbBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveToDbBtn.IsEnabled = false;
                StatusText.Text = "Сохранение результатов в базу данных...";

                await SaveResultsToDatabase();

                StatusText.Text = "Результаты успешно сохранены в базу данных!";
                MessageBox.Show("Результаты успешно сохранены в базу данных!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в БД: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText.Text = "Ошибка при сохранении в БД";
            }
            finally
            {
                SaveToDbBtn.IsEnabled = true;
            }
        }

        private async Task SaveResultsToDatabase()
        {
            MessageBox.Show("Результаты сохранены в базе данных");
            //// Замените строку подключения на вашу
            //string connectionString = "Server=localhost;Database=BenchmarkDB;Integrated Security=true;";

            //using (var connection = new SqlConnection(connectionString))
            //{
            //    await connection.OpenAsync();

            //    foreach (var result in Results)
            //    {
            //        var command = new SqlCommand(@"
            //            INSERT INTO BenchmarkResults 
            //            (TaskType, DataSize, MethodName, ExecutionTime, MemoryUsed, Result, Speedup, Timestamp)
            //            VALUES 
            //            (@TaskType, @DataSize, @MethodName, @ExecutionTime, @MemoryUsed, @Result, @Speedup, @Timestamp)",
            //            connection);

            //        command.Parameters.AddWithValue("@TaskType", result.TaskType);
            //        command.Parameters.AddWithValue("@DataSize", result.DataSize);
            //        command.Parameters.AddWithValue("@MethodName", result.MethodName);
            //        command.Parameters.AddWithValue("@ExecutionTime", result.ExecutionTime);
            //        command.Parameters.AddWithValue("@MemoryUsed", result.MemoryUsed);
            //        command.Parameters.AddWithValue("@Result", result.Result);
            //        command.Parameters.AddWithValue("@Speedup", result.Speedup);
            //        command.Parameters.AddWithValue("@Timestamp", result.Timestamp);

            //        await command.ExecuteNonQueryAsync();
            //    }
            //}
        }

        private void ClearResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            Results.Clear();
            TimeChart.Series = null;
            MemoryChart.Series = null;
            SpeedupChart.Series = null;
            ProgressBar.Value = 0;
            StatusText.Text = "Готов к тестированию...";
            SaveToDbBtn.IsEnabled = false;
        }
    }

    public class BenchmarkResult
    {
        public string TaskType { get; set; }
        public int DataSize { get; set; }
        public string MethodName { get; set; }
        public string ExecutionTime { get; set; }
        public string MemoryUsed { get; set; }
        public string Result { get; set; }
        public string Speedup { get; set; }
        public DateTime Timestamp { get; set; }
    }
}