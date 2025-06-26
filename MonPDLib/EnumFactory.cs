using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPDLib
{
    public class EnumFactory
    {

        public enum EJobName
        {
            [Description("DBOPABT")]
            DBOPABT = 1,
            [Description("DBMONABT")]
            DBMONABT = 2,

            [Description("DBOPHOTEL")]
            DBOPHOTEL = 3,
            [Description("DBMONHOTEL")]
            DBMONHOTEL = 4
        }    
        
    }
}
