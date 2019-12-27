using System;

namespace SqlDAL.Domain
{
    public class MemberFriend
    {
        public long Id { get; set; }
        public long MemberId { get; set; }

        public Member Friend { get; set; }

        public DateTime Dob { get; set; }
    }
}
