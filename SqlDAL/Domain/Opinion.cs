using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    // Give an opinion on a topic by a User
    public class Opinion
    {
        public int Id { get; set; }
        public Topic Topic { get; set; }
        public Member Member { get; set; }
        public string Comment { get; set; }
        public DateTime Dob { get; set; }
        public virtual IEnumerable<Like> Likes { get; set; } = new List<Like>();

    }
}
