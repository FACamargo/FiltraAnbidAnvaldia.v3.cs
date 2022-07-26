using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;


namespace FiltraAnbidAnvaldia
{
    public class AnvldiaRecord
    {

        // Public bNullCodFundo As Boolean
        // Public bNullData As Boolean
        public bool bNullPL;
        public bool bZeroPL;
        public bool bNullValCota;
        public bool bNullRentDia;
        public bool bNullRentMes;
        public bool bNullRentAno;
        public bool bNullRecord;

        public bool bSignalRecDate;       // indicates record with date prior to segment start!
        public bool bFlagCotaOffBounds;   // indicates record with ValCota outside bounds;
        public int  nFlagCotaOBType;      // indicates type of boundary violation for ValCota;
        public int nFlagCotaMatchPL;      // indicates that out of boundary violation for ValCota may match PL calculation;

        public bool bRemoveLine;

        public string CodFundo;
        public DateTime Data;

        public double PL;
        public double ValCota;
        public double RentDia;
        public double RentMes;
        public double RentAno;

        public int EventoTipo;
        public int HistPri;

        public string SValCota;

        public AnvldiaRecord ptrPreviousRecord;
        public AnvldiaRecord ptrNextRecord;

        public string LogText;

        // Public Delegate Function Comparison(Of In AnvlDiaRecord)(x As AnvlDiaRecord, y As AnvlDiaRecord) As Integer

        public AnvldiaRecord()
        {
            bNullPL = false;
            bZeroPL = false;
            bNullValCota = false;
            bNullRentDia = false;
            bNullRentMes = false;
            bNullRentAno = false;
            bNullRecord = false;

            bRemoveLine = false;

            bSignalRecDate = false;       // indicates record with date prior to segment start!
            bFlagCotaOffBounds = false;   // indicates record with ValCota outside bounds;
            nFlagCotaOBType = 0;          // indicates type of boundary violation for ValCota;
            nFlagCotaMatchPL = 0;         // indicates type of boundary violation for ValCota;

            // CodFundo = "";
            // Data = "01/01/1901";

            PL = 0.0;
            ValCota = 0.0;
            RentDia = 0.0;
            RentMes = 0.0;
            RentAno = 0.0;

            SValCota = null;

            EventoTipo = 0;
            HistPri = 0;

            ptrPreviousRecord = null;
            ptrNextRecord = null;

            LogText = null;
        }

        public static int AnvldiaRecordComparator(AnvldiaRecord x, AnvldiaRecord y)
        {
            int sc;
            int dc;
            sc = string.Compare(x.CodFundo, y.CodFundo);
            if (sc == 0)
            {
                dc = DateTime.Compare(x.Data, y.Data);
                return dc;
            }
            else
                return sc;
        }
    };

}
