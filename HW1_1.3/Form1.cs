using System.Diagnostics;
using System.Text;
namespace HW1_1._3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("+");
            comboBox1.Items.Add("-");
            comboBox1.Items.Add("/");
            comboBox1.Items.Add("*");
        }

        private void StartCalc_Click(object sender, EventArgs e)
        {
            if (Validation(textBox1.Text, textBox2.Text, comboBox1.Text))
            {
                string exePath = "CalculatorApp.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"{textBox1.Text} {textBox2.Text} {comboBox1.Text}",
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                };

                try
                {
                    using (Process process = Process.Start(startInfo))
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        MessageBox.Show($"Результат:\n{output}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при запуске калькулятора:\n{ex.Message}","Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool Validation(string num1, string num2, string operation)
        {
            if (string.IsNullOrWhiteSpace(num1) || string.IsNullOrWhiteSpace(num2))
            {
                MessageBox.Show("Введите два числа", "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!double.TryParse(num1, out double arg1) || !double.TryParse(num2, out double arg2))
            {
                MessageBox.Show("Оба аргумента должны быть числами", "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (operation == "/" && double.Parse(num2) == 0)
            {
                MessageBox.Show("Деление на ноль невозможно", "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}
