using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public class Topic
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Dob { get; set; }
        public virtual IEnumerable<Opinion> Opinions { get; set; } = new List<Opinion>();

    }
}
