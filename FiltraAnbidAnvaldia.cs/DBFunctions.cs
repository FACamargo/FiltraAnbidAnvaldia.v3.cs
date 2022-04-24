using System;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;

namespace FiltraAnbidAnvaldia
{
    public sealed class DBFunctions
    {
        //private static string connStr;

        public static SqlConnection pDBConnection;
        public static string pServerName;
        public static string pDBName;
        public static string pDBUser;
        public static string pDBPwd;

        public static string sCmd1 = "select codfundo=f1.fundocodigoanbid, data=fc1.historicodata, pl = case when fc1.eventotipoid = 1 then fc1.fundopl-fc1.fundolucro when fc1.eventotipoid = 2 then fc1.fundopl else 0.0 end, cota=fc1.fundocota, evtid = eventotipoid from ech_full as fc1 join mapaemendafundos as m1 on fc1.ativoid = m1.ativoid_ultseg join ativos as a1 on a1.ativoid = m1.ativoid join fundos as f1 on f1.ativoid = m1.ativoid where f1.fundocodigoanbid = '";
        public static string sCmd2 = "' and historicodata >= '";
        public static string sCmd3 = "' and fc1.historicodata <= a1.ativohistoricodatafinal order by fc1.historicodata";

        public static void OpenDBConn(string aServerName, string aDBName, string aDBUser, string aDBPassword)
        {
            if (pDBConnection != null) return;
            pServerName = aServerName;
            pDBName = aDBName;
            pDBUser = aDBUser;
            pDBPwd = aDBPassword;
            try
            {
                pDBConnection = new SqlConnection($"Data Source={pServerName}; Initial Catalog={pDBName}; User ID={pDBUser};Password={pDBPwd};");
                pDBConnection.Open();
            }
            catch
            {
                MessageBox.Show($"Error: cannot open/connect database {pDBName} at {pServerName}.");
                pDBConnection = null;
            }
        }

        public static SqlDataReader GetFundHistoryData(string aCodFundo, DateTime aStartDate, DateTime aEndDate)
        {

            // select codfundo=f1.fundocodigoanbid,
            //        data=fc1.historicodata,
            //        pl= case when fc1.eventotipoid = 1 then fc1.fundopl-fc1.fundolucro
            //                 when fc1.eventotipoid = 2 then fc1.fundopl
            //                 else 0.0
            //            end,
            //        cota=fc1.fundocota
            //        from fch_full as fc1 
            //        join fundos as f1
            //          on f1.ativoid = fc1.ativoid
            //        where f1.fundocodigoanbid = '000191' and historicodata >= '2018-05-01'
            //        --where f1.fundocodigoanbid = '001996'
            //        order by fc1.historicodata

            SqlCommand oCmd;
            SqlDataReader aRS;

            SqlParameter aCodAnbid;
            SqlParameter aSDate;
            SqlParameter aEDate;

            try
            {
                aCodAnbid = new SqlParameter("@FundoCodigoAnbid", aCodFundo);
                aSDate = new SqlParameter("@StartDate", aStartDate);
                aEDate = new SqlParameter("@EndDate", aEndDate);

                oCmd = new SqlCommand() {
                    Connection = pDBConnection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Get_FundoHistorico_E1",
                    CommandTimeout = 120,
                };
                oCmd.Parameters.Add(aCodAnbid);
                oCmd.Parameters.Add(aSDate);
                oCmd.Parameters.Add(aEDate);
                aRS = oCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                aRS = null;
            }
            return aRS;
        }

        public static SqlDataReader GetFundHistoryData1(string aCodFundo, DateTime aStartDate)
        {
            String aCmd;
            SqlCommand oCmd;
            SqlDataReader aRS;

            StringBuilder sb = new StringBuilder(sCmd1);
            sb.Append(aCodFundo);
            sb.Append(sCmd2);
            sb.Append(aStartDate.ToString("yyyy-MM-dd"));
            sb.Append(sCmd3);
            aCmd = sb.ToString();
            try
            {
                oCmd = new SqlCommand() { Connection = pDBConnection, CommandType = CommandType.Text, CommandText = aCmd, CommandTimeout = 120 };
                aRS = oCmd.ExecuteReader();
            }
            catch
            {
                MessageBox.Show($"Error: executing command \"{aCmd}\" on database {pDBName} at {pServerName}.");
                aRS = null;
            }
            return aRS;
        }


        public static SqlDataReader ExecDBCmd(string aCmdString)
        {
            SqlCommand oCmd;
            SqlDataReader aRS;

            try
            {
                oCmd = new SqlCommand() { Connection = pDBConnection, CommandType = CommandType.Text, CommandText = aCmdString, CommandTimeout = 120 };
                aRS = oCmd.ExecuteReader();
            }
            catch
            {
                MessageBox.Show($"Error: executing command \"{aCmdString}\" on database {pDBName} at {pServerName}.");
                aRS =null;
            }
            return aRS;
        }

        public static void CloseDBConn()
        {
            try
            {
                if ((pDBConnection != null)) pDBConnection.Close();
            }
            catch
            {
                MessageBox.Show($"Error: cannot close/disconnect database {pDBName} at {pServerName}.");
            }
            pDBConnection = null;
            return;
        }
    }
}
