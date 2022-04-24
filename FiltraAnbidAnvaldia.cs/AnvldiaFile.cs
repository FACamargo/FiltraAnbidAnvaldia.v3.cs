using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;

namespace FiltraAnbidAnvaldia
{

    public class AnvldiaException
    {
        public Boolean IsComment { get; set; }
        public int Linha { get; set; }

        public string CodFundo { get; set; }
        public string Comparison { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set;  }
        public DateTime LastSeen { get; set; }
        public string Description { get; set; }

        public AnvldiaException()
        {

        }
        
        public AnvldiaException(string aCod, string aComp, string aDts, string anAct, string lastDts, string aDesc)
        {
            IsComment = false;
            CodFundo = aCod;
            Comparison = aComp;
            Date = DateTime.ParseExact(aDts,"dd/MM/yyyy", CultureInfo.InvariantCulture);
            Action = anAct;
            LastSeen = DateTime.ParseExact(lastDts, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Description = aDesc;
        }       
    }

    public class AnvldiaFile
    {
        private static readonly char tab = '\t';
        //private static readonly string csCrLf = "\r\n";
        //private static readonly string eol = "\n";
        private static CultureInfo sBr = new CultureInfo("pt-BR");

        public string FullFileName;

        public List<AnvldiaRecord> FileRecords;
        public int[] FileFirstRecord;
        public int[] FileLastRecord;
        public int FileNumRecords;
        public int FileNumFunds;

        public int nAddPrevious;
        public List<AnvldiaRecord> DBRecords;

        public DateTime aSearchDate;

        public DateTime MaxData = new DateTime(1900, 1, 1, 0, 0, 0);
        public DateTime MinData = new DateTime(2200, 12, 31, 23, 59, 59);

        //private int lastMatch = 0; // last exception matched with a record
        AnvldiaException TempException = new AnvldiaException("", "", "01/01/1901", "", "01/01/1901", "");
        public List<AnvldiaException> theCommentExceptionList;
        public List<AnvldiaException> theExceptionList;

        /*      public List<AnvldiaException> theExceptionList = new List<AnvldiaException> {
                    new AnvldiaException ( "207446", "==", "16/11/2021", "R", "28/02/2022", "ERRO - Cota incorreta e PL zero incorreto" ),
                    new AnvldiaException ( "241334", "==", "02/12/2021", "R", "28/02/2022", "ERRO - Cota incorreta" ),
                    new AnvldiaException ( "241334", "==", "08/12/2021", "R", "28/02/2022", "ERRO - Cota incorreta" ),
                    new AnvldiaException ( "241334", "==", "10/12/2021", "R", "28/02/2022", "ERRO - Cota incorreta" ),
                    new AnvldiaException ( "241334", "==", "14/12/2021", "R", "28/02/2022", "ERRO - Cota incorreta" ),
                    new AnvldiaException ( "305316", "<=", "15/07/2021", "R", "28/02/2022", "ERRO - Segment Overlap (31/10/2012 e 28/11/2014)" ),
                    new AnvldiaException ( "344532", "==", "03/10/2016", "R", "28/02/2022", "ERRO - Cota zero errada" ),
                    new AnvldiaException ( "424641", "<=", "28/05/2021", "R", "28/02/2022", "ERRO - Segment Overlap (24/02/2021)" ),
                    new AnvldiaException ( "475084", "<=", "06/05/2015", "R", "28/02/2022", "ERRO - Segment Overlap (See 631949 - 06/05/2022)" ),
                    new AnvldiaException ( "475084", ">=", "21/01/2022", "R", "28/02/2022", "ERRO - Segment Overlap (See 631949 - 21/01/2022)" ),
                    new AnvldiaException ( "486302", ">=", "07/02/2022", "R", "28/02/2022", "ERRO - Segment Overlap (See 632971 - 07/02/2022)" ),
                    new AnvldiaException ( "561835", "==", "02/10/2020", "R", "28/02/2022", "ERRO - Cota inicial é 1 não 1000" ),
                    new AnvldiaException ( "561894", "==", "02/10/2020", "R", "28/02/2022", "ERRO - Cota inicial é 1 não 1000" ),
                    // new AnvldiaException ( "569116", "==", "09/06/2021", "R", "28/02/2022", "ERRO - PL e Cota corretos mas fundo abertura - corrgi fundo" ),
                    new AnvldiaException ( "569771", "==", "30/04/2021", "R", "28/02/2022", "ERRO - PL e Cota incorretos" ),
                    new AnvldiaException ( "569771", "==", "31/05/2021", "R", "28/02/2022", "ERRO - PL e Cota incorretos" ),
                    new AnvldiaException ( "569771", "==", "30/06/2021", "R", "28/02/2022", "ERRO - PL e Cota incorretos" ),
                    new AnvldiaException ( "569771", "==", "30/07/2021", "R", "28/02/2022", "ERRO - PL e Cota incorretos" ),
                    new AnvldiaException ( "569771", "==", "31/08/2021", "R", "28/02/2022", "ERRO - PL e Cota incorretos" ),
                    new AnvldiaException ( "569771", "==", "30/09/2021", "R", "28/02/2022", "ERRO - PL e Cota incorretos" ),
                    new AnvldiaException ( "581321", ">=", "21/01/2022", "R", "28/02/2022", "ERRO - Segment Overlap (See 631027 - 21/01/2022)" ),
                    new AnvldiaException ( "582689", "==", "09/03/2021", "R", "28/02/2022", "ERRO - PL = 0,01 (should auto remove itself)" ),
                    new AnvldiaException ( "600032", "==", "23/04/2021", "R", "28/02/2022", "ERRO - Primeira cota é 500 e não 1000" ),
                    new AnvldiaException ( "600393", "==", "22/06/2021", "R", "28/02/2022", "ERRO - PL e Cota errados" ),
                    new AnvldiaException ( "607347", "<=", "31/08/2021", "R", "28/02/2022", "ERRO - Segment Overlap (23/9/2016 e 30/9/2019)" ),
                    new AnvldiaException ( "607355", "==", "13/05/2021", "R", "28/02/2022", "ERRO - Cota deve ser 100 e nao 1" ),
                    new AnvldiaException ( "608114", "==", "17/08/2021", "R", "28/02/2022", "ERRO - PL e Cota errados" ),
                    new AnvldiaException ( "613053", "==", "27/12/2019", "R", "28/02/2022", "ERRO - PL = 0.01 e Cota 100" ),
                    new AnvldiaException ( "614793", "==", "30/09/2021", "R", "28/02/2022", "ERRO - Cota deve ser 1 e nao 1000000" ),
                    new AnvldiaException ( "618608", "<=", "18/11/2021", "R", "28/02/2022", "ERRO - Segment Overlap (06/10/2021)" ),
                    new AnvldiaException ( "618640", "<=", "18/11/2021", "R", "28/02/2022", "ERRO - Segment Overlap (07/10/2021)" ),
                    new AnvldiaException ( "618731", "<=", "18/11/2021", "R", "28/02/2022", "ERRO - Segment Overlap (06/10/2021)" ),
                    new AnvldiaException ( "619469", "<=", "10/11/2021", "R", "28/02/2022", "ERRO - Segment Overlap (23/07/2013)" ),
                    new AnvldiaException ( "621201", "==", "31/10/2012", "R", "28/02/2022", "ERRO - PL=0.01 e Cota errados" ),
                    new AnvldiaException ( "624462", "<=", "30/11/2021", "R", "28/02/2022", "ERRO - Segment Overlap (13/11/2014 e 24/11/2014)" ),
                    new AnvldiaException ( "630871", "<=", "31/12/2021", "R", "28/02/2022", "ERRO - Fundo maluco (30/09/2021 para trás)" ),
                    new AnvldiaException ( "631027", "<=", "21/01/2022", "R", "28/02/2022", "ERRO - Segment Overlap (See 581321 - 21/01/2022)" ),
                    new AnvldiaException ( "631949", "<=", "21/01/2022", "R", "28/02/2022", "ERRO - Segment Overlap (See 475084 - 21/01/2022)" ),
                    new AnvldiaException ( "632971", "<=", "07/02/2022", "R", "28/02/2022", "ERRO - Segment Overlap (See 486302 - 07/02/2022)" )
                }; */

