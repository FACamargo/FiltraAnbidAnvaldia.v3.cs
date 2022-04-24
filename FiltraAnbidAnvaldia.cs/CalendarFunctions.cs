using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiltraAnbidAnvaldia
{
    public class Calendario
    {
        //internal Feriados Feriados;
        //internal DiasUteis DiasUteis;
        internal DiasCorridos DiasCorridos;

        public Calendario()
        {
            //Feriados = new Feriados();
            //DiasUteis = new DiasUteis();
            DiasCorridos = new DiasCorridos();
        }

        public bool IsWorkingDay(DateTime aDate) {
            // Attention: First record [0] is for safekeeping. Actual table starts at [1].
            //TimeSpan j;
            int i = 1 + (int)aDate.Subtract(DiasCorridos.theDiasCorridos[1].NDate).Days; //No need to use TotalDays...
            return DiasCorridos.theDiasCorridos[i].IsWorkingDay;
        }

        public DateTime NextWorkingDay(DateTime aDate)
        {
            // Attention: First record [0] is for safekeeping. Actual table starts at [1].
            //TimeSpan j;
            int i = 1 + (int)aDate.Subtract(DiasCorridos.theDiasCorridos[1].NDate).Days; //No need to use TotalDays...
            return DiasCorridos.theDiasCorridos[i].NextWDate;
        }

        public DateTime PreviousWorkingDay(DateTime aDate)
        {
            // Attention: First record [0] is for safekeeping. Actual table starts at [1].
            int i = 1 + (int)aDate.Subtract(DiasCorridos.theDiasCorridos[1].NDate).TotalDays;
            return DiasCorridos.theDiasCorridos[i].PrevWDate;
        }

    }

    public class Feriado
    {
        public int FeriadoId;
        public DateTime FeriadoData;
        public String FeriadoNome;
        public int PaisId;
    }

    public class DiaUtil
    {
        public int DiaUtilId;
        public DateTime WDate;
        public DateTime PrevWDate;
        public DateTime NextWDate;
    }

    public class DiaCorrido
    {
        public int DiaCorridoId;
        public DateTime NDate;
        public DateTime PrevWDate;
        public DateTime NextWDate;
        public bool IsWorkingDay;
    }

    public class Feriados
    {
        internal List<Feriado> theFeriados; 

        public Feriados()
        {
            SqlDataReader aRS;
            Feriado aFeriado;

            theFeriados = new List<Feriado>();

            theFeriados.Clear();
            aRS = DBFunctions.ExecDBCmd("Select FeriadoId, FeriadoData, FeriadoNome, PaisId from Feriados order by FeriadoData");
            if (aRS != null)
            {
                while (aRS.Read())
                {
                    aFeriado = new Feriado()
                    {
                        FeriadoId = aRS.GetInt32(0),
                        FeriadoData = aRS.GetDateTime(1), 
                        FeriadoNome = aRS.GetString(2),
                        PaisId = aRS.GetInt32(3) 
                    };
                    theFeriados.Add(aFeriado);
                }
                aRS.Close();
            }
        }
    }

    public class DiasUteis
    {
        internal List<DiaUtil> theDiasUteis;

        public DiasUteis()
        {
            SqlDataReader aRS;
            DiaUtil aDiaUtil;

            theDiasUteis = new List<DiaUtil>();

            theDiasUteis.Clear();
            aRS = DBFunctions.ExecDBCmd("Select DiaUtilId, WDate, PrevWDate, NextWDate from DiasUteis order by WDate");
            if (aRS != null)
            {
                while (aRS.Read())
                {
                    aDiaUtil = new DiaUtil()
                    {
                        DiaUtilId = aRS.GetInt32(0),
                        WDate = aRS.GetDateTime(1),
                        PrevWDate = aRS.GetDateTime(2),
                        NextWDate = aRS.GetDateTime(3)
                    };
                    theDiasUteis.Add(aDiaUtil);
                }
                aRS.Close();
            }
        }
    }

    public class DiasCorridos
    {
        internal List<DiaCorrido> theDiasCorridos;
        public DiasCorridos()
        {
            SqlDataReader aRS;
            DiaCorrido aDiaCorrido;

            theDiasCorridos = new List<DiaCorrido>();

            theDiasCorridos.Clear();
            aRS = DBFunctions.ExecDBCmd("Select DiaCorridoId, NDate, PrevWDate, NextWDate, IsWorkingDay from DiasCorridos order by NDate");
            if (aRS != null)
            {
                while (aRS.Read())
                {
                    aDiaCorrido = new DiaCorrido()
                    {
                        DiaCorridoId = aRS.GetInt32(0),
                        NDate = aRS.GetDateTime(1),
                        PrevWDate = aRS.GetDateTime(2),
                        NextWDate = aRS.GetDateTime(3),
                        IsWorkingDay = aRS.GetInt32(4).Equals(0) ? false : true
                    };
                    theDiasCorridos.Add(aDiaCorrido);
                }
                aRS.Close();
            }
        }
    }

}
