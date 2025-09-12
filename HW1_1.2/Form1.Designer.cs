namespace HW1_1._2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            NotepadProcess = new System.Diagnostics.Process();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(70, 18);
            button1.Name = "button1";
            button1.Size = new Size(144, 57);
            button1.TabIndex = 0;
            button1.Text = "Start Notepad";
            button1.UseVisualStyleBackColor = true;
            button1.Click += StartNotepad_Click;
            // 
            // NotepadProcess
            // 
            NotepadProcess.StartInfo.Domain = "";
            NotepadProcess.StartInfo.LoadUserProfile = false;
            NotepadProcess.StartInfo.Password = null;
            NotepadProcess.StartInfo.StandardErrorEncoding = null;
            NotepadProcess.StartInfo.StandardInputEncoding = null;
            NotepadProcess.StartInfo.StandardOutputEncoding = null;
            NotepadProcess.StartInfo.UseCredentialsForNetworkingOnly = false;
            NotepadProcess.StartInfo.UserName = "";
            NotepadProcess.SynchronizingObject = this;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(303, 87);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Task 2";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private System.Diagnostics.Process NotepadProcess;
    }
}
