using System.Diagnostics;
namespace HW1_1._2
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
            DialogResult result = MessageBox.Show(
                "Завершить процесс принудительно?",
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                KillProcess();
            }
            else
            {
                WaitForExit();
            }
        }
        private void KillProcess()
        {
            Thread.Sleep(2000);
            NotepadProcess.Kill();
            NotepadProcess.WaitForExit();
            MessageBox.Show($"Процесс принудительно завершен с кодом: {NotepadProcess.ExitCode}");
        }
        private void WaitForExit()
        {
            MessageBox.Show("Самостоятельно закройте приложение");
            NotepadProcess.WaitForExit();
            MessageBox.Show($"Процесс завершен с кодом: {NotepadProcess.ExitCode}");
        }
    }
}
