using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiltraAnbidAnvaldia
{
    public partial class AnvldiaForm : Form
    {
        private static CultureInfo sBr = new CultureInfo("pt-BR");

        private AnvldiaFile theFile;
        private string theCodFundo;
        private int theFirstRow;
        private int theLastRow;

        private List<AnvldiaRecord> theList;

        public AnvldiaForm()
        {
            InitializeComponent();

            //anvldiaGridViewAddRow();

            //anvldiaGridView.SelectionChanged += anvldiaGridView_SelectionChanged;

            anvldiaGridView.UserDeletingRow += anvldiaGridView_UserDeletingRow;

            //anvldiaGridView.UserAddedRow += anvldiaGridView_UserAddedRow;

            anvldiaGridView.RowHeaderMouseDoubleClick += anvldiaGridView_RowHeaderMouseDoubleClick;

            anvldiaGridView.KeyDown += anvldiaGridView_KeyDown;

            anvldiaGridView.RowsDefaultCellStyle.BackColor = Color.White; //.Bisque;
            anvldiaGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Bisque; // Color.Beige;
        }

        public void InitializeGrid(AnvldiaFile aFile, int aFirstRow, int aLastRow)
        {
            int i;

            int id = 0;

            theFile = aFile;
            theCodFundo = aFile.FileRecords[aFirstRow].CodFundo;

            theFirstRow = aFirstRow;
            theLastRow = aLastRow;

            theList = new List<AnvldiaRecord>();

            id = 0;
            i = aLastRow;
            while (i>=aFirstRow)
            {

                AnvldiaRecord aRecord = aFile.FileRecords[i];
                theList.Add(aRecord);
                id = ANVLDIAGridViewAddRow(id, aRecord);
                while (!(aRecord.ptrPreviousRecord is null))
                {
                    aRecord = aRecord.ptrPreviousRecord;
                    id = ANVLDIAGridViewAddRow(id, aRecord);
                    theList.Add(aRecord);
                };
                i--;
            }

            for (i= aFile.DBRecords.Count-1; i >= 0; i--)
            {
                DATABASEGridViewAddRow(i,aFile.DBRecords[i]);
            }

        }

        public int ANVLDIAGridViewAddRow(int id, AnvldiaRecord aRecord)
        {
            DataGridViewRow aRow = new DataGridViewRow();

            aRow.CreateCells(anvldiaGridView);

            aRow.Cells[0].Value = false;
            aRow.Cells[1].Value = id.ToString();
            aRow.Cells[2].Value = aRecord.CodFundo;
            aRow.Cells[3].Value = aRecord.Data.ToString("dd/MM/yyyy");

            if (aRecord.bZeroPL) aRow.Cells[3].Style.BackColor = Color.MediumSpringGreen;
            else if (aRecord.bSignalRecDate) aRow.Cells[3].Style.BackColor = Color.Yellow;
            else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 1) aRow.Cells[3].Style.BackColor = Color.MediumSlateBlue;
            else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 2) aRow.Cells[3].Style.BackColor = Color.LightBlue;
            else if (aRecord.bNullRecord) aRow.Cells[3].Style.BackColor = Color.Red;
            else if (aRecord.bRemoveLine) aRow.Cells[3].Style.BackColor = Color.LightCoral;
            else aRow.Cells[3].Style.BackColor = Color.White;

            aRow.Cells[4].Value = aRecord.PL.ToString("F2", sBr);
            aRow.Cells[5].Value = aRecord.ValCota.ToString("F9", sBr); // "1,23456700";
            aRow.Cells[6].Value = aRecord.RentDia.ToString("F7", sBr);
            aRow.Cells[7].Value = aRecord.RentMes.ToString("F7", sBr);
            aRow.Cells[8].Value = aRecord.RentAno.ToString("F7", sBr);
            aRow.Cells[9].Value = aRecord.bRemoveLine;
            anvldiaGridView.Rows.Add(aRow);

            return ++id;
        }

        public void DATABASEGridViewAddRow(int id, AnvldiaRecord aRecord)
        {
                DataGridViewRow aRow = new DataGridViewRow();

                aRow.CreateCells(databaseGridView);

                aRow.Cells[0].Value = false;
                aRow.Cells[1].Value = id.ToString();
                aRow.Cells[2].Value = aRecord.CodFundo;
                aRow.Cells[3].Value = aRecord.Data.ToString("dd/MM/yyyy");

                aRow.Cells[3].Style.BackColor = Color.White;

                aRow.Cells[4].Value = aRecord.PL.ToString("F2", sBr);
                aRow.Cells[5].Value = aRecord.ValCota.ToString("F9", sBr); // "1,23456700";
                aRow.Cells[6].Value = aRecord.EventoTipo.ToString();
                aRow.Cells[7].Value = aRecord.HistPri.ToString();
                aRow.Cells[8].Value = aRecord.bRemoveLine;
                databaseGridView.Rows.Add(aRow);
       }

        private void anvldiaGridView_SelectionChanged(object sender, EventArgs e)
        {
            int aRow = anvldiaGridView.CurrentRow.Index;
            int aCol = anvldiaGridView.CurrentCell.ColumnIndex;
            MessageBox.Show($"Selected cell at row={aRow} and col={aCol}.");
        }


        private void anvldiaGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int aRow = e.RowIndex;
            int aCol = e.ColumnIndex; 
            // MessageBox.Show($"Hi there row={aRow} col={aCol}");
        }

        private void anvldiaGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            int aRow = e.Row.Index;
            //int aCol = e.ColumnIndex;
            MessageBox.Show($"Trying to add a row={aRow}");
            //e.Cancel = true;
        }

        private void anvldiaGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int aRow = e.Row.Index;
            //int aCol = e.ColumnIndex;
            MessageBox.Show($"Trying to delete row={aRow}");
            e.Cancel = true;
        }

        private void anvldiaGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int aRow = e.RowIndex;
            int aCol = e.ColumnIndex;
            MessageBox.Show($"Trying to double click on row={aRow} col={aCol}");
            //e.Cancel = true;
        }
        
        /* Neat trick with FORMS
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {  if (keyData == Keys.Insert) { MessageBox.Show($"InsertKey: "); }
            return base.ProcessCmdKey(ref msg, keyData);
        }*/

        private void anvldiaGridView_KeyDown(object sender, KeyEventArgs e)
        {
            //int aRow = e.RowIndex;
            //int aCol = e.ColumnIndex;
            //MessageBox.Show($"Trying to double click on row={aRow} col={aCol}");
            //e.Cancel = true;
            if (e.KeyCode == Keys.Insert)
            {              
                MessageBox.Show($"Insert Pressed");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"http://local.fortuna.com.br/default.aspx?nWhat=Papeis&nOp1=Fundos&nWho=InfHist2&aW=F{theCodFundo}");
        }

        private void databaseGridView_CellContentClick(object sender, EventArgs e)
        {

        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            int i;
            DialogResult dr;
            for (i = 0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
            {
                anvldiaGridView.Rows[i].Cells[9].Value = true;
            }
            dr = MessageBox.Show("Remove all records and save?", "Remove Records", MessageBoxButtons.OKCancel);
            if (dr.Equals(DialogResult.OK))
            {
                for (i=0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
                { 
                    updateFileRecord(theList[i], anvldiaGridView.Rows[i], true);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {   // Cancel - sets all back
                for (i = 0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
                {
                    updateGridRow(anvldiaGridView.Rows[i], theList[i]);
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i;
            DialogResult dr;
            for (i = 0; i < theList.Count; i++) // for (i = theFirstRow; i <= theLastRow; i++)
            {
                anvldiaGridView.Rows[i].Cells[9].Value = false;
            }
            dr = MessageBox.Show("Accept all records and save?", "Accept Records", MessageBoxButtons.OKCancel);
            if (dr.Equals(DialogResult.OK))
            {
                //for (i = theFirstRow; i <= theLastRow; i++)
                for (i = 0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
                {
                        updateFileRecord(theList[i], anvldiaGridView.Rows[i], false);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                for (i = 0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
                {
                    updateGridRow(anvldiaGridView.Rows[i], theList[i]);
                }
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            int i;
            bool bRemoveLine;
            DialogResult dr;

            dr = MessageBox.Show("Process all records and save?", "Process Records", MessageBoxButtons.OKCancel);
            if (dr.Equals(DialogResult.OK))
            {
                for (i = 0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
                {
                    bRemoveLine = (bool)anvldiaGridView.Rows[i].Cells[9].Value;
                    updateFileRecord(theList[i], anvldiaGridView.Rows[i], bRemoveLine);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                for (i = 0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
                {
                    updateGridRow(anvldiaGridView.Rows[i], theList[i]);
                }
            }

        }

        private void updateFileRecord(AnvldiaRecord aRecord, DataGridViewRow aRow, bool bRemoveLine)
        {
            string sAux;
            // PL
            sAux = (string)aRow.Cells[4].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.PL = 0.0;
                aRecord.bNullPL = true;
            }
            else
            {
                aRecord.PL = double.Parse((string)aRow.Cells[4].Value, sBr);
                aRecord.bNullPL = false;
            }
            // ValCota
            sAux = (string)aRow.Cells[5].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.ValCota = 0.0;
                aRecord.SValCota = "";
                aRecord.bNullValCota = true;
            }
            else
            {
                aRecord.ValCota = double.Parse((string)aRow.Cells[5].Value, sBr);
                aRecord.SValCota = (string)aRow.Cells[5].Value;
                aRecord.bNullValCota = false;
            }
            // RentDia
            sAux = (string)aRow.Cells[6].Value;
            if (string.IsNullOrEmpty(sAux))
            {
               aRecord.RentDia = 0.0;
               aRecord.bNullRentDia = true;
            } 
            else
            {
                aRecord.RentDia = double.Parse((string)aRow.Cells[6].Value, sBr);
                aRecord.bNullRentDia = false;
            }
            // RentMes
            sAux = (string)aRow.Cells[7].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.RentMes = 0.0;
                aRecord.bNullRentMes = true;
            }
            else
            {
                aRecord.RentMes = double.Parse((string)aRow.Cells[7].Value, sBr);
                aRecord.bNullRentMes = false;
            }
            // RentAno
            sAux = (string)aRow.Cells[8].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.RentAno = 0.0;
                aRecord.bNullRentAno = true;
            }
            else
            {
                aRecord.RentAno = double.Parse((string)aRow.Cells[8].Value, sBr);
                aRecord.bNullRentAno = false;
            }
            if (aRecord.bNullPL && aRecord.bNullValCota)
            {
                aRecord.bNullRecord = true;
                bRemoveLine = true;
            }
            aRecord.bRemoveLine = bRemoveLine;
        }

        private void updateGridRow(DataGridViewRow aRow, AnvldiaRecord aRecord)
        {
            if (aRecord.bNullRecord) { }
            else
            {
                if (aRecord.bZeroPL) aRow.Cells[3].Style.BackColor = Color.MediumSpringGreen;
                else if (aRecord.bSignalRecDate) aRow.Cells[3].Style.BackColor = Color.Yellow;
                else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 1) aRow.Cells[3].Style.BackColor = Color.MediumSlateBlue;
                else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 2) aRow.Cells[3].Style.BackColor = Color.LightBlue;
                else if (aRecord.bNullRecord) aRow.Cells[3].Style.BackColor = Color.Red;
                else if (aRecord.bRemoveLine) aRow.Cells[3].Style.BackColor = Color.LightCoral;
                else aRow.Cells[3].Style.BackColor = Color.White; 

                if (aRecord.bNullPL) aRow.Cells[4].Value = "";
                else aRow.Cells[4].Value = aRecord.PL.ToString("F2", sBr);

                if (aRecord.bNullValCota) aRow.Cells[5].Value = "";
                else aRow.Cells[5].Value = aRecord.ValCota.ToString("F9", sBr);

                if (aRecord.bNullRentDia) aRow.Cells[6].Value = "";
                else aRow.Cells[6].Value = aRecord.RentDia.ToString("F7", sBr);

                if (aRecord.bNullRentMes) aRow.Cells[7].Value = "";
                else aRow.Cells[7].Value = aRecord.RentMes.ToString("F7", sBr);

                if (aRecord.bNullRentAno) aRow.Cells[8].Value = "";
                else aRow.Cells[8].Value = aRecord.RentAno.ToString("F7", sBr);

                aRow.Cells[9].Value = aRecord.bRemoveLine;
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void AnvldiaForm_Load(object sender, EventArgs e)
        {

        }
    }
}
