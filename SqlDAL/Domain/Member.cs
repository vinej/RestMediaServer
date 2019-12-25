using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public class Member
    {
        public virtual int Id { get; set; }

        public virtual string Email { get; set; }

        public virtual string Alias { get; set; }

        public virtual DateTime Dob { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual IEnumerable<Friend> Friends { get; set; } = new List<Friend>();
        public virtual IEnumerable<Opinion> Opinions { get; set; } = new List<Opinion>();
        public virtual IEnumerable<Like> Likes { get; set; } = new List<Like>();
    }
}
