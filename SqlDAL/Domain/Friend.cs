using System;

namespace SqlDAL.Domain
{
    public class Friend
    {
        public virtual long Id { get; set; }
        public virtual long MemberId { get; set; }

        public virtual Member TFriend { get; set; }

        public virtual DateTime Dob { get; set; }
    }
}
