using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myoshidan.IBM.Watson.STT.Enums
{
    public enum Region
    {
        [Description("Dallas")]
        Dallas,
        [Description("Washington, DC")]
        WashingtonDC,
        [Description("Frankfurt")]
        Frankfurt,
        [Description("Sydney")]
        Sydney,
        [Description("Tokyo")]
        Tokyo,
        [Description("London")]
        London
    }
}
