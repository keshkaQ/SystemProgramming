using System.Diagnostics;
namespace HW1_1._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            NotepadProcess.StartInfo = new ProcessStartInfo("notepad.exe");
          
        }

        private void StartNotepad_Click(object sender, EventArgs e)
        {
            NotepadProcess.Start();
            MessageBox.Show($"������� �������: {NotepadProcess.ProcessName}");
            NotepadProcess.WaitForExit();
            MessageBox.Show($"������� �������� � �����: {NotepadProcess.ExitCode}");
            NotepadProcess.CloseMainWindow();
            NotepadProcess.Close();
        }
    }
}
