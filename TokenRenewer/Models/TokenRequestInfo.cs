using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenRenewer.Models
{
    // Mở rộng của TokenRequest, gồm TokenRequest + Info lưu trong DB.
    class TokenRequestInfo : TokenRequest
    {
        public string request_url_0 { get; set; }
        public string request_url_1 { get; set; }
        public int renew_interval { get; set; }
    }
}
