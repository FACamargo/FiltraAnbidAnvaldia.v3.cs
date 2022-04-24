using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiltraAnbidAnvaldia
{
    public partial class LogForm: Form
    {

        private System.Threading.Thread thread;

        private int lastLogEntryLength;

        public delegate void ThreadFinishedEventHandler();
        public event ThreadFinishedEventHandler ThreadFinished;

        public delegate void AppThread(LogForm aForm);
        public event AppThread TheAppThread;

        //public LogForm()
        //{
        //    InitializeComponent();
        //}

        // Specialized Constructor
        public LogForm(AppThread anApp)
        {
            InitializeComponent();
            TheAppThread = anApp;
        }

        private void LogForm_Load(object sender, System.EventArgs e)
        {
            thread = null;

            CBDoIt.Text = "Do it!";
            DateTimePicker1.Value = Program.theRefDate;
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult iRetVal;

            if ((thread != null))
                iRetVal = MessageBox.Show("Deseja encerrar no meio da execução do programa?", "CSV2SPW", MessageBoxButtons.OKCancel);
            else
                iRetVal = MessageBox.Show("Deseja fechar esta janela?", "CSV2SPW", MessageBoxButtons.OKCancel);
            if (iRetVal != DialogResult.OK)
            {
                CBDoIt.Text = "Do it!";
                e.Cancel = true;
            }
        }

        public void LogForm_ThreadFinished()
        {
            CBDoIt.Text = "Exit";
            thread = null;
        }

        private void CBDoIt_Click(System.Object sender, System.EventArgs e)
        {
            if (CBDoIt.Text == "Exit")
            {
                thread = null;
                this.Close();
            }
            else if (thread == null)
            {
                thread = new System.Threading.Thread(() => TheAppThread(this))  //Program.ProcessAll(this))
                {
                    IsBackground = true
                };
                this.ThreadFinished += LogForm_ThreadFinished;
                CBDoIt.Text = "Stop!";
                thread.Start();
            }
            else
            {
                thread.Abort();
                thread = null;
                AddLogNewLine("Aborted!");
                CBDoIt.Text = "Do it!";
            }
        }

        public void AddLogText(string aText)
        {
            lastLogEntryLength += aText.Length;
            LogTextBox.AppendText(aText);
            LogTextBox.SelectionStart = LogTextBox.TextLength;
            LogTextBox.ScrollToCaret();
        }

        public void AddLogNewLine(string aText)
        {
            lastLogEntryLength = aText.Length;
            LogTextBox.AppendText(Program.csCrLf + aText);
            LogTextBox.SelectionStart = LogTextBox.TextLength;
            LogTextBox.ScrollToCaret();
        }

        public void AddLogSameLine(string aText)
        {
            LogTextBox.Text = LogTextBox.Text.Remove(LogTextBox.TextLength - lastLogEntryLength);
            LogTextBox.AppendText(aText);
            lastLogEntryLength = aText.Length;
            LogTextBox.SelectionStart = LogTextBox.TextLength;
            LogTextBox.ScrollToCaret();
        }

        public void LogDone()
        {
            LogTextBox.AppendText(Program.csCrLf + "Done!" + Program.csCrLf);
            LogTextBox.SelectionStart = LogTextBox.TextLength;
            LogTextBox.ScrollToCaret();

            ThreadFinished?.Invoke(); // Conditional invocation!
        }

        private void DateTimePicker1_ValueChanged(System.Object sender, System.EventArgs e)
        {
            if (thread == null)
                Program.theRefDate = DateTimePicker1.Value;
            else
                DateTimePicker1.Value = Program.theRefDate;
        }

        private void LogForm_Resize(object sender, System.EventArgs e)
        {
            if (this.Width < 400)
                this.Width = 400;
            if (this.Height < 250)
                this.Height = 250;

            LogTextBox.Width = this.Width - 35;
            LogTextBox.Height = this.Height - 95;

            CBDoIt.Left = this.Width - 135;
        }

        private void ReuseFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (ReuseFiles.Checked) Program.reuseFiles = true;
            else Program.reuseFiles = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnvldiaForm aForm = new AnvldiaForm();

            aForm.Show();

        }
    }
}