        private readonly AnvldiaRecordComparer dc = new AnvldiaRecordComparer();
        private readonly AnvldiaExceptionComparer xdc = new AnvldiaExceptionComparer();

        public int cntAbertura = 0;
        public int cntAberturaBateu = 0;
        public int cntAberturaRegDuplicado = 0;
        public int cntAberturaNaoBateu = 0;
        public int cntAberturaMissingPLRecord = 0;
        public int cntAberturaTwoRecords = 0;
        public int cntAberturaSozinho = 0;
        public int cntAberturaDataUMASozinho = 0;
        public int cntAberturaDataUMATwoRecords = 0;
        public int cntAberturaSemDataUMASozinho = 0;
        public int cntAberturaSemDataUMATwoRecords = 0;
        public int cntAberturaSemDataZero = 0;
        public int cntAberturaNaoSequencial = 0;
        public int cntAberturaComDataUMA = 0;
        public int cntAberturaSemDataUMA =0;
        public int cntAberturaHasOnly3Records = 0;
        public int cntAberturaHas4OrMoreRecords = 0;
        public int cntAberturaRemoveFourthRecordAndBeyond = 0;
        public int cntAberturaLeaveFourthRecordAndBeyondAlone = 0;
        public int cntAberturaRemovedRecordRecovered = 0;
        public int cntAberturaFourthRecAndBeyondDataMismatch = 0;
        public int cntAberturaThirdRecDataSequential = 0;
        public int cntAberturaThirdRecDataMismatch = 0;

        public int cntMatchRecordsCount = 0;
        public int cntMatchRecordsfromDB = 0;
        public int cntMatchRecordsPL = 0;
        public int cntMatchRecordsValCota = 0;


        public int cntAberturaAberturaSemDataUMARemoveRecord = 0;
        public int cntAberturaSemDataUMARemovedRecordRecovered = 0;

        public double nbDiffmin = 1000000.0;
        public double nbDiffmax = -1000000.0;

        public double btDiffmin = 1000000.0;
        public double btDiffmax = -1000000.0;

        public DateTime dataZero;
        public DateTime dataUMA;
                
        public AnvldiaFile()
        {
            FullFileName = "";

            FileRecords = new List<AnvldiaRecord>();
            FileLastRecord = new int[60001];
            FileFirstRecord = new int[60001];
            FileNumRecords = 0;
            FileNumFunds = 0;

            nAddPrevious = 0;

            DBRecords = new List<AnvldiaRecord>();
        }

