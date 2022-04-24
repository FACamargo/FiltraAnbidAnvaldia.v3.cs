namespace FiltraAnbidAnvaldia
{
    partial class LogForm: System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Label1 = new System.Windows.Forms.Label();
            this.DateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.CBDoIt = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.ReuseFiles = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(17, 28);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(86, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Reference Date:";
            // 
            // DateTimePicker1
            // 
            this.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePicker1.Location = new System.Drawing.Point(110, 21);
            this.DateTimePicker1.Name = "DateTimePicker1";
            this.DateTimePicker1.Size = new System.Drawing.Size(98, 20);
            this.DateTimePicker1.TabIndex = 1;
            this.DateTimePicker1.ValueChanged += new System.EventHandler(this.DateTimePicker1_ValueChanged);
            // 
            // CBDoIt
            // 
            this.CBDoIt.Location = new System.Drawing.Point(574, 18);
            this.CBDoIt.Name = "CBDoIt";
            this.CBDoIt.Size = new System.Drawing.Size(102, 23);
            this.CBDoIt.TabIndex = 2;
            this.CBDoIt.Text = "Do It!";
            this.CBDoIt.UseVisualStyleBackColor = true;
            this.CBDoIt.Click += new System.EventHandler(this.CBDoIt_Click);
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(15, 47);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(661, 255);
            this.LogTextBox.TabIndex = 3;
            // 
            // ReuseFiles
            // 
            this.ReuseFiles.AutoSize = true;
            this.ReuseFiles.Location = new System.Drawing.Point(291, 24);
            this.ReuseFiles.Name = "ReuseFiles";
            this.ReuseFiles.Size = new System.Drawing.Size(118, 17);
            this.ReuseFiles.TabIndex = 4;
            this.ReuseFiles.Text = "Manual Download?";
            this.ReuseFiles.UseVisualStyleBackColor = true;
            this.ReuseFiles.CheckedChanged += new System.EventHandler(this.ReuseFiles_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(460, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 328);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ReuseFiles);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.CBDoIt);
            this.Controls.Add(this.DateTimePicker1);
            this.Controls.Add(this.Label1);
            this.Name = "LogForm";
            this.Text = "FiltraAnbidAnvaldia";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.Resize += new System.EventHandler(this.LogForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.DateTimePicker DateTimePicker1;
        private System.Windows.Forms.Button CBDoIt;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.CheckBox ReuseFiles;
        private System.Windows.Forms.Button button1;
    }
}

