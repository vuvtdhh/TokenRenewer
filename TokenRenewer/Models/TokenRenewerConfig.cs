using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenRenewer.Models
{
    public class TokenRenewerConfig
    {
        public string StartStatus { get; set; }
        public string StopStatus { get; set; }
        public string AutoRenewButtonContentWhenStart { get; set; }
        public string AutoRenewButtonContentWhenStop { get; set; }
        public bool AutoStartRenewer { get; set; }
        public bool AutoRestartRenewer { get; set; }
        public int RestartRenewerAfter { get; set; }
        public int RenewInterval { get; set; }
        public int TimerInterval { get; set; }
        public string ScrollViewerHeader { get; set; }
        public string RenewSuccessMessage { get; set; }
        public string RenewFailedMessage { get; set; }
    }
}
