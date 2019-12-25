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

        public virtual IList<Member> Friends { get; set; } = new List<Member>();

        public virtual IList<Member> getFriends()
        {
            return this.Friends;
        }
        public virtual void setFriends(IList<Member> friends)
        {
            this.Friends = friends;
        }

        public virtual void addFriend(Member friend)
        {
           this.Friends.Add(friend);
        }
    }
}
