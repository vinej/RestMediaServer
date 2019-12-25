using System;

namespace SqlDAL.Domain
{
    public class Friend
    {
        public virtual int Id { get; set; }
        public virtual Member Member { get; set; }

        public virtual Member TFriend { get; set; }

        public virtual DateTime Dob { get; set; }
    }
}
