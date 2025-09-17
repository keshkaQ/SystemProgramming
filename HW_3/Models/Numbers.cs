using HW_3.RelayCommand;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HW_3.Models
{
    public class Numbers : INotifyPropertyChanged
    {
        // Раздельные потоки и TokenSource для кубов и факториалов
        private Thread cubeThread;
        private Thread factThread;
        private CancellationTokenSource cubeTokenSource;
        private CancellationTokenSource factTokenSource;

        // Раздельные события паузы - примитив синхронизации
        private ManualResetEventSlim cubePauseEvent = new ManualResetEventSlim(true);
        private ManualResetEventSlim factPauseEvent = new ManualResetEventSlim(true);

        // для обновления UI через фоновый поток
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public ICommand StartCubeCommand { get; }
        public ICommand StopCubeCommand { get; }
        public ICommand PauseCubeCommand { get; }
        public ICommand StartFactorialCommand { get; }
        public ICommand StopFactorialCommand { get; }
        public ICommand PauseFactorialCommand { get; }

        private bool isCubePaused;
        public bool IsCubePaused
        {
            get => isCubePaused;
            set
            {
                isCubePaused = value;
                OnPropertyChanged();
            }
        }

        private bool isFactPaused;
        public bool IsFactPaused
        {
            get => isFactPaused;
            set
            {
                isFactPaused = value;
                OnPropertyChanged();
            }
        }

        private int lowerLimit;
        public int LowerLimit
        {
            get => lowerLimit;
            set
            {
                lowerLimit = value;
                OnPropertyChanged();
            }
        }

        private int highLimit;
        public int HighLimit
        {
            get => highLimit;
            set
            {
                highLimit = value;
                OnPropertyChanged();
            }
        }

        private string cubeTxtBlock;
        public string CubeTextBlock
        {
            get => cubeTxtBlock;
            set
            {
                cubeTxtBlock = value;
                OnPropertyChanged();
            }
        }

        private int limitNumber;
        public int LimitNumber
        {
            get => limitNumber;
            set
            {
                limitNumber = value;
                OnPropertyChanged();
            }
        }

        private string factorialTextBlock;
        public string FactorialTextBlock
        {
            get => factorialTextBlock;
            set
            {
                factorialTextBlock = value;
                OnPropertyChanged();
            }
        }

        public Numbers()
        {
            // Команды для кубов
            StartCubeCommand = new DelegateCommand((p) =>
            {
                cubeTokenSource = new CancellationTokenSource();
                cubePauseEvent.Set();
                cubeThread = new Thread(CubeCalculation) { IsBackground = true };
                cubeThread.Start(cubeTokenSource.Token);
            },
            p => cubeThread == null || !cubeThread.IsAlive);

            PauseCubeCommand = new DelegateCommand(p =>
            {
                if (IsCubePaused)
                {
                    cubePauseEvent.Set();
                    IsCubePaused = false;
                }
                else
                {
                    cubePauseEvent.Reset();
                    IsCubePaused = true;
                }
            },
            p => cubeThread != null && cubeThread.IsAlive);

            StopCubeCommand = new DelegateCommand(p =>
            {
                cubeTokenSource?.Cancel();
                cubePauseEvent.Set();
                cubeThread = null;
                cubeTokenSource = null;

                dispatcher.Invoke(() =>
                {
                    LowerLimit = 0;
                    HighLimit = 0;
                    CubeTextBlock = string.Empty;
                    IsCubePaused = false;
                });
            },
            p => cubeThread != null && cubeThread.IsAlive);

            // Команды для факториалов
            StartFactorialCommand = new DelegateCommand((p) =>
            {
                factTokenSource = new CancellationTokenSource();
                factPauseEvent.Set();
                factThread = new Thread(FactCalculation) { IsBackground = true };
                factThread.Start(factTokenSource.Token);
            },
            p => factThread == null || !factThread.IsAlive);

            PauseFactorialCommand = new DelegateCommand(p =>
            {
                if (IsFactPaused)
                {
                    factPauseEvent.Set();
                    IsFactPaused = false;
                }
                else
                {
                    factPauseEvent.Reset();
                    IsFactPaused = true;
                }
            },
            p => factThread != null && factThread.IsAlive);

            StopFactorialCommand = new DelegateCommand(p =>
            {
                factTokenSource?.Cancel();
                factPauseEvent.Set();
                factThread = null;
                factTokenSource = null;

                dispatcher.Invoke(() =>
                {
                    LimitNumber = 0;
                    FactorialTextBlock = string.Empty;
                    IsFactPaused = false;
                });
            },
            p => factThread != null && factThread.IsAlive);
        }

        private void CubeCalculation(object state)
        {
            if (lowerLimit < 0 || highLimit >= 1000)
            {
                dispatcher.Invoke(() => MessageBox.Show("Нижняя граница от 1, верхняя до 1000"));
                return;
            }

            var token = (CancellationToken)state;
            var resultBuilder = new StringBuilder();

            for (int i = LowerLimit; i <= HighLimit; i++)
            {
                if (token.IsCancellationRequested)
                    break;

                cubePauseEvent.Wait(token);
                if (token.IsCancellationRequested)
                    break;

                var cube = Math.Pow(i, 3);
                if (resultBuilder.Length > 0)
                    resultBuilder.Append(" ");
                resultBuilder.Append(cube.ToString());

                dispatcher.Invoke(() =>
                {
                    CubeTextBlock = resultBuilder.ToString();
                });

                Thread.Sleep(1000);
            }
        }

        private void FactCalculation(object state)
        {
            if (limitNumber >= 15)
            {
                dispatcher.Invoke(() => MessageBox.Show("Число должно быть не больше 14"));
                return;
            }

            var token = (CancellationToken)state;
            var resultBuilder = new StringBuilder();

            for (int i = 1; i <= limitNumber; i++) 
            {
                if (token.IsCancellationRequested)
                    break;

                factPauseEvent.Wait(token); 
                if (token.IsCancellationRequested)
                    break;

                BigInteger fact = Fact(i);
                if (resultBuilder.Length > 0)
                    resultBuilder.Append(" ");
                resultBuilder.Append(fact.ToString());

                dispatcher.Invoke(() =>
                {
                    FactorialTextBlock = resultBuilder.ToString();
                });

                Thread.Sleep(1000);
            }
        }

        public static BigInteger Fact(BigInteger n)
        {
            if (n == 0)
                return 1;
            else
                return n * Fact(n - 1);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}