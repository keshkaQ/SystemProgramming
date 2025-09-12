using System.Diagnostics;
using System.Text;
namespace HW1_1._4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите оба аргумента для поиска", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filePath = @"C:\Users\TitanPC\source\repos\SystemProgramming\FindCountWord\bin\Debug\net9.0\FindCountWord.exe";
            string consoleAppDirectory = Path.GetDirectoryName(filePath);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = filePath,
                Arguments = $"\"{textBox1.Text}\" \"{textBox2.Text}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false, 
                WorkingDirectory = consoleAppDirectory, 
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    MessageBox.Show($"Слово {textBox2.Text} встречается в файле {output} раз");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске файла:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
