using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Models
{
    public class FirebaseClientData
    {
        public string accessToken { get; set; }
        public string displayName { get; set; }
        public string photoUrl { get; set; }
        public string email { get; set; }
        public FirebaseProviderUserInfo[] providerData { get; set; }
    }
}
