namespace CalcRunner
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
            button2 = new Button();
            myProcess = new System.Diagnostics.Process();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(37, 64);
            button1.Name = "button1";
            button1.Size = new Size(128, 48);
            button1.TabIndex = 0;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Start_Click;
            // 
            // button2
            // 
            button2.Location = new Point(195, 64);
            button2.Name = "button2";
            button2.Size = new Size(128, 48);
            button2.TabIndex = 1;
            button2.Text = "Stop";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Stop_Click;
            // 
            // myProcess
            // 
            myProcess.StartInfo.Domain = "";
            myProcess.StartInfo.LoadUserProfile = false;
            myProcess.StartInfo.Password = null;
            myProcess.StartInfo.StandardErrorEncoding = null;
            myProcess.StartInfo.StandardInputEncoding = null;
            myProcess.StartInfo.StandardOutputEncoding = null;
            myProcess.StartInfo.UseCredentialsForNetworkingOnly = false;
            myProcess.StartInfo.UserName = "";
            myProcess.SynchronizingObject = this;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(351, 215);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private System.Diagnostics.Process myProcess;
    }
}
