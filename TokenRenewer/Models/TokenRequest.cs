using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenRenewer.Models
{
    // Object dùng để gửi request lấy Token.
    class TokenRequest
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string scope { get; set; }
        public string session_id { get; set; }
    }
}
