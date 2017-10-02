using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.Models
{
    public class FirebaseContactInfo
    {
        public string kind { get; set; }
        public FirebaseUser[] users { get; set; }
    }
}
