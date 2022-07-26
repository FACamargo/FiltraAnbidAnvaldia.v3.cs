using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiltraAnbidAnvaldia
{
    static public class AAAGlobals
    {
        static public SettingsManager oSettingsManager = new SettingsManager();

        static public DateTime AlternateTime = new DateTime(1900, 1, 1, 0, 0, 0);

        static public DateTime ResetTime = new DateTime(1900, 1, 1, 6, 0, 0);

        static public DateTime DataZero = new DateTime();

        static public DateTime DataUMA = new DateTime();

        static public double FixFundoAberturaMaxDiff = 0.0000005;  // 5.0x10-7

        static public bool FixFundoAberturaFirstRecordNaoBateu = true;

        static public bool FixFundoAberturaRegistroDuplicado = true;

        static public bool FixFundoAberturaMissingPLRecord = true;

        static public bool FixFundoAberturaRemoveMatchedRecord = true;

        static public bool FixFundoAberturaRemoveFourthRecordAndBeyond = true;

        static public bool FixFundoAberturaSemDataUMARemoveRecord = true;
    }
}
