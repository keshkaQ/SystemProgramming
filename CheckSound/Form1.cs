using System.Media;
using System.Runtime.InteropServices;
namespace CheckSound
{
    public partial class Form1 : Form
    {
        [DllImport ("User32.dll")]
        static extern Boolean MessageBeep (UInt32 beepType);

        [DllImport("User32.dll",CharSet = CharSet.Unicode)]
        static extern int MessageBox(IntPtr hWnd,string lpText, string lpCaption, uint utype);
        public Form1()
        {
            InitializeComponent();
        }
        private void Sound()
        {
            if (int.Parse(textBox1.Text) < 0)
            {
                MessageBox(IntPtr.Zero, "MB_OK", "Заголовок MB_OK", 0x00000000);
                MessageBox(IntPtr.Zero, "MB_OKCANCEL", "Заголовок MB_OKCANCEL", 0x00000001);
                MessageBox(IntPtr.Zero, "MB_ICONINFORMATION", "Заголовок MB_ICONINFORMATION", 0x00000010);
                MessageBox(IntPtr.Zero, "MB_ICONWARNING", "Заголовок MB_ICONWARNING", 0x00000020);
                MessageBox(IntPtr.Zero, "MB_ICONERROR", "Заголовок MB_ICONERROR", 0x00000030);
                MessageBox(IntPtr.Zero, "MB_ICONQUESTION", "Заголовок MB_ICONQUESTION", 0x00000040);
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Sound();
        }
    }
}
