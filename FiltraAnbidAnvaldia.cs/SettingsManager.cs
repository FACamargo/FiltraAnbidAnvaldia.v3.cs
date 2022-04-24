using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiltraAnbidAnvaldia
{
    public class SettingsManager
    {

        private const string CONFIG_FILE__PATH = @"C:\Program Files\Fortuna\";
        private const string CONFIG_FILE__NAME = "Fortuna.cfg";

        private const string cteErro1 = "ERRO: Problema ao processar LoadSettings!";
        private const string cteErro2 = "ERRO: Problema ao processar ReducedLoadSettings!";

        public string SQLServerMachineName;
        public string SQLServer_CotacoesTR;
        public string SQLServer_Noticias;
        public string DBPwd;

        public bool BackupValoresDBBeforeInit;
        public string BackupDeviceName;

        public bool SupportBulkInsert;
        public string DatFilesLocation_ForTheClient;
        public string DatFilesLocation_ForTheServer;
        public string AnbidFilesLocation;

        public bool SupportWebSiteStartStop;
        public string W3Address;
        public long NSiteFortuna;
        public long NSiteForaDoAr;

        public string ChartsCreatorsPath;
        public string ChartsArchivePath;
        public bool ChartsCopyToSecondary;
        public string ChartsArchiveSecondaryPath;
        public string SemAcordoChartsArchivePath;

        public bool GenCFGPFiles;
        public string CFGP_HTMFilesPath;
        public bool CFGP_HTMFilesCopyToSecondary;
        public string CFGP_HTMFilesSecondaryPath;
        public string CFGP_ASPFilesPath;
        public bool CFGP_ASPFilesCopyToSecondary;
        public string CFGP_ASPFilesSecondaryPath;

        public bool CopyToSecondary;

        public string AuxiliaryFilesPath;

        public bool SwapNSyncDBs;
        public string SecondarySQLServer;
        public long NSiteForaDoAr1Min;
        public string WebServerDisk;

        private bool mSettingsLoaded;
        private bool mReducedSettingsLoaded;

        public SettingsManager()
        {
            mSettingsLoaded = false;
            mReducedSettingsLoaded = false;
        }

        ~SettingsManager()
        {
            mSettingsLoaded = false;
            mReducedSettingsLoaded = false;
        }

        public bool LoadSettings()
        {
            string cLine;
            const int BufferSize = 128;
            if (!mSettingsLoaded)
            {
                try
                {
                    using (var fileStream = System.IO.File.OpenRead(CONFIG_FILE__PATH + CONFIG_FILE__NAME))
                    using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        cLine = streamReader.ReadLine();
                        SQLServerMachineName = cLine.Substring(12);

                        cLine = streamReader.ReadLine();
                        SQLServer_CotacoesTR = cLine.Substring(23);

                        cLine = streamReader.ReadLine();
                        SQLServer_Noticias = cLine.Substring(21);

                        cLine = streamReader.ReadLine();
                        DBPwd = cLine.Substring(8);

                        cLine = streamReader.ReadLine();
                        SupportBulkInsert = BooleanOf(cLine.Substring(21));

                        cLine = streamReader.ReadLine();
                        DatFilesLocation_ForTheClient = cLine.Substring(37);

                        cLine = streamReader.ReadLine();
                        DatFilesLocation_ForTheServer = cLine.Substring(37);

                        cLine = streamReader.ReadLine();
                        AnbidFilesLocation = cLine.Substring(22);

                        cLine = streamReader.ReadLine();
                        SupportWebSiteStartStop = BooleanOf(cLine.Substring(29));

                        cLine = streamReader.ReadLine();
                        W3Address = cLine.Substring(12);

                        cLine = streamReader.ReadLine();
                        NSiteFortuna = System.Convert.ToInt64(cLine.Substring(16));

                        cLine = streamReader.ReadLine();
                        NSiteForaDoAr = System.Convert.ToInt64(cLine.Substring(17));

                        cLine = streamReader.ReadLine();
                        ChartsCreatorsPath = cLine.Substring(22);

                        cLine = streamReader.ReadLine();
                        ChartsArchivePath = cLine.Substring(21);

                        cLine = streamReader.ReadLine();
                        GenCFGPFiles = BooleanOf(cLine.Substring(21));

                        cLine = streamReader.ReadLine();
                        CFGP_HTMFilesPath = cLine.Substring(17);

                        cLine = streamReader.ReadLine();
                        AuxiliaryFilesPath = cLine.Substring(22);

                        cLine = streamReader.ReadLine();
                        SwapNSyncDBs = BooleanOf(cLine.Substring(17));

                        cLine = streamReader.ReadLine();
                        SecondarySQLServer = cLine.Substring(22);

                        cLine = streamReader.ReadLine();
                        NSiteForaDoAr1Min = System.Convert.ToInt64(cLine.Substring(21));

                        cLine = streamReader.ReadLine();
                        WebServerDisk = cLine.Substring(18);

                        cLine = streamReader.ReadLine();
                        SemAcordoChartsArchivePath = cLine.Substring(31);

                        cLine = streamReader.ReadLine();
                        CFGP_ASPFilesPath = cLine.Substring(21);

                        cLine = streamReader.ReadLine();
                        CopyToSecondary = BooleanOf(cLine.Substring(25));

                        cLine = streamReader.ReadLine();
                        ChartsCopyToSecondary = BooleanOf(cLine.Substring(16));

                        cLine = streamReader.ReadLine();
                        CFGP_HTMFilesCopyToSecondary = BooleanOf(cLine.Substring(23));

                        cLine = streamReader.ReadLine();
                        CFGP_ASPFilesCopyToSecondary = BooleanOf(cLine.Substring(23));

                        cLine = streamReader.ReadLine();
                        ChartsArchiveSecondaryPath = cLine.Substring(20);

                        cLine = streamReader.ReadLine();
                        CFGP_HTMFilesSecondaryPath = cLine.Substring(27);

                        cLine = streamReader.ReadLine();
                        CFGP_ASPFilesSecondaryPath = cLine.Substring(27);

                        cLine = streamReader.ReadLine();
                        BackupValoresDBBeforeInit = BooleanOf(cLine.Substring(30));

                        cLine = streamReader.ReadLine();
                        BackupDeviceName = cLine.Substring(30);

                        streamReader.Close();
                        mSettingsLoaded = true;
                        mReducedSettingsLoaded = true;
                    }
                }
                catch
                {
                    mSettingsLoaded = false;
                    mReducedSettingsLoaded = false;
                }
            }
            return mSettingsLoaded;
        }

        public bool ReducedLoadSettings()
        {
            string cLine;
            const Int32 BufferSize = 128;
            if (!mReducedSettingsLoaded)
            {
                try
                {
                    using (var fileStream = System.IO.File.OpenRead(CONFIG_FILE__PATH + CONFIG_FILE__NAME))
                    using (var streamReader = new System.IO.StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        // arquivo existe
                        // obtém dele os valores dos parâmetros

                        cLine = streamReader.ReadLine();
                        SQLServerMachineName = cLine.Substring(12);

                        cLine = streamReader.ReadLine();
                        SQLServer_CotacoesTR = cLine.Substring(23);

                        cLine = streamReader.ReadLine();
                        SQLServer_Noticias = cLine.Substring(21);

                        cLine = streamReader.ReadLine();
                        DBPwd = cLine.Substring(8);

                        mReducedSettingsLoaded = true;
                        streamReader.Close();
                    }
                }
                catch
                {
                    mReducedSettingsLoaded = false;
                }
            }
            return mReducedSettingsLoaded;
        }

        private bool BooleanOf(string v)
        {
            if ((v == "True"))
                return true;
            else
                return false;
        }
    }
}