        public void LoadExceptionsFile(string aFullFileName)
        {
            int aIndex = -1;
            string line;
            string[] fieldArray;
            AnvldiaException aRecord;

            string ExceptionFullFileName = aFullFileName;
            //ExceptionFileNumFunds = 0;

            FileInfo theFI;
            long theFileSize;
            DateTime fileCreationTime;
            DateTime fileLastWriteTime;

            //DateTime dataZero;
            //DateTime dataUMA;

            FullFileName = aFullFileName;
            List<AnvldiaException> aExceptionList = new List<AnvldiaException>();
            List<AnvldiaException> aCommentExceptionList = new List<AnvldiaException>();

            theFI = new FileInfo(aFullFileName);
            if (theFI.Exists)
            {
                fileCreationTime = theFI.CreationTime;    // Actual file creation date - for instance, copied here today
                fileLastWriteTime = theFI.LastWriteTime;  // File Last Time info update - first time creation date. This is what counts
                theFileSize = theFI.Length;
                try
                {
                    // Open the file using a stream reader.
                    using (StreamReader sr = new StreamReader(aFullFileName))
                    {
                        while ((!sr.EndOfStream))
                        {
                            aIndex += 1;
                            line = sr.ReadLine();

                            if (line.StartsWith("//")) // Commented out exception: just ignore it.
                            {
                                aRecord = new AnvldiaException()
                                {
                                    IsComment = true,
                                    Linha = aIndex,
                                    Description = line
                                };
                                aCommentExceptionList.Add(aRecord);

                            }
                            else
                            {
                                fieldArray = line.Split(tab);
                                if (fieldArray.Length == 6)
                                {
                                    aRecord = new AnvldiaException()
                                    {
                                        IsComment = false,
                                        Linha = aIndex,
                                        CodFundo = fieldArray[0],
                                        Comparison = fieldArray[1],
                                        Date = DateTime.ParseExact(fieldArray[2], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        Action = fieldArray[3],
                                        LastSeen = DateTime.ParseExact(fieldArray[4], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        Description = fieldArray[5]
                                    };
                                    aExceptionList.Add(aRecord);
                                }
                            }
                        }
                    }
                    if (aExceptionList.Count > 0)
                    {
                        Program.theLogForm.AddLogNewLine($"Exceptions Rules Loaded: {aExceptionList.Count.ToString()} exception rules.\n");
                    }
                    theExceptionList = aExceptionList;
                    theCommentExceptionList = aCommentExceptionList; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: File {aFullFileName} could not be read: {ex.Message}.", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
        }

        public void DumpExceptionsFile(string aFullExceptionFileName)
        {
            int i;
            int j;
            int k;
            int l;

            using (StreamWriter sw = new StreamWriter(aFullExceptionFileName))
            {
                i = theExceptionList.Count;
                j = 0;
                k = theCommentExceptionList.Count;
                l = 0;
                while (j<i)
                {
                    while ((l<k) && theCommentExceptionList[l].Linha < theExceptionList[j].Linha )
                    {
                        sw.WriteLine(theCommentExceptionList[l].Description);
                        l++;
                    }
                    PrintExceptionRecordLine(sw, theExceptionList[j]);
                    j++;
                }
                sw.Close();
            }
        }

        public void PrintExceptionRecordLine(StreamWriter sw, AnvldiaException aRecord)
        {
            StringBuilder sb = new StringBuilder(aRecord.CodFundo);
            sb.Append(tab);
            sb.Append(aRecord.Comparison);
            sb.Append(tab);
            sb.Append(aRecord.Date.ToString(@"dd\/MM\/yyyy"));
            sb.Append(tab);
            sb.Append(aRecord.Action);
            sb.Append(tab);
            sb.Append(aRecord.LastSeen.ToString(@"dd\/MM\/yyyy"));
            sb.Append(tab);
            sb.Append(aRecord.Description);
            sw.WriteLine(sb.ToString());
        }

        public DateTime LoadFile(string aFullFileName)
        {
            string line;
            string[] fieldArray;
            string CodFundoAnterior = "";
            AnvldiaRecord aRecord;

            FileInfo theFI;
            long theFileSize;
            DateTime fileCreationTime;
            DateTime fileLastWriteTime;

            //DateTime dataZero;
            //DateTime dataUMA;

            FullFileName = aFullFileName;
            FileNumFunds = 0;

            theFI = new FileInfo(aFullFileName);           
            if (theFI.Exists)
            {
                fileCreationTime = theFI.CreationTime;    // Actual file creation date - for instance, copied here today
                fileLastWriteTime = theFI.LastWriteTime;  // File Last Time info update - first time creation date. This is what counts
                theFileSize = theFI.Length;
                try
                {
                    // Open the file using a stream reader.
                    using (StreamReader sr = new StreamReader(aFullFileName))
                    {
                        while ((!sr.EndOfStream))
                        {
                            line = sr.ReadLine();
                            fieldArray = line.Split(tab);
                            if (fieldArray.Length == 7)
                            {
                                aRecord = new AnvldiaRecord()
                                {
                                    CodFundo = fieldArray[0],
                                    Data = DateTime.Parse(fieldArray[1], sBr.DateTimeFormat),
                                };

                                if (string.IsNullOrEmpty(fieldArray[2])) aRecord.bNullPL = true;
                                else aRecord.PL = double.Parse(fieldArray[2], sBr.NumberFormat);

                                if (string.IsNullOrEmpty(fieldArray[3])) aRecord.bNullValCota = true;
                                else
                                {
                                    aRecord.ValCota = double.Parse(fieldArray[3], sBr.NumberFormat);
                                    aRecord.SValCota = fieldArray[3];
                                }

                                if (string.IsNullOrEmpty(fieldArray[4])) aRecord.bNullRentDia = true;
                                else aRecord.RentDia = double.Parse(fieldArray[4], sBr.NumberFormat);

                                if (string.IsNullOrEmpty(fieldArray[5])) aRecord.bNullRentMes = true;
                                else aRecord.RentMes = double.Parse(fieldArray[5], sBr.NumberFormat);

                                if (string.IsNullOrEmpty(fieldArray[6])) aRecord.bNullRentAno = true;
                                else aRecord.RentAno = double.Parse(fieldArray[6], sBr.NumberFormat);

                                if (aRecord.bNullPL & aRecord.bNullValCota & aRecord.bNullRentDia & aRecord.bNullRentMes & aRecord.bNullRentAno) aRecord.bNullRecord = true;

                                if (!aRecord.bNullRecord)
                                {
                                    if (aRecord.Data > MaxData) MaxData = aRecord.Data;
                                    if (aRecord.Data < MinData) MinData = aRecord.Data;

                                    if (aRecord.CodFundo != CodFundoAnterior)
                                    {
                                        FileFirstRecord[FileNumFunds] = FileNumRecords;                              // Save THIS fund start index (Watch it: FileNumRecords updates later...)
                                        if (FileNumFunds > 0) FileLastRecord[FileNumFunds - 1] = FileNumRecords - 1; // Save PREV fund final index i.e. =FileFirstRecord[FileNumFunds] - 1;
                                        CodFundoAnterior = aRecord.CodFundo;
                                        FileNumFunds += 1;
                                    }
                                    FileNumRecords += 1; // ONLY NOW THEN UPDATES THE RECORD COUNT - All of these are very tricky - Do not move these lines around... 
                                    FileRecords.Add(aRecord);
                                } // Else - remove registro como se não existisse!
                            }
                            else
                            {
                                MessageBox.Show($"Error: Line too short: {line}", "Anvaldia", MessageBoxButtons.OK);
                            }
                        }
                        if (FileNumFunds > 0) FileLastRecord[FileNumFunds - 1] = FileNumRecords - 1; // Save LAST Fund FINAL index
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: File {aFullFileName} could not be read: {ex.Message}.", "Error", MessageBoxButtons.OK);
                    return new DateTime(0,0,0);
                }

                // If it were to VerifyOnly then could exist program around here... 

                FileRecords.Sort(AnvldiaRecord.AnvldiaRecordComparator);

                //Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - File Creation Date:{fileCreationTime.ToString("dd/MM/yyyy hh:mm:ss") }.\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - File Date:{fileLastWriteTime.ToString("yyyy-MM-dd hh:mm:ss")}.\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - File Size:{theFileSize.ToString()}.\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - NumRecords:{FileNumRecords} (non null).\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - MaxData:{MaxData.ToString("yyyy-MM-dd")}.\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - MinData:{MinData.ToString("yyyy-MM-dd")}.\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - First Fund:{FileRecords[0].CodFundo}.\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - Last Fund:{FileRecords[FileNumRecords - 1].CodFundo}.\n");
                Program.theLogForm.AddLogNewLine($"Arquivo Anvldia.txt - NumFunds:{FileNumFunds} (non null).\n\n");
                Program.theLogForm.AddLogNewLine("\n");

                dataZero = DateTime.Parse(fileLastWriteTime.ToShortDateString());
                if (!dataZero.Equals(MaxData))
                {
                    if (DialogResult.OK == MessageBox.Show($"Reset data zero to {MaxData.ToString()}?", "MESSAGE", MessageBoxButtons.OKCancel))
                    {
                        dataZero = MaxData;
                    }
                }
                dataUMA = Program.oCalendario.PreviousWorkingDay(dataZero);
            }
            else
            {
                MessageBox.Show($"Error: File {aFullFileName} could not be found.", "Error", MessageBoxButtons.OK);
                return new DateTime(0, 0, 0); ;
            }
            return dataZero;
        }

        public void DumpFile(string aFullFileName, bool AllLines, bool RemovedLines, bool AddedLines)
        {
            int i;
            int j;

            using (StreamWriter sw = new StreamWriter(aFullFileName))
            {
                i = FileRecords.Count;
                j = 0;
                while (j < i)
                {
                    if ((AllLines) || (RemovedLines == FileRecords[j].bRemoveLine) ||
                       ((!(FileRecords[j].ptrPreviousRecord is null)) && (RemovedLines == FileRecords[j].ptrPreviousRecord.bRemoveLine)))
                    {
                        if (AddedLines && !(FileRecords[j].ptrPreviousRecord is null)) sw.WriteLine("Added Previous Record");
                        PrintRecordLine(sw, FileRecords[j]);
                    }
                    j = j + 1;
                }
                sw.Close();
            }
        }

        public void DumpLogFile(string aFullFileName)
        {
            int i;
            int j;

            using (StreamWriter sw = new StreamWriter(aFullFileName))
            {
                i = FileRecords.Count;
                j = 0;
                while (j < i)
                {
                    if (!String.IsNullOrEmpty(FileRecords[j].LogText))
                    {
                        PrintLogRecordLine(sw, FileRecords[j]);
                    }
                    j = j + 1;
                }
                sw.Close();
            }
        }

        public void PrintRecordLine(StreamWriter sw, AnvldiaRecord aRecord)
        {
            if (!(aRecord.ptrPreviousRecord is null)) PrintRecordLine(sw, aRecord.ptrPreviousRecord);

            StringBuilder sb = new StringBuilder(aRecord.CodFundo);
            sb.Append(tab);

            sb.Append(aRecord.Data.ToString(@"dd\/MM\/yyyy HH:mm:ss"));
            sb.Append(tab);

            if (!aRecord.bNullPL)
            {
                if (aRecord.PL != 0) sb.Append(aRecord.PL.ToString("F2", sBr));
                else sb.Append("0");
            }
            sb.Append(tab);

            if (!aRecord.bNullValCota)
            {
                if (aRecord.ValCota != 0) sb.Append(aRecord.SValCota);  // line = line & aRecord.ValCota.ToString("F9", sBr)
                else sb.Append("0");
            }
            sb.Append(tab);
            if (!aRecord.bNullRentDia)
            {
                if (aRecord.RentDia != 0) sb.Append(aRecord.RentDia.ToString("F7", sBr));
                else sb.Append("0");
            }
            sb.Append(tab);
            if (!aRecord.bNullRentMes)
            {
                if (aRecord.RentMes != 0) sb.Append(aRecord.RentMes.ToString("F7", sBr));
                else sb.Append("0");
            }
            sb.Append(tab);
            if (!aRecord.bNullRentAno)
            {
                if (aRecord.RentAno != 0) sb.Append(aRecord.RentAno.ToString("F7", sBr));
                else sb.Append("0");
            }
            sw.WriteLine(sb.ToString());

            if (!(aRecord.ptrNextRecord  is null)) PrintRecordLine(sw, aRecord.ptrNextRecord);
        }

        public void PrintLogRecordLine(StreamWriter sw, AnvldiaRecord aRecord)
        {
            StringBuilder sb = new StringBuilder(aRecord.CodFundo);
            sb.Append(tab);

            sb.Append(aRecord.Data.ToString(@"dd\/MM\/yyyy HH:mm:ss"));
            sb.Append(tab);

            sb.Append(tab);
            sb.Append(aRecord.LogText);

            sw.WriteLine(sb.ToString());
        }


        public void FileCleanUp()
        {
            string aCodFundo = "";
            int iMax; 
            int i;

            FileLastRecord = new int[FileNumFunds];  // resize to at most current FileNumFunds records...
            FileFirstRecord = new int[FileNumFunds]; // resize to at most current FileNumFunds records...

            FileRecords.RemoveAll((x) => x.bRemoveLine);
            iMax = FileRecords.Count;

            FileNumFunds = 0;
            for (i = 0; i < iMax; i++)
            { if (!FileRecords[i].CodFundo.Equals(aCodFundo))
              {
                 FileFirstRecord[FileNumFunds] = i;                              // Save THIS fund start index (Watch it: FileNumRecords updates later...)
                 if (FileNumFunds > 0) FileLastRecord[FileNumFunds - 1] = i - 1; // Save PREV fund final index i.e. =FileFirstRecord[FileNumFunds] - 1;
                    aCodFundo = FileRecords[i].CodFundo;
                    FileNumFunds += 1;
              }
            }
            if (FileNumFunds > 0) FileLastRecord[FileNumFunds - 1] = iMax - 1; // Save LAST Fund FINAL index
            FileNumRecords = iMax;
        }

        public void FirstFilter()
        {
            int i = FileRecords.Count;
            int j = 1;
            int k = 0;

            AnvldiaRecord aRecord = FileRecords[0];

            double aValCota;
            double aPL;

            FileNumFunds = 0;
            // This should never ever happens... but... if it does... we are safe!
            if (j == i)
            {
                FileLastRecord[FileNumFunds] = 0;
                FileNumFunds = 1;
            }
            Program.theLogForm.AddLogNewLine("Filtro 1:\n");

            while (j < i)
            {
                aRecord = FileRecords[j];
                aValCota = aRecord.ValCota;
                aPL = aRecord.PL;

                //if (aRecord.CodFundo.Equals("201677"))
                //{
                    //MessageBox.Show("Here!");
                //}

                if (!aRecord.CodFundo.Equals(FileRecords[j - 1].CodFundo))
                {
                    if ((++k % 20) == 0)
                    {
                        Program.theLogForm.AddLogSameLine($"Filtro 1: Processando fundo {aRecord.CodFundo}.");
                        k = 0;
                    }
                   TempException.CodFundo = aRecord.CodFundo; // saves micro-seconds with Exceptions Searching
                   FileLastRecord[FileNumFunds] = j - 1;
                   FileNumFunds = FileNumFunds + 1;
                }
                TempException.Date = aRecord.Data; // saves micro-seconds with Exceptions Searching

                if (aRecord.bNullRecord)
                {
                    aRecord.bRemoveLine = true;
                }
                else if ((aPL >= 0.98 && aPL <= 1.02) && (aValCota <= 0.1 && aValCota > 0.0)) aRecord.bRemoveLine = true;
                else if (aPL == 5.0 && aValCota > 0.0 && aValCota <= 0.1) aRecord.bRemoveLine = true;
                else if (aPL == 4.37 && (aValCota > 0.0 && aValCota <= 0.6)) aRecord.bRemoveLine = true;
                else if (aPL > 0.0 && aPL <= 0.0101) aRecord.bRemoveLine = true; // Tricky - includes < 0.0 and = 0.01 - too bad: was removing all zeros too!
                else if ((aPL > 0.0 && aPL < 0.1) && (aValCota > 0.0 && aValCota <= 0.1)) aRecord.bRemoveLine = true; // tough - we are looking for 0,02, or 0,07
                else if (aPL == 0.0 && aValCota > 0.0 && aValCota <= 0.01) aRecord.bRemoveLine = true;
                else if ((aValCota > 0.0 && aValCota <= 0.00001) && aRecord.CodFundo.Equals(FileRecords[j - 1].CodFundo) && FileRecords[j - 1].bRemoveLine) aRecord.bRemoveLine = true;
                else if (aPL == 0.0 && aValCota == 0.0) aRecord.bRemoveLine = true;
                else if (aPL < 0.0 || aValCota < 0.0) aRecord.bRemoveLine = true;
                else if (FindException()>=0) aRecord.bRemoveLine = true;  // no need to push an aRecord...
                j = j + 1;
            }
            // Finishes up to last fund last record.
            Program.theLogForm.AddLogSameLine($"Filtro 1: Processando fundo {aRecord.CodFundo}.");
            FileLastRecord[FileNumFunds] = j-1;
            FileNumFunds = FileNumFunds + 1;
        }

        public void SecondFilter()
        {
            int fundNumber;
            int firstRecord;
            int lastRecord;
            int evTipo;
            //AnvldiaRecord aRecord;
            String aCodFundo;
            DateTime aStartDate;
            DateTime aEndDate;

            try
            {
                fundNumber = 0;
                firstRecord = 0;
                lastRecord = 0;

                Program.theLogForm.AddLogNewLine("Filtro 2:\n");

                while (fundNumber < FileNumFunds)
                {
                    firstRecord = FileFirstRecord[fundNumber];
                    lastRecord = FileLastRecord[fundNumber];

                    //if (FileRecords[firstRecord].CodFundo.Equals("640778"))
                    //{
                    //    MessageBox.Show("Here");
                    //    AnvldiaForm aForm = new AnvldiaForm();
                    //    aForm.InitializeGrid(this, firstRecord, lastRecord);
                    //    DialogResult aResult = aForm.ShowDialog();
                    //}

                    while (firstRecord < lastRecord && FileRecords[firstRecord].bRemoveLine) firstRecord++;  // Adjust beggining and end of sequence
                    while (firstRecord <= lastRecord && FileRecords[lastRecord].bRemoveLine) lastRecord--;  // Adjust beggining and end of sequence

                    if (firstRecord <= lastRecord && !FileRecords[firstRecord].bRemoveLine && !FileRecords[lastRecord].bRemoveLine)
                    {
                        aCodFundo = FileRecords[firstRecord].CodFundo;
                        aStartDate = FileRecords[firstRecord].Data;
                        aEndDate = FileRecords[lastRecord].Data;

                        Program.theLogForm.AddLogSameLine($"Filtro 2: Processando fundo {aCodFundo}.");

                        LoadFundFromDB(aCodFundo, aStartDate, aEndDate);
                        if (DBRecords.Count > 0)
                        {
                            if (FileRecords[firstRecord].CodFundo.Equals(DBRecords[0].CodFundo))
                            {
                                evTipo = DBRecords[0].EventoTipo;
                                if (evTipo == 1) FiltraFundoAbertura(fundNumber, firstRecord, lastRecord);
                                else if (evTipo == 2) FiltraFundoFechamento(fundNumber, firstRecord, lastRecord);
                                else
                                {
                                    MessageBox.Show($"Error: Fundo is not Abertura nor Fechamento: Fundo({fundNumber.ToString()}): {FileRecords[firstRecord].CodFundo}.");
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Error: DBRecord {DBRecords[0].CodFundo} does not match Fund({fundNumber.ToString()}): {FileRecords[fundNumber].CodFundo}.");
                            }
                        }
                        //else
                        //{ //Here we process the records without any DB data on the selected period
                          // processa exceções tipo 
                          // 219665 em 06/11/2012 (maybe fixed)
                          // 305316 em 31/10/2012 e 31/12/2014
                          // 621201 em 31/10/2012
                          // 619469 em 23/07/2013
                          // 624462 em 13/11/2014 e 24/11/2014
                          //  299731  01 / 04 / 2015 possivelmente corrigido
                          // 299741   04 / 05 / 2015 possivelmente corrigido
                          // 299758   04 / 05 / 2015 possivelmente corrigido
                          // 299766   04 / 05 / 2015 possivelmente corrigido
                          //  344532  03 / 10 / 2016 Erro flagrante
                          //  607347  23 / 09 / 2016 Outro segmento - erro 40131040,00 1,000000000 (tlavez trocar CNPJ)
                          //  607347  23 / 09 / 2019 Outro segmento - (tlavez trocar CNPJ)
                          //  264539  19 / 12 / 2017 00:00:00 0   1,510356010 - ja fechou em 2015
                          //  613053  27 / 12 / 2019 00:00:00 0,01    100,000000000 (MALUCO)
                          //  552178    23 / 03 / 2020 00:00:00 0   1000,000000000 possivelmente corrigido (=2015)
                          //  552208    23 / 03 / 2020 00:00:00 0   1000,000000000 possivelmente corrigido (=2015)
                          //  561835    02 / 10 / 2020 00:00:00  PL  1000,000000000 Exceção - a cota correta é 1 e não 1000
                          //  561894    02 / 10 / 2020 00:00:00  PL  1000,000000000 Exceção - a cota correta é 1 e não 1000
                          //  575771    02 / 12 / 2020 00:00:00  PL  1000,000000000 Exceção - a cota correta é 1 e não 1000 (podia corrigir)
                          //  578622    28 / 09 / 2020 é de outro segmento - e totalmente errada
                          //  579378	21/12/2020 00:00:00	2000000,00	1000,000000000  (inserted record- maybe fixed)
                          //  630871  long story - from 25/08/2016 até 30/09/2021
                          // 224571 22/11/2021 - re-fechei o fundo (exceção - registro zero cota)
                          //  241334 em 2/12/2021 a 31/01/2022
                          //  253863 em 31/12/2021 - maybe is correct!!!
                          //  411302 what the fuck - why did it not work?
                      //}

                    }
                    // else skip fund all together...
                    //firstRecord = lastRecord + 1;
                    fundNumber += 1;
                }
                
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura = {cntAbertura}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Bateu = {cntAberturaBateu} diffmin= {btDiffmin} diffmax={btDiffmax}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Nao Bateu = {cntAberturaNaoBateu} diffmin= {nbDiffmin} diffmax={nbDiffmax} {(AAAGlobals.FixFundoAberturaFirstRecordNaoBateu ? "All Fixed" : "None Fixed")}\n");
                Program.theLogForm.AddLogNewLine($"\tFundos de Abertura Two Records (=2) = {cntAberturaTwoRecords}\n");
                Program.theLogForm.AddLogNewLine($"\tFundos de Abertura Three Records (=3) = {cntAberturaHasOnly3Records}\n");
                Program.theLogForm.AddLogNewLine($"\t\tFundos de Abertura 3 Recs Data Sequential = {cntAberturaThirdRecDataSequential}\n");
                Program.theLogForm.AddLogNewLine($"\t\tFundos de Abertura 3 Recs Data Mismatch = {cntAberturaThirdRecDataMismatch}\n");
                Program.theLogForm.AddLogNewLine($"\t\tFundos de Abertura 4 or More Records (>=3) = {cntAberturaHas4OrMoreRecords}\n");
                Program.theLogForm.AddLogNewLine($"\tFundos de Abertura 4 Recs & Beyond Data Mismatch = {cntAberturaFourthRecAndBeyondDataMismatch}\n");
                Program.theLogForm.AddLogNewLine($"\t\tFundos de Abertura Remove 4th Record and Beyond = {cntAberturaRemoveFourthRecordAndBeyond} {(AAAGlobals.FixFundoAberturaRemoveFourthRecordAndBeyond ? "All Removed" : "None Removed")}\n");
                Program.theLogForm.AddLogNewLine($"\t\tFundos de Abertura Leave 4th Record and Beyond left Alone = {cntAberturaLeaveFourthRecordAndBeyondAlone}\n");
                Program.theLogForm.AddLogNewLine($"\t\tFundos de Abertura Removed Record and Recovered back = {cntAberturaRemovedRecordRecovered}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Sozinho (<2) = {cntAberturaSozinho} Left Alone\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Reg Duplicado = {cntAberturaRegDuplicado} {(AAAGlobals.FixFundoAberturaRegistroDuplicado ? "All Fixed" : "None Fixed")}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Missing PL Record = {cntAberturaMissingPLRecord} {(AAAGlobals.FixFundoAberturaMissingPLRecord ? "All Fixed" : "None Fixed")}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Nao Sequencial = {cntAberturaNaoSequencial}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Sem DataZERO = {cntAberturaSemDataZero}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Com DataUMA = {cntAberturaComDataUMA}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura DataUMA Sozinho (<2) = {cntAberturaDataUMASozinho} Left Alone\n");
                Program.theLogForm.AddLogNewLine($"\tFundos de Abertura DataUMA Two Records (=2) = {cntAberturaDataUMATwoRecords}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Sem DataUMA = {cntAberturaSemDataUMA}\n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Sem DataUMA Sozinho (<2) = {cntAberturaSemDataUMASozinho} Left Alone\n");
                Program.theLogForm.AddLogNewLine($"\tFundos de Abertura Sem DataUMA Two Records (=2) = {cntAberturaSemDataUMATwoRecords}\n");

                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Sem DataUMA Removed Record = {cntAberturaAberturaSemDataUMARemoveRecord} \n");
                Program.theLogForm.AddLogNewLine($"Fundos de Abertura Sem DataUMA Removed Recovered Record = {cntAberturaSemDataUMARemovedRecordRecovered} \n");

                Program.theLogForm.AddLogNewLine($"Matched Records Count = {cntMatchRecordsCount} \n");
                Program.theLogForm.AddLogNewLine($"Matched Records from DB = {cntMatchRecordsfromDB} \n");
                Program.theLogForm.AddLogNewLine($"Matched Records PL = {cntMatchRecordsPL} \n");
                Program.theLogForm.AddLogNewLine($"Matched Records ValCota = {cntMatchRecordsValCota} \n");

                MessageBox.Show("Second Filter done!");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}.");
            }
        }

        public void AddPreviousDBRecord(AnvldiaRecord aRecord, int anIndex, List<AnvldiaRecord> aDataBase)
        {
            DateTime aNewData = Program.oCalendario.PreviousWorkingDay(aRecord.Data);
            AnvldiaRecord aNewRecord = new AnvldiaRecord() { CodFundo = aRecord.CodFundo, Data = aNewData };

            int aDBIndex = FindFundDBRecord(aNewRecord);
            if (aDBIndex >= 0)
            {
                aNewRecord.bNullRecord = false;
                aNewRecord.bRemoveLine = false;
                aNewRecord.bNullPL = false;
                aNewRecord.bZeroPL = (aDataBase[aDBIndex].PL == 0.0) ? true : false;
                aNewRecord.PL = aDataBase[aDBIndex].PL;
                aNewRecord.bNullValCota = false;
                aNewRecord.ValCota = aDataBase[aDBIndex].ValCota;
                aNewRecord.SValCota = aDataBase[aDBIndex].SValCota;
                aNewRecord.EventoTipo = aDataBase[aDBIndex].EventoTipo;
                aNewRecord.HistPri = aDataBase[aDBIndex].HistPri;
            }
            else
            {
                aNewRecord.bNullRecord = true;
                aNewRecord.bRemoveLine = true;
                aNewRecord.bNullPL = true;
                aNewRecord.bZeroPL = true;
                aNewRecord.PL = 0;
                aNewRecord.bNullValCota = true;
                aNewRecord.ValCota = 0;
                aNewRecord.SValCota = "";
                aNewRecord.EventoTipo = aRecord.EventoTipo; // the best we can do... is this.
                aNewRecord.HistPri = -1;
            }
            aNewRecord.bNullRentDia = true;
            aNewRecord.bNullRentMes = true;
            aNewRecord.bNullRentAno = true;
            aNewRecord.RentDia = 0.0;
            aNewRecord.RentMes = 0.0;
            aNewRecord.RentAno = 0.0;

            aNewRecord.ptrPreviousRecord = null;
            aNewRecord.ptrNextRecord = null;
            aNewRecord.ptrIndex = -1;
            aNewRecord.LogText = null;

            aNewRecord.bSignalRecDate = false;       // indicates record with date prior to segment start!
            aNewRecord.bFlagCotaOffBounds = false;   // indicates record with ValCota outside bounds;
            aNewRecord.nFlagCotaOBType = 0;          // indicates type of boundary violation for ValCota;

            aRecord.ptrPreviousRecord = aNewRecord;
            aRecord.ptrIndex = anIndex;
        }

        public void FiltraFundoAbertura(int fundNumber, int firstRecord, int lastRecord)
        {
            //int iCount = 0;
            double diff;
            double diff1;
            //DateTime seqDate;
            //
            int showCount = 0;
            //int theIndex;

            int numRecs = lastRecord - firstRecord + 1;
            DateTime lastData = FileRecords[lastRecord].Data;
            String theCodFundo = FileRecords[lastRecord].CodFundo;

            //double dThisCota;

            double dLastCota = DBRecords[DBRecords.Count - 1].ValCota;

            cntAbertura += 1;

            if (lastData.Equals(dataZero))
            {   // First record is on dataZero
                if (numRecs < 2)
                { // Just one record alone, but on the correct date
                    AddPreviousDBRecord(FileRecords[lastRecord], lastRecord, DBRecords);
                    showCount = TestaCota(dLastCota, lastRecord, showCount);
                }
                else //  numRecs >=2
                {   // Tenho pelo menos 2 registros... São sequenciais?
                    if (Program.oCalendario.PreviousWorkingDay(FileRecords[lastRecord].Data).Equals(FileRecords[lastRecord - 1].Data))
                    { // Sim, dois registros sequenciais
                        if (FileRecords[lastRecord].bNullPL && !FileRecords[lastRecord - 1].bNullPL) // Missing PL on dataZero?
                        {   if (AAAGlobals.FixFundoAberturaMissingPLRecord) // Fix missing PL record and warn us about it.
                            {  FileRecords[lastRecord].PL = FileRecords[lastRecord - 1].PL * (FileRecords[lastRecord].ValCota / FileRecords[lastRecord - 1].ValCota);
                               FileRecords[lastRecord].bNullPL = false;
                               FileRecords[lastRecord].LogText = "fixed dataZero record with null PL.";
                            }
                        }
                        else if (FileRecords[lastRecord].PL == FileRecords[lastRecord - 1].PL)
                        { // Fix duplicated PL record and warn us about it.
                            if (AAAGlobals.FixFundoAberturaRegistroDuplicado)
                            {  FileRecords[lastRecord].PL = FileRecords[lastRecord - 1].PL * (FileRecords[lastRecord].ValCota / FileRecords[lastRecord - 1].ValCota);
                               FileRecords[lastRecord].LogText = "fixed datazero duplicated record.";
                            }
                        }
                        else // Does dataZero PL de abertura matches dataUMA PL de fechamento?
                        {   diff = Math.Abs(FileRecords[lastRecord].PL - FileRecords[lastRecord - 1].PL * (FileRecords[lastRecord].ValCota / FileRecords[lastRecord - 1].ValCota));
                            diff1 = diff / FileRecords[lastRecord].PL;
                            if (diff <= 0.10 || diff1 <= AAAGlobals.FixFundoAberturaMaxDiff)  // Use to be 1e-7 -- changed it to 5e-7
                            {   // Maravilha! Everything is ok! So far, so good! 
                                if (diff < btDiffmin) btDiffmin = diff1;
                                if (diff > btDiffmax) btDiffmax = diff1;
                            }
                            else
                            {   // MessageBox.Show($"Verifique Problema: Cota de Abertura não bate: Fundo {FileRecords[lastRecord].CodFundo} em {FileRecords[lastRecord].Data.ToString("yyyy-MM-dd")}.");
                                if (diff < nbDiffmin) nbDiffmin = diff1;
                                if (diff > nbDiffmax) nbDiffmax = diff1;
                                // Aqui merece processamento adicional: corrige? não corrige? log? print? what?
                                if (AAAGlobals.FixFundoAberturaFirstRecordNaoBateu)
                                {
                                    FileRecords[lastRecord].PL = FileRecords[lastRecord - 1].PL * (FileRecords[lastRecord].ValCota / FileRecords[lastRecord - 1].ValCota);
                                    FileRecords[lastRecord].LogText = "fixed oppening PL on dataZero record.";
                                }
                            }
                        }
                        // Let's check some numbers...
                        showCount = TestaCota(dLastCota, lastRecord, showCount);
                        showCount = TestaCota(dLastCota, lastRecord-1, showCount);

                        // From here on, First PL Record is fixed at best. And Second record is consistent with it...!
                        if (numRecs == 3)
                        {   // check if it is sequential
                            if (Program.oCalendario.PreviousWorkingDay(FileRecords[lastRecord - 1].Data).Equals(FileRecords[lastRecord - 2].Data))
                            { // yes, it is sequential... Only three records and Third record is sequential... 
                                AddPreviousDBRecord(FileRecords[lastRecord - 2], lastRecord - 2, DBRecords);
                            }
                            else
                            { // nope, tird record is not sequential... has to add after the second...
                                AddPreviousDBRecord(FileRecords[lastRecord - 1], lastRecord - 1, DBRecords);
                            }
                            // In either case, we now test third record for consistency (the fourth just added certainly is consistent... or so we hope)
                            showCount = TestaCota(dLastCota, lastRecord - 2, showCount);
                        }
                        else if (numRecs > 3)
                             {  // at this point we have at least 4 records...
                                if (!Program.oCalendario.PreviousWorkingDay(FileRecords[lastRecord - 1].Data).Equals(FileRecords[lastRecord - 2].Data))
                                {// third record is not sequential - Add record before the second... 
                                    AddPreviousDBRecord(FileRecords[lastRecord - 1], lastRecord - 1, DBRecords); // between 2nd and 3rd
                                    showCount = LoopThroughRecords(firstRecord, lastRecord - 2, dLastCota, showCount); // loop through third on...
                                }
                                else
                                { // third record is sequential with second. What about fourth and third?
                                  if (!Program.oCalendario.PreviousWorkingDay(FileRecords[lastRecord - 2].Data).Equals(FileRecords[lastRecord - 3].Data))
                                  { // nope... there is a gap between 3rd and 4th...
                                    AddPreviousDBRecord(FileRecords[lastRecord - 2], lastRecord - 2, DBRecords); // add between 3rd and 4th 
                                    showCount = TestaCota(dLastCota, lastRecord - 2, showCount); // check 3rd consistency
                                    showCount = LoopThroughRecords(firstRecord, lastRecord - 3, dLastCota, showCount); // loop through fourth on...
                                  }
                                  else
                                  { // third and fourth are sequential... 
                                    showCount = LoopThroughRecords(firstRecord, lastRecord - 2, dLastCota, showCount); // loop through third on...
                                  }
                                } 
                             }
                        else // numRecs = 2 
                        {    // ... it is only two records already processed.
                            // Add record just before the one before DataZero...
                            AddPreviousDBRecord(FileRecords[lastRecord - 1], lastRecord - 1, DBRecords);  
                        }
                    } // Sequential Records processing done!
                    else 
                    { // Não, não são sequenciais - Falta lógica aqui?!?... e fundos também. Ninguém caiu aqui!
                      // Second record on are consistent?
                        AddPreviousDBRecord(FileRecords[lastRecord], lastRecord, DBRecords); // add record just before the DataZero record
                        showCount = TestaCota(dLastCota, lastRecord, showCount);
                        showCount = LoopThroughRecords(firstRecord, lastRecord-1, dLastCota, showCount);
                    }
                } // First record at dataZero, processing done!
            }
            else // First record is not dataZero
            {   if (numRecs < 2) AddPreviousDBRecord(FileRecords[lastRecord], lastRecord, DBRecords); // Just one record alone, on an uncertain date
                else showCount = LoopThroughRecords(firstRecord, lastRecord, dLastCota, showCount); // more records, no hope to fixing them...
            }
            if (showCount > 0 || theCodFundo.Equals("001996"))
            {
               AnvldiaForm aForm = new AnvldiaForm();
               aForm.InitializeGrid(this, firstRecord, lastRecord);
               DialogResult aResult = aForm.ShowDialog();
            }
        }


        public int LoopThroughRecords(int firstRecord, int lastRecord, double dLastCota, int showCount)
        {
            int iCount;
            int theIndex;

            for (iCount = lastRecord; iCount >= firstRecord; iCount--)
            {
                theIndex = FindFundDBRecord(FileRecords[iCount]);
                if (theIndex >= 0)
                { // Fund record found in DB. Does it match?
                    if (DBRecords[theIndex].HistPri == 0 && MatchFundDBRecord(FileRecords[iCount], theIndex))
                    {  // Registro idêntico ao encontrado no DB: ==> REMOVE
                        if (AAAGlobals.FixFundoAberturaRemoveMatchedRecord)
                        {  // PAREI AQUI !!!
                            if (iCount < lastRecord && FileRecords[iCount].Data.Equals(Program.oCalendario.PreviousWorkingDay(FileRecords[iCount + 1].Data)))
                            {
                                if (FileRecords[iCount + 1].bRemoveLine) FileRecords[iCount].bRemoveLine = true;
                                else { FileRecords[iCount].bRemoveLine = false;
                                }
                            }
                            else { 
                                if (iCount < lastRecord) // if I get in here, they are not sequential... there is a gap...
                                {   // if previous record was not removed (it was not a match), add a supporting record just before that record
                                    if (!FileRecords[iCount + 1].bRemoveLine) AddPreviousDBRecord(FileRecords[iCount+1], iCount + 1, DBRecords); 
                                    FileRecords[iCount].bRemoveLine = true; // and remove this match...
                                }
                                else FileRecords[iCount].bRemoveLine = true;  //  first (actually last) matching record of a sequence is removed!
                            }
                        }
                    }
                    else { // Registro diferente = informação nova! Leave it alone
                          if (iCount < lastRecord && FileRecords[iCount].Data.Equals(Program.oCalendario.PreviousWorkingDay(FileRecords[iCount + 1].Data)))
                          {  // Is sequential - then ...
                             if (FileRecords[iCount + 1].bRemoveLine)
                             {  // Bring back previously removed record - actually the record ahead in time... 
                                FileRecords[iCount + 1].bRemoveLine = false;
                             }
                          }
                          else { // Does not match and is out of sequence... Nothing to do! Do not even report it! Move next;  
                                 if (iCount < lastRecord) 
                                 {  if (!FileRecords[iCount + 1].bRemoveLine) // there is a gap... try to fill it up
                                    AddPreviousDBRecord(FileRecords[iCount + 1], iCount + 1, DBRecords); // add record just before the DataZero record
                                 }
                                 if (FileRecords[iCount].Data < DBRecords[0].Data) showCount = SignalFund(dLastCota, iCount, showCount); // This record may have to be manuallly removed
                        }
                    }
                }
                // Let's check some numbers...
                showCount = TestaCota(dLastCota, iCount, showCount);
            }
            if (!FileRecords[firstRecord].bRemoveLine && (FileRecords[firstRecord].ptrPreviousRecord is null)) // did not remove first record...
               AddPreviousDBRecord(FileRecords[firstRecord], firstRecord, DBRecords); // add record just before the first record record... if possible...
            return showCount;
        }

        public void FiltraFundoFechamento(int fundNumber, int firstRecord, int lastRecord)
        {
            int iCount;
            int theIndex;
            int showCount = 0;
            int numRecs = lastRecord - firstRecord + 1;
            double dLastCota = DBRecords[DBRecords.Count - 1].ValCota;
            for (iCount = lastRecord; iCount >= firstRecord; iCount--)
            {
                theIndex = FindFundDBRecord(FileRecords[iCount]);
                if (theIndex >= 0)
                { // Fund record found in DB. Does it really exists and matches?
                    if (DBRecords[theIndex].HistPri == 0 && MatchFundDBRecord(FileRecords[iCount], theIndex))
                    {  // Identical record found in DB: ==> REMOVE
                        FileRecords[iCount].bRemoveLine = true;
                    }
                }
                else
                { // We need work here... the problem is when data spans prior segments... or pseudo-segments...
                  // Remove records before the start date of fund history... i.e., do not extend history
                  //if (FileRecords[iCount].Data < DBRecords[0].Data) FileRecords[iCount].bRemoveLine = true;
                  // For now it's better to just flag this record...
                    if (FileRecords[iCount].Data < DBRecords[0].Data) // This record may have to be removed
                        showCount = SignalFund(dLastCota, iCount, showCount);
                }
                // Let's check some numbers...
                showCount = TestaCota(dLastCota, iCount, showCount);
            }
            if (showCount > 0)
            {
                AnvldiaForm aForm = new AnvldiaForm();
                aForm.InitializeGrid(this, firstRecord, lastRecord);
                DialogResult aResult = aForm.ShowDialog();
            }
        }

        public int SignalFund(double dLastCota, int iCount, int showCount)
        {  // Notify user that fund has problem, and should be verified!
            if (showCount > 3) return showCount;
            FileRecords[iCount].LogText = "watch for record date out of segment bounds (W).";
            FileRecords[iCount].bSignalRecDate = true;
            //MessageBox.Show("Caso W: Fundo= F" + FileRecords[iCount].CodFundo + " data=" + FileRecords[iCount].Data.ToString("dd-MM-yyyy") + " WATCH IT" + csCrLf);
            return ++showCount;
        }

        public int TestaCota(double dLastCota, int iCount, int showCount)
        {  // Checks whether ValCota is within range, or should be verified!
            double dThisCota;
            if (showCount > 3) return showCount;
            // Let's check some numbers...
            if (FileRecords[iCount].PL == 0.0)
            {
                FileRecords[iCount].bZeroPL = true;
                if (!FileRecords[iCount].bRemoveLine) showCount++;
            }
            if (dLastCota != 0.0 && !FileRecords[iCount].bRemoveLine)
            {   // Check if the record is not inconsistent...
                dThisCota = FileRecords[iCount].ValCota;
                if (dThisCota / dLastCota >= 2.0)
                {
                    //MessageBox.Show("Caso A: Fundo= F" + FileRecords[iCount].CodFundo + " data=" + FileRecords[iCount].Data.ToString("dd-MM-yyyy") + csCrLf + " Esta Cota= " + dThisCota.ToString() + " Última Cota= " + dLastCota.ToString());
                    FileRecords[iCount].LogText = "watch for ValCota out of range (1).";

                    FileRecords[iCount].bFlagCotaOffBounds = true;
                    FileRecords[iCount].nFlagCotaOBType = 1; 
                    showCount++;
                }
                else if (dThisCota != 0.0)
                {
                    if (dLastCota / dThisCota >= 2.0)
                    {
                        //MessageBox.Show("Caso B: Fundo= F" + FileRecords[iCount].CodFundo + " data=" + FileRecords[iCount].Data.ToString("dd-MM-yyyy") + csCrLf + " Esta Cota= " + dThisCota.ToString() + " Última Cota= " + dLastCota.ToString());
                        FileRecords[iCount].LogText = "watch for ValCota out of range (2).";
                        FileRecords[iCount].bFlagCotaOffBounds = true;
                        FileRecords[iCount].nFlagCotaOBType = 2;
                        showCount++;
                    }
                }
            }
            return showCount;
        }


        public int FindFundDBRecord(AnvldiaRecord aRecord)
        {
            var aDate = aRecord.Data;
            int iLast = DBRecords.Count - 1;
            int index;

            if (DBRecords.Count < 6) index = DBRecords.FindIndex((AnvldiaRecord aRec) => { return aRec.Data == aRecord.Data; });
            else index = DBRecords.BinarySearch(aRecord, dc);
            return index; 
        }

        public bool MatchFundDBRecord(AnvldiaRecord aRecord, int anIndex)  // Index is a positive integer indexing DBRecords[]
        {
            bool r1, r2, r;
            double diff;

            //cntMatchRecordsCount++;

            if (DBRecords[anIndex].PL != 0.0)
            {
               diff = Math.Abs(aRecord.PL - DBRecords[anIndex].PL);
                r1 = (diff < 0.10) | ((diff / DBRecords[anIndex].PL) < 1.0e-9);   // 5.0e-8); // Really arbitrary
            }
            else if (aRecord.PL == 0.0) { r1 = true; } else { r1 = false; }

            if (DBRecords[anIndex].ValCota != 0.0)
            {
               diff = Math.Abs(aRecord.ValCota - DBRecords[anIndex].ValCota);
               r2 = (diff < 0.000001) | ((diff / DBRecords[anIndex].ValCota) < 1.0e-9);  // Really arbitrary
            }
            else if (aRecord.ValCota == 0.0) { r2 = true; } else { r2 = false; }

            //if (r1) cntMatchRecordsPL++;
            //if (r2) cntMatchRecordsValCota++;

            r = r1 & r2;
            //if (r) cntMatchRecordsfromDB++;

            return (r);

        }

        public bool MatchRecord(AnvldiaRecord aRecord)
        {
            if (aRecord.Data == aSearchDate)
                return true;
            else
                return false;
        }


        public void LoadFundFromDB(string aCodFundo, DateTime aStartdate, DateTime aEnddate)
        {
            SqlDataReader aRS;
            AnvldiaRecord aRecord;

            DBRecords.Clear();
            aRS = DBFunctions.GetFundHistoryData(aCodFundo, aStartdate, aEnddate);
            if (aRS != null)
            {
                while (aRS.Read())
                {
                    aRecord = new AnvldiaRecord()
                    {
                        CodFundo = aRS.GetString(0),                     // aRS.Fields(0).Value
                        Data = aRS.GetDateTime(1),                       // .Fields(1).Value
                        PL = aRS.GetDouble(2),                           // .Fields(2).Value
                        ValCota = aRS.GetDouble(3),                      // .Fields(3).Value
                        SValCota = aRS.GetDouble(3).ToString("F9", sBr), // KIKO
                        RentDia = 0,
                        RentMes = 0,
                        RentAno = 0,
                        EventoTipo = aRS.GetInt32(4),  // .Fields(4).Value
                        HistPri = aRS.GetInt32(5)  // .Fields(5).Value
                    };
                    DBRecords.Add(aRecord);
                }
                aRS.Close();
            }
        }

        public int FindException()  // (AnvldiaRecord aRecord) removi o parametro...
        {
            // TempException.CodFundo = aRecord.CodFundo; updated in FirstFilter main loop
            //TempException.Date = aRecord.Data ;

            //int index = theExceptionList.FindIndex(lastMatch,(AnvldiaException aRec) => { return MatchException(aRec, aRecord); });
            return theExceptionList.BinarySearch(TempException, xdc);
            //if (index >= 0) lastMatch = index;
            //return index;
        }

        // Not being used for we now run Binary Search
        public bool MatchException(AnvldiaException aRec, AnvldiaRecord aRecord)
        {
            if (aRecord.CodFundo.Equals(aRec.CodFundo))
            {
                if (aRec.Comparison.Equals("==") && aRecord.Data.Equals(aRec.Date)) return true;
                else if (aRec.Comparison.Equals("<=") && aRecord.Data <= aRec.Date) return true;
                else if (aRec.Comparison.Equals(">=") && aRecord.Data >= aRec.Date) return true;
                else return false;
            } else return false;
        }
               
    }

    public class AnvldiaRecordComparer : IComparer<AnvldiaRecord>
    {
        public int Compare(AnvldiaRecord x, AnvldiaRecord y)
        {
            int dc;
            dc = DateTime.Compare(x.Data, y.Data);
            return dc;
        }
    }

    public class AnvldiaExceptionComparer : IComparer<AnvldiaException>
    {
        public int Compare(AnvldiaException x, AnvldiaException y)
        {
            int dc;
            String cc;

            cc = x.Comparison;
            if (cc.Equals(""))
            {
                cc = y.Comparison;
                dc = x.CodFundo.CompareTo(y.CodFundo);
                if (dc < 0) return dc;
                else if (dc > 0) return dc;
                else
                {   // This logic is iffy... need some verification but seems ok!
                    if (cc.Equals("==")) { dc = x.Date.CompareTo(y.Date); return dc; }
                    else if (cc.Equals("<=")) { if (x.Date <= y.Date) return 0; else return 1; }
                    else if (cc.Equals(">=")) { if (x.Date >= y.Date) return 0; else return -1; }
                    else return 1;
                }
            }
            else
            {
                cc = x.Comparison;
                dc = x.CodFundo.CompareTo(y.CodFundo);
                if (dc < 0) return dc;
                else if (dc > 0) return dc;
                else
                {   // This logic is iffy... need some verification but seems ok!
                    if (cc.Equals("==")) { dc = x.Date.CompareTo(y.Date); return dc; }
                    else if (cc.Equals("<=")) { if (x.Date < y.Date) return -1; else return 0; }
                    else if (cc.Equals(">=")) { if (x.Date > y.Date) return 1; else return 0; }
                    else return 1;
                }
            }
        }
    }



}
