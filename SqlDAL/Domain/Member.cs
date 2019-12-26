using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public class Member
    {
        public virtual long Id { get; set; }

        public virtual string Email { get; set; }

        public virtual string Alias { get; set; }

        public virtual DateTime Dob { get; set; }

        public virtual bool IsActive { get; set; }
    }
}
