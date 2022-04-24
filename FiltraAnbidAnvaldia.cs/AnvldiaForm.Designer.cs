namespace FiltraAnbidAnvaldia
{
    partial class AnvldiaForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.anvldiaGridView = new System.Windows.Forms.DataGridView();
            this.SelectFlag = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodFundo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HistoricoData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FundoPL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValCota = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RentDia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RentMes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RentAno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoveFlag = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.databaseGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anvldiaGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.databaseGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 55);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(804, 419);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnAccept);
            this.tabPage1.Controls.Add(this.btnProcess);
            this.tabPage1.Controls.Add(this.btnReject);
            this.tabPage1.Controls.Add(this.anvldiaGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(796, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ANVLDIA";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(460, 335);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(78, 38);
            this.btnAccept.TabIndex = 4;
            this.btnAccept.Text = "Accept All";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(330, 335);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(78, 38);
            this.btnProcess.TabIndex = 3;
            this.btnProcess.Text = "Process All";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(200, 335);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(78, 38);
            this.btnReject.TabIndex = 2;
            this.btnReject.Text = "Reject All";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // anvldiaGridView
            // 
            this.anvldiaGridView.AllowUserToAddRows = false;
            this.anvldiaGridView.AllowUserToDeleteRows = false;
            this.anvldiaGridView.AllowUserToResizeRows = false;
            this.anvldiaGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.anvldiaGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectFlag,
            this.LineId,
            this.CodFundo,
            this.HistoricoData,
            this.FundoPL,
            this.ValCota,
            this.RentDia,
            this.RentMes,
            this.RentAno,
            this.RemoveFlag});
            this.anvldiaGridView.Location = new System.Drawing.Point(30, 50);
            this.anvldiaGridView.Name = "anvldiaGridView";
            this.anvldiaGridView.Size = new System.Drawing.Size(743, 274);
            this.anvldiaGridView.TabIndex = 1;
            this.anvldiaGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.anvldiaGridView_CellContentClick);
            // 
            // SelectFlag
            // 
            this.SelectFlag.HeaderText = "Select";
            this.SelectFlag.MinimumWidth = 40;
            this.SelectFlag.Name = "SelectFlag";
            this.SelectFlag.Width = 40;
            // 
            // LineId
            // 
            this.LineId.HeaderText = "Id";
            this.LineId.MinimumWidth = 10;
            this.LineId.Name = "LineId";
            this.LineId.ReadOnly = true;
            this.LineId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LineId.Visible = false;
            this.LineId.Width = 80;
            // 
            // CodFundo
            // 
            this.CodFundo.HeaderText = "CodFundo";
            this.CodFundo.MinimumWidth = 10;
            this.CodFundo.Name = "CodFundo";
            this.CodFundo.ReadOnly = true;
            this.CodFundo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CodFundo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodFundo.Width = 80;
            // 
            // HistoricoData
            // 
            this.HistoricoData.HeaderText = "Data";
            this.HistoricoData.MinimumWidth = 10;
            this.HistoricoData.Name = "HistoricoData";
            this.HistoricoData.ReadOnly = true;
            this.HistoricoData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HistoricoData.Width = 80;
            // 
            // FundoPL
            // 
            this.FundoPL.HeaderText = "PL";
            this.FundoPL.MinimumWidth = 10;
            this.FundoPL.Name = "FundoPL";
            this.FundoPL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FundoPL.Width = 80;
            // 
            // ValCota
            // 
            this.ValCota.HeaderText = "Cota";
            this.ValCota.MinimumWidth = 10;
            this.ValCota.Name = "ValCota";
            this.ValCota.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ValCota.Width = 80;
            // 
            // RentDia
            // 
            this.RentDia.HeaderText = "RentDia";
            this.RentDia.MinimumWidth = 10;
            this.RentDia.Name = "RentDia";
            this.RentDia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RentDia.Width = 70;
            // 
            // RentMes
            // 
            this.RentMes.HeaderText = "RentMes";
            this.RentMes.MinimumWidth = 10;
            this.RentMes.Name = "RentMes";
            this.RentMes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RentMes.Width = 70;
            // 
            // RentAno
            // 
            this.RentAno.HeaderText = "RentAno";
            this.RentAno.MinimumWidth = 10;
            this.RentAno.Name = "RentAno";
            this.RentAno.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RentAno.Width = 70;
            // 
            // RemoveFlag
            // 
            this.RemoveFlag.HeaderText = "Remove";
            this.RemoveFlag.MinimumWidth = 50;
            this.RemoveFlag.Name = "RemoveFlag";
            this.RemoveFlag.Width = 50;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.databaseGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(796, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DataBase";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // databaseGridView
            // 
            this.databaseGridView.AllowUserToAddRows = false;
            this.databaseGridView.AllowUserToDeleteRows = false;
            this.databaseGridView.AllowUserToResizeRows = false;
            this.databaseGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.databaseGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn0,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewCheckBoxColumn2});
            this.databaseGridView.Location = new System.Drawing.Point(30, 50);
            this.databaseGridView.Name = "databaseGridView";
            this.databaseGridView.Size = new System.Drawing.Size(746, 274);
            this.databaseGridView.TabIndex = 2;
            this.databaseGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.databaseGridView_CellContentClick);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 40;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn0
            // 
            this.dataGridViewTextBoxColumn0.HeaderText = "Id";
            this.dataGridViewTextBoxColumn0.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn0.Name = "dataGridViewTextBoxColumn0";
            this.dataGridViewTextBoxColumn0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn0.Visible = false;
            this.dataGridViewTextBoxColumn0.Width = 80;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "CodFundo";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Data";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "PL";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Cota";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Event";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 70;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "HistPri";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn6.Width = 70;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.HeaderText = "Remove";
            this.dataGridViewCheckBoxColumn2.MinimumWidth = 50;
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Width = 50;
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(621, 11);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(78, 38);
            this.btnBrowser.TabIndex = 2;
            this.btnBrowser.Text = "Browser";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.button1_Click);
            // 
            // AnvldiaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 480);
            this.Controls.Add(this.btnBrowser);
            this.Controls.Add(this.tabControl1);
            this.Name = "AnvldiaForm";
            this.Text = "anvldiaForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.anvldiaGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.databaseGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView anvldiaGridView;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView databaseGridView;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn0;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodFundo;
        private System.Windows.Forms.DataGridViewTextBoxColumn HistoricoData;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundoPL;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValCota;
        private System.Windows.Forms.DataGridViewTextBoxColumn RentDia;
        private System.Windows.Forms.DataGridViewTextBoxColumn RentMes;
        private System.Windows.Forms.DataGridViewTextBoxColumn RentAno;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RemoveFlag;
    }
}