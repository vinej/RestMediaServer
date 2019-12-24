using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public interface IMemberRepository
    {
        void Add(Member member);
        void Update(Member member);
        void Remove(Member member);
        Member GetById(Guid memberId);
        Member GetByAlias(string alias);
        ICollection<Member> GetByIsActive();
    }
}