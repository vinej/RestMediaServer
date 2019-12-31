using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public class Member
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string Alias { get; set; }

        public string HashPassword { get; set; }

        public DateTime Dob { get; set; }

        public bool IsActive { get; set; }
    }
}
