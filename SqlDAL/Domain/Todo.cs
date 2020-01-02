using System;

namespace SqlDAL.Domain
{
    public class Todo
    {
        public long Id { get; set; }

        public long MemberId { get; set; }

        public string Content { get; set; }
        public bool IsDone { get; set; }

        public DateTime DoneDate { get; set; }

        public DateTime Dob { get; set; }
                
    }
}
