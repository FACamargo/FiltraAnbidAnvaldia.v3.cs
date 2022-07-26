using System;
using System.Configuration;
using System.Windows.Forms;

namespace FiltraAnbidAnvaldia
{
    static class Program
    {
        public const string csCrLf = "\r\n";
        public const string csTab = "\t";

        public static string runPath;
        //public static string basePath;
        // public static string dataFolder;


        static public DateTime theRefDate = DateTime.Today;
        static public DateTime theGerDate;
        static public DateTime theToday = DateTime.Today;
        public static LogForm theLogForm;

        static public bool reuseFiles;

        static public Calendario oCalendario;
                

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Configuration.Configuration  cAppConfig = ConfigurationManager.OpenExeConfiguration(Application.StartupPath + @"\FiltraAnbidAnvaldia.exe");
            AppSettingsSection asSettings = cAppConfig.AppSettings;

            if (asSettings.Settings.AllKeys.Length > 0)
            {
                foreach (var aKey in asSettings.Settings.AllKeys)
                {
                    if (aKey == "runPath")
                        runPath = asSettings.Settings["runPath"].Value;
                }
            }
            else
            {
                if (runPath == "")
                    runPath = Application.StartupPath;
                asSettings.Settings.Add("runPath", "");
                asSettings.Settings["runPath"].Value = runPath;
            }
            cAppConfig.Save(System.Configuration.ConfigurationSaveMode.Modified);

            //basePath = runPath + @"\" + dataFolder;

            // Line added to avoid Thread error messages when passing log entries

            var aLogForm = new LogForm(anApp:ProcessAll); // anApp is the routine executed within LogForm
            LogForm.CheckForIllegalCrossThreadCalls = false;

            aLogForm.AddLogText($"Executando a partir do diretório {runPath}.\n");

            Application.Run(aLogForm);

            MessageBox.Show("FiltraAnbidAnvaldia done!");
        }


        public static void ProcessAll(LogForm aLogForm)
        {
            DateTime dataZero; 

            theLogForm = aLogForm;

            theGerDate = theToday;

            theLogForm.AddLogText($"Data de Geração: {theGerDate.ToString("yyyy-MM-dd")}");
            theLogForm.AddLogNewLine($"Data de Referência: {theRefDate.ToString("yyyy-MM-dd")}");

            SettingsManager aSettings = new SettingsManager();

            aSettings.LoadSettings();
           
            AnvldiaFile afile = new AnvldiaFile();

            DBFunctions.OpenDBConn("ONYX", "Valores", "sa", "b83k6la7");
        
            oCalendario = new Calendario();

            theGerDate = oCalendario.NextWorkingDay(theToday);

            dataZero = afile.LoadFile(@"C:\SIAnbid42\Anbid\Importar\anvldia.txt");

            afile.LoadExceptionsFile(@"C:\SIAnbid42\anvldiaExceptions.txt");

            Console.WriteLine($"Record Count: {afile.FileRecords.Count.ToString()}");

            // Besteirada de teste AAAGlobals
            //if (!AAAGlobals.FixFundoAberturaFirstRecordNaoBateu is true)
            //{
            //    MessageBox.Show($"Somethin weird here: {AAAGlobals.FixFundoAberturaFirstRecordNaoBateu}");
            //}

            afile.DumpFile(@"C:\SIAnbid42\Anbid\Importar\anvldia(0).txt", AllLines: true, RemovedLines: true, AddedLines: false);

            afile.FirstFilter();
            afile.DumpFile(@"C:\SIAnbid42\Anbid\Importar\anvldia(r1).txt", AllLines: false, RemovedLines: true, AddedLines: false);

            afile.FileCleanUp();

            afile.SecondFilter();

            afile.DumpLogFile(@"C:\SIAnbid42\Anbid\Importar\anvldia(1).log");

            afile.DumpFile(@"C:\SIAnbid42\Anbid\Importar\anvldia(1).txt", AllLines: false, RemovedLines: false, AddedLines: false);

            afile.DumpFile(@"C:\SIAnbid42\Anbid\Importar\anvldia(r).txt", AllLines: false, RemovedLines: true, AddedLines: false);

            afile.DumpFile(@"C:\SIAnbid42\Anbid\Importar\anvldia(a).txt", AllLines: false, RemovedLines: false, AddedLines: true);

            //if (DialogResult.OK == MessageBox.Show($"Rewrite/Dump Exceptions File?", "MESSAGE", MessageBoxButtons.OKCancel))
            //{
            //    afile.DumpExceptionsFile(@"C:\SIAnbid42\anvldiaExceptions.txt");
            //}

            DBFunctions.CloseDBConn();
            
            theLogForm.LogDone();
        }

    }
}
