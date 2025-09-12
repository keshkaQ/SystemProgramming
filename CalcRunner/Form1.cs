using System.Diagnostics;
namespace CalcRunner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            myProcess.StartInfo = new ProcessStartInfo("notepad.exe");
        }

        private void Start_Click(object sender, EventArgs e)
        {
            myProcess.Start();

        }

        private void Stop_Click(object sender, EventArgs e)
        {
            myProcess.CloseMainWindow();
            myProcess.Close();
        }
    }
}
