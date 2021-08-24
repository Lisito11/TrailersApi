using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrailersApi.Models {
    public class UserTrailer: IdentityUser<int>, IId {
        public int UserTrailerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
