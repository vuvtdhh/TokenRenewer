using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenRenewer.Models
{
    // Mở rộng của TokenResponse, gồm TokenResponse + Info để lưu vào DB.
    class TokenInfo :  TokenResponse
    {
        public DateTime last_update_time { get; set; }
        public string last_update_status { get; set; }
    }
}
