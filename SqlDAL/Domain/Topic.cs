using System;

namespace SqlDAL.Domain
{
    public class Topic
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActivated { get; set; }
        public DateTime Dob { get; set; }
    }
}
