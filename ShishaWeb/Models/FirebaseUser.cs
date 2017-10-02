using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Models
{
    public class FirebaseUser
    {
        public string localId { get; set; }
        public string email { get; set; }
        public bool emailVerified { get; set; }
        public string displayName { get; set; }
        public string photoUrl { get; set; }
        public string validSince { get; set; }
        public string lastLoginAt { get; set; }
        public string createdAt { get; set; }
        public FirebaseProviderUserInfo[] providerUserInfo { get; set; }
    }
}
