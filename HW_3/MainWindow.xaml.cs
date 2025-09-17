using System.Windows;

namespace HW_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Models.Numbers(); 
        }
    }
}