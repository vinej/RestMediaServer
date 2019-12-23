using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Alias { get; set; }
                
        public DateTime Dob { get; set; }

        public bool IsActive { get; set; }

        public List<User> Friends { get; set; }
    }
}
