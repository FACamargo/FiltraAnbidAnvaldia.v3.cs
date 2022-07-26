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
            anvldiaGridView.CellEndEdit += anvldiaGridView_CellEndEdit;

            anvldiaGridView.UserDeletingRow += anvldiaGridView_UserDeletingRow;
            anvldiaGridView.UserAddedRow += anvldiaGridView_UserAddedRow;
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

            System.Diagnostics.Process.Start($"http://local.fortuna.com.br/default.aspx?nWhat=Papeis&nOp1=Fundos&nWho=InfHist2&aW=F{theCodFundo}");

        }

        public int ANVLDIAGridViewAddRow(int id, AnvldiaRecord aRecord)
        {
            DataGridViewRow aRow = new DataGridViewRow();

            aRow.CreateCells(anvldiaGridView);

            aRow.Cells[0].Value = false;
            //aRow.Cells[1].Value = id.ToString();
            aRow.Cells[1].Value = aRecord.CodFundo;
            aRow.Cells[2].Value = aRecord.Data.ToString("dd/MM/yyyy");

            if (aRecord.bZeroPL) aRow.Cells[2].Style.BackColor = Color.MediumSpringGreen;
            else if (aRecord.bSignalRecDate) aRow.Cells[2].Style.BackColor = Color.Yellow;
            else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 1)
            {
                aRow.Cells[2].Style.BackColor = Color.MediumSlateBlue;
                if (aRecord.nFlagCotaMatchPL == 1) aRow.Cells[1].Style.BackColor = Color.Gold;
            }
            else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 2)
            {
                aRow.Cells[2].Style.BackColor = Color.LightBlue;
                if (aRecord.nFlagCotaMatchPL == 1) aRow.Cells[1].Style.BackColor = Color.Gold;
            }
            else if (aRecord.bNullRecord) aRow.Cells[2].Style.BackColor = Color.Red;
            else if (aRecord.bRemoveLine) aRow.Cells[2].Style.BackColor = Color.LightCoral;
            else aRow.Cells[2].Style.BackColor = aRow.Cells[3].Style.BackColor; // Color.White;

            aRow.Cells[3].Value = aRecord.bNullPL ? "" : aRecord.PL.ToString("F2", sBr);
            aRow.Cells[4].Value = aRecord.bNullValCota ? "" : aRecord.ValCota.ToString("F9", sBr); // "1,23456700";
            aRow.Cells[5].Value = aRecord.bNullRentDia ? "" : aRecord.RentDia.ToString("F7", sBr);
            aRow.Cells[6].Value = aRecord.bNullRentMes ? "" : aRecord.RentMes.ToString("F7", sBr);
            aRow.Cells[7].Value = aRecord.bNullRentAno ? "" : aRecord.RentAno.ToString("F7", sBr);
            aRow.Cells[8].Value = aRecord.bRemoveLine;
            anvldiaGridView.Rows.Add(aRow);

            return ++id;
        }

        public void anvldiaGridViewAddFirstRow()
        {
            DateTime prevDate = new DateTime();
            DateTime nextDate = new DateTime();


            prevDate = DateTime.ParseExact(anvldiaGridView.CurrentRow.Cells[2].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            nextDate = Program.oCalendario.NextWorkingDay(prevDate);

            if (nextDate > AAAGlobals.DataZero)
            {
                MessageBox.Show($"Can't insert past DataZero: {AAAGlobals.DataZero.ToString("dd/MM/yyyy")}");
                return;
            }

            DataGridViewRow aRow = (DataGridViewRow)anvldiaGridView.CurrentRow.Clone();

            for (Int32 index = 0; index < aRow.Cells.Count; index++)
            {
                aRow.Cells[index].Value = anvldiaGridView.CurrentRow.Cells[index].Value;
            }
            aRow.Cells[2].Value = nextDate.ToString("dd/MM/yyyy");
            aRow.Cells[5].Value = ""; // clear RentDia
            aRow.Cells[6].Value = ""; // clear RentMes
            aRow.Cells[7].Value = ""; // clear RentAno
            aRow.Cells[8].Value = false; // clear eventual bRemoveLine true

            anvldiaGridView.Rows.Insert(0, aRow);
            AnvldiaRecord aRecord = new AnvldiaRecord();
            updateFileRecord(aRecord, anvldiaGridView.Rows[0], false);
            theList[0].ptrNextRecord = aRecord;  
            theList.Insert(0, aRecord);
        }

        public void anvldiaGridViewAddMiddleRow(int aRowNumber)
        {
            DateTime prevDate = new DateTime();
            DateTime thisDate = new DateTime();
            DateTime nextDate = new DateTime();

            nextDate = DateTime.ParseExact(anvldiaGridView.Rows[aRowNumber-1].Cells[2].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            prevDate = DateTime.ParseExact(anvldiaGridView.Rows[aRowNumber].Cells[2].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            thisDate = Program.oCalendario.PreviousWorkingDay(nextDate);

            if (thisDate.Equals(prevDate))
            {
                MessageBox.Show($"Can't insert here: row={aRowNumber}");
                return;
            }

            DataGridViewRow aRow = (DataGridViewRow)anvldiaGridView.Rows[aRowNumber-1].Clone();

            for (Int32 index = 0; index < aRow.Cells.Count; index++)
            {
                aRow.Cells[index].Value = anvldiaGridView.Rows[aRowNumber - 1].Cells[index].Value;
            }
            aRow.Cells[2].Value = thisDate.ToString("dd/MM/yyyy");
            aRow.Cells[5].Value = ""; // clear RentDia
            aRow.Cells[6].Value = ""; // clear RentMes
            aRow.Cells[7].Value = ""; // clear RentAno
            aRow.Cells[8].Value = false; // clear eventual bRemoveLine true

            anvldiaGridView.Rows.Insert(aRowNumber, aRow);
            AnvldiaRecord aRecord = new AnvldiaRecord();
            updateFileRecord(aRecord, anvldiaGridView.Rows[aRowNumber], false);

            AnvldiaRecord auxRecord = theList[aRowNumber - 1].ptrPreviousRecord;
            theList[aRowNumber-1].ptrPreviousRecord = aRecord;
            theList.Insert(aRowNumber, aRecord);
            aRecord.ptrPreviousRecord = auxRecord;
        }

        public void DATABASEGridViewAddRow(int id, AnvldiaRecord aRecord)
        {
                DataGridViewRow aRow = new DataGridViewRow();

                aRow.CreateCells(databaseGridView);

                aRow.Cells[0].Value = false;
                //aRow.Cells[1].Value = id.ToString();
                aRow.Cells[1].Value = aRecord.CodFundo;
                aRow.Cells[2].Value = aRecord.Data.ToString("dd/MM/yyyy");

                aRow.Cells[2].Style.BackColor = aRow.Cells[3].Style.BackColor; // Color.White;

                aRow.Cells[3].Value = aRecord.PL.ToString("F2", sBr);
                aRow.Cells[4].Value = aRecord.ValCota.ToString("F9", sBr); // "1,23456700";
                aRow.Cells[5].Value = aRecord.EventoTipo.ToString();
                aRow.Cells[6].Value = aRecord.HistPri.ToString();
                aRow.Cells[7].Value = aRecord.bRemoveLine;
                databaseGridView.Rows.Add(aRow);
       }

        /*
        private void anvldiaGridView_SelectionChanged(object sender, EventArgs e)
        {
            int aRow = anvldiaGridView.CurrentRow.Index;
            int aCol = anvldiaGridView.CurrentCell.ColumnIndex;
            MessageBox.Show($"Selected cell at row={aRow} and col={aCol}.");
        }
        */
        private void anvldiaGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int aRow = e.RowIndex;
            int aCol = e.ColumnIndex;
            //if (aCol != 2) return;
            if (aCol == 2) CheckEditedDate(sender, e);
            if (aCol == 3) CheckEditedPL(sender, e);
            if (aCol == 4) CheckEditedCota(sender, e);
        }

        private void CheckEditedPL(object sender, DataGridViewCellEventArgs e)
        {
            bool res = false;
            int aRow = e.RowIndex;
            int aCol = e.ColumnIndex;
            //int aCount = anvldiaGridView.Rows.Count;
            string sPL;

            if (anvldiaGridView.Rows[aRow].Cells[aCol].Value == null) sPL = "0,00";
            else
            {
                sPL = ((string)anvldiaGridView.Rows[aRow].Cells[aCol].Value).Trim(new char[] { '+', ' ', '\t'}).Replace(".", String.Empty);
                if (string.IsNullOrEmpty(sPL)) sPL = "0,00";
            }
            res = Double.TryParse(sPL.Replace(",","."), NumberStyles.Float, CultureInfo.InvariantCulture, out double aPL);
            if (!res)
            {
                MessageBox.Show($"Invalid PL: {sPL}. Try again...");
                sPL = "0,00";
            }
            anvldiaGridView.Rows[aRow].Cells[aCol].Value = sPL;
        }

        private void CheckEditedCota(object sender, DataGridViewCellEventArgs e)
        {
            bool res = false;
            int aRow = e.RowIndex;
            int aCol = e.ColumnIndex;
            //int aCount = anvldiaGridView.Rows.Count;
            string sCota;

            if (anvldiaGridView.Rows[aRow].Cells[aCol].Value == null) sCota = "0,00";
            else
            {
                sCota = ((string)anvldiaGridView.Rows[aRow].Cells[aCol].Value).Trim(new char[] { '+', ' ', '\t' }).Replace(".", String.Empty);
                if (string.IsNullOrEmpty(sCota)) sCota = "0,00";
            }
            res = Double.TryParse(sCota.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out double aCota);
            if (!res)
            {
                MessageBox.Show($"Invalid Cota: {sCota}. Try again...");
                sCota = "0,00";
            }
            anvldiaGridView.Rows[aRow].Cells[aCol].Value = sCota;
        }

        private void CheckEditedDate(object sender, DataGridViewCellEventArgs e)
        {
            DateTime thisDate = new DateTime();
            DateTime prevDate = new DateTime();
            DateTime nextDate = new DateTime();
            bool res;
            int aRow = e.RowIndex;
            int aCol = e.ColumnIndex;
            int aCount = anvldiaGridView.Rows.Count;
           
            if (anvldiaGridView.CurrentRow.IsNewRow) return;
            if (anvldiaGridView.Rows[aCount-1].IsNewRow) return;

            string sDate = (string)anvldiaGridView.Rows[aRow].Cells[2].Value;
            res = DateTime.TryParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out thisDate);
            if (!res)
            {
                MessageBox.Show($"Invalid date: {sDate}. Try again...");
                if (aRow > 0)
                {
                    prevDate = DateTime.ParseExact((string)anvldiaGridView.Rows[aRow - 1].Cells[2].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    prevDate = Program.oCalendario.PreviousWorkingDay(prevDate);
                    anvldiaGridView.Rows[aRow].Cells[2].Value = prevDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    anvldiaGridView.Rows[aRow].Cells[2].Value = AAAGlobals.DataZero.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                if (aRow > 0)
                {
                    prevDate = DateTime.ParseExact((string)anvldiaGridView.Rows[aRow - 1].Cells[2].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (thisDate >= prevDate)
                    {
                        MessageBox.Show($"Date has to be before: {prevDate.ToString("dd/MM/yyyy")}. Try again...");
                        prevDate = Program.oCalendario.PreviousWorkingDay(prevDate);
                        anvldiaGridView.Rows[aRow].Cells[2].Value = prevDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        if (!Program.oCalendario.IsWorkingDay(thisDate))
                        {
                            MessageBox.Show($"Date {thisDate.ToString("dd/MM/yyyy")} is not working day. Try again...");
                            prevDate = Program.oCalendario.PreviousWorkingDay(prevDate);
                            anvldiaGridView.Rows[aRow].Cells[2].Value = prevDate.ToString("dd/MM/yyyy");
                        }
                        else
                        { // All set. Date seems good!
                            if (aRow < (anvldiaGridView.Rows.Count - 1))
                            {
                                nextDate = DateTime.ParseExact((string)anvldiaGridView.Rows[aRow + 1].Cells[2].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if (thisDate < nextDate)
                                {
                                    MessageBox.Show($"Date cannot be before: {nextDate.ToString("dd/MM/yyyy")}. Try again...");
                                    prevDate = Program.oCalendario.PreviousWorkingDay(prevDate);
                                    anvldiaGridView.Rows[aRow].Cells[2].Value = prevDate.ToString("dd/MM/yyyy");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (thisDate > AAAGlobals.DataZero)
                    {
                        MessageBox.Show($"Date has to at most: {AAAGlobals.DataZero.ToString("dd/MM/yyyy")}. Try again...");
                        anvldiaGridView.Rows[aRow].Cells[2].Value = AAAGlobals.DataZero.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        if (!Program.oCalendario.IsWorkingDay(thisDate))
                        {
                            MessageBox.Show($"Date {thisDate.ToString("dd/MM/yyyy")} is not working day. Try again...");
                            anvldiaGridView.Rows[aRow].Cells[2].Value = AAAGlobals.DataZero.ToString("dd/MM/yyyy");
                        }
                        else
                        { // All set. Date is good!
                            if (aRow < (anvldiaGridView.Rows.Count - 1))
                            {
                                nextDate = DateTime.ParseExact((string)anvldiaGridView.Rows[aRow + 1].Cells[2].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if (thisDate < nextDate)
                                {
                                    MessageBox.Show($"Date cannot be before: {nextDate.ToString("dd/MM/yyyy")}. Try again...");
                                    anvldiaGridView.Rows[aRow].Cells[2].Value = AAAGlobals.DataZero.ToString("dd/MM/yyyy");
                                }
                            }
                        }
                    }
                }
            }
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
            DateTime prevDate = new DateTime(); 
            anvldiaGridView.Rows[aRow-1].Cells[1].Value = anvldiaGridView.Rows[0].Cells[1].Value.ToString();
            prevDate = DateTime.ParseExact(anvldiaGridView.Rows[aRow - 2].Cells[2].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            prevDate = Program.oCalendario.PreviousWorkingDay(prevDate); 
            anvldiaGridView.Rows[aRow-1].Cells[2].Value = prevDate.ToString("dd/MM/yyyy");
            anvldiaGridView.Rows[aRow - 1].Cells[3].Value = anvldiaGridView.Rows[aRow - 2].Cells[3].Value;  // PL;
            anvldiaGridView.Rows[aRow - 1].Cells[4].Value = anvldiaGridView.Rows[aRow - 2].Cells[4].Value;  // ValCota
            anvldiaGridView.Rows[aRow - 1].Cells[5].Value = "";    // RentDia
            anvldiaGridView.Rows[aRow - 1].Cells[6].Value = "";    // RentMes
            anvldiaGridView.Rows[aRow - 1].Cells[7].Value = "";    // RentAno
            anvldiaGridView.Rows[aRow - 1].Cells[8].Value = false; // bRemoveLine

            anvldiaGridView.AllowUserToAddRows = false;

            AnvldiaRecord aRecord = new AnvldiaRecord();
            updateFileRecord(aRecord, anvldiaGridView.Rows[aRow - 1], false);
            theList[aRow - 2].ptrPreviousRecord = aRecord;
            theList.Insert(aRow - 1, aRecord); 

            //MessageBox.Show($"Just added a row={aRow}");
            //e.Cancel = true;
        }


        private void anvldiaGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int aRow = e.Row.Index;
            //int aCol = e.ColumnIndex;
            //MessageBox.Show($"Trying to delete row={aRow}");
            anvldiaGridView.Rows[aRow].Cells[8].Value = true;
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
            if (e.KeyCode == Keys.Insert)
            {
                if (anvldiaGridView.CurrentRow.IsNewRow) return;

                int aRow = anvldiaGridView.CurrentCell.RowIndex;
                int aCol = anvldiaGridView.CurrentCell.ColumnIndex;
                //MessageBox.Show($"Trying to insert on row={aRow} col={aCol}");
                if (aRow == 0) anvldiaGridViewAddFirstRow();
                else if (aRow < anvldiaGridView.RowCount) anvldiaGridViewAddMiddleRow(aRow);
                //e.Cancel = true;

            }
            if (e.KeyCode == Keys.Down)
            {
                int aRow = anvldiaGridView.CurrentCell.RowIndex;
                if (aRow == anvldiaGridView.RowCount - 1)
                {
                    anvldiaGridView.AllowUserToAddRows = true; 
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                int aRow = anvldiaGridView.CurrentCell.RowIndex;
                if (aRow == anvldiaGridView.RowCount - 1)
                {
                    anvldiaGridView.AllowUserToAddRows = false;
                }
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"http://local.fortuna.com.br/default.aspx?nWhat=Papeis&nOp1=Fundos&nWho=InfHist2&aW=F{theCodFundo}");
        }
                      
        private void btnReject_Click(object sender, EventArgs e)
        {
            int i;
            DialogResult dr;
            for (i = 0; i < theList.Count; i++) //for (i = theFirstRow; i <= theLastRow; i++)
            {
                anvldiaGridView.Rows[i].Cells[8].Value = true;
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
                anvldiaGridView.Rows[i].Cells[8].Value = false;
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
                    bRemoveLine = (bool)anvldiaGridView.Rows[i].Cells[8].Value;
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

            // Codfundo
            sAux = (string)aRow.Cells[1].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.CodFundo = "";
                aRecord.bNullRecord = true;
                aRecord.bRemoveLine = true;
            }
            else
            {
                aRecord.CodFundo = sAux;
                aRecord.bNullRecord = false;
            }

            // Date
            sAux = (string)aRow.Cells[2].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.Data = DateTime.ParseExact("01/01/1901", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                aRecord.bNullRecord = true;
                aRecord.bRemoveLine = true;
            }
            else
            {
                aRecord.Data = DateTime.ParseExact(sAux, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                aRecord.bNullRecord = false;
            }
            // PL
            sAux = (string)aRow.Cells[3].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.PL = 0.0;
                aRecord.bNullPL = true;
            }
            else
            {
                aRecord.PL = double.Parse(sAux, sBr);
                aRecord.bNullPL = false;
            }
            // ValCota
            sAux = (string)aRow.Cells[4].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.ValCota = 0.0;
                aRecord.SValCota = "";
                aRecord.bNullValCota = true;
            }
            else
            {
                aRecord.ValCota = double.Parse(sAux, sBr);
                aRecord.SValCota = sAux;
                aRecord.bNullValCota = false;
            }
            // RentDia
            sAux = (string)aRow.Cells[5].Value;
            if (string.IsNullOrEmpty(sAux))
            {
               aRecord.RentDia = 0.0;
               aRecord.bNullRentDia = true;
            } 
            else
            {
                aRecord.RentDia = double.Parse(sAux, sBr);
                aRecord.bNullRentDia = false;
            }
            // RentMes
            sAux = (string)aRow.Cells[6].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.RentMes = 0.0;
                aRecord.bNullRentMes = true;
            }
            else
            {
                aRecord.RentMes = double.Parse(sAux, sBr);
                aRecord.bNullRentMes = false;
            }
            // RentAno
            sAux = (string)aRow.Cells[7].Value;
            if (string.IsNullOrEmpty(sAux))
            {
                aRecord.RentAno = 0.0;
                aRecord.bNullRentAno = true;
            }
            else
            {
                aRecord.RentAno = double.Parse(sAux, sBr);
                aRecord.bNullRentAno = false;
            }
            if ((aRecord.bNullPL && aRecord.bNullValCota) || aRecord.bNullRecord)
            {
                aRecord.bNullRecord = true;
                bRemoveLine = true;
            }
            else aRecord.bRemoveLine = bRemoveLine; // Else here is very important!!!
        }

        private void updateGridRow(DataGridViewRow aRow, AnvldiaRecord aRecord)
        {
            if (aRecord.bNullRecord) { }
            else
            {
                if (aRecord.bZeroPL) aRow.Cells[2].Style.BackColor = Color.MediumSpringGreen;
                else if (aRecord.bSignalRecDate) aRow.Cells[2].Style.BackColor = Color.Yellow;
                else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 1)
                {
                    aRow.Cells[2].Style.BackColor = Color.MediumSlateBlue;
                    if (aRecord.nFlagCotaMatchPL == 1) aRow.Cells[1].Style.BackColor = Color.Gold; 
                }
                else if (aRecord.bFlagCotaOffBounds && aRecord.nFlagCotaOBType == 2)
                {
                    aRow.Cells[2].Style.BackColor = Color.LightBlue;
                    if (aRecord.nFlagCotaMatchPL == 1) aRow.Cells[1].Style.BackColor = Color.Gold;
                }
                else if (aRecord.bNullRecord) aRow.Cells[2].Style.BackColor = Color.Red;
                else if (aRecord.bRemoveLine) aRow.Cells[2].Style.BackColor = Color.LightCoral;
                else aRow.Cells[2].Style.BackColor = aRow.Cells[3].Style.BackColor; // Color.White; 

                if (aRecord.bNullPL) aRow.Cells[3].Value = "";
                else aRow.Cells[3].Value = aRecord.PL.ToString("F2", sBr);

                if (aRecord.bNullValCota) aRow.Cells[4].Value = "";
                else aRow.Cells[4].Value = aRecord.ValCota.ToString("F9", sBr);

                if (aRecord.bNullRentDia) aRow.Cells[5].Value = "";
                else aRow.Cells[5].Value = aRecord.RentDia.ToString("F7", sBr);

                if (aRecord.bNullRentMes) aRow.Cells[6].Value = "";
                else aRow.Cells[6].Value = aRecord.RentMes.ToString("F7", sBr);

                if (aRecord.bNullRentAno) aRow.Cells[7].Value = "";
                else aRow.Cells[7].Value = aRecord.RentAno.ToString("F7", sBr);

                aRow.Cells[8].Value = aRecord.bRemoveLine;
            }

        }

    }
}
