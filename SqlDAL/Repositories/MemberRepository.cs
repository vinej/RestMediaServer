using NHibernate;
using NHibernate.Criterion;
using SqlDAL.Domain;
using System;
using System.Collections.Generic;

namespace SqlDAL.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public void Add(Member member)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(member);
                transaction.Commit();
            }
        }

        public void Update(Member member)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(member);
                transaction.Commit();
            }
        }

        public void Remove(Member member)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(member);
                transaction.Commit();
            }
        }

        public Member GetById(Guid memberId)
        {
            using(ISession session = NHibernateHelper.OpenSession())
                return session.Get<Member>(memberId);
        }

        public Member GetByAlias(string alias)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                Member member = session
                    .CreateCriteria(typeof(Member))
                    .Add(Restrictions.Eq("Alias", alias))
                    .UniqueResult<Member>();
                return member;
            }
        }

        public ICollection<Member> GetByIsActive()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var members = session
                    .CreateCriteria(typeof(Member))
                    .Add(Restrictions.Eq("IsActive", true))
                    .List<Member>();
                return members;
            }
        }
    }
}