using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SqlDAL.Domain;
using SqlDAL.Repositories;
using System;
using System.Collections.Generic;

[TestClass]
public class MemberRepository_Fixture
{
    private static ISessionFactory _sessionFactory;
    private static Configuration _configuration;

    private static readonly Member[] _members = new[]
                 {
                     new Member { Email = "Melon", Alias = "Fruits1", IsActive = true, Dob = DateTime.Now },
                     new Member { Email = "Pear", Alias = "Fruits2", IsActive = true, Dob = DateTime.Now},
                     new Member { Email = "Milk", Alias = "Beverages1", IsActive = false, Dob = DateTime.Now},
                     new Member { Email = "Coca Cola", Alias = "Beverages2", IsActive = false, Dob = DateTime.Now},
                     new Member { Email = "Pepsi Cola", Alias = "Beverages3", IsActive = false, Dob = DateTime.Now},
                 };

    private bool IsInCollection(Member member, ICollection<Member> fromDb)
    {
        foreach (var item in fromDb)
            if (member.Id == item.Id)
                return true;
        return false;
    }

    private static void CreateInitialData()
    {
        using (ISession session = _sessionFactory.OpenSession())
        using (ITransaction transaction = session.BeginTransaction())
        {
            foreach (var member in _members)
                session.Save(member);
            transaction.Commit();
        }
    }

    [ClassInitialize]
    public static void TestFixtureSetUp(TestContext context)
    {
        _configuration = new Configuration();
        _configuration.Configure();
        _configuration.AddAssembly(typeof(Member).Assembly);
        _sessionFactory = _configuration.BuildSessionFactory();
        CreateInitialData();
    }

    [ClassCleanup]
    public static void TestFixtureCleanUo()
    {
        IMemberRepository repository = new MemberRepository();
        bool isFirst = true;
        foreach (Member member in _members)
        {
            if (isFirst)
            {
                // the first was already remove within the testing
                isFirst = false;
            }
            else
            {
                repository.Remove(member);
            }
        }
        var member2 = new Member { Email = "Apple1", Alias = "Fruits44" };
        repository.Remove(member2);
    }

    [TestMethod]
    public void Can_add_new_member()
    {
        var member = new Member { Email = "Apple44", Alias = "Fruits44", IsActive=true, Dob = DateTime.Now };
        IMemberRepository repository = new MemberRepository();
        repository.Add(member);

        // use session to try to load the product
        using (ISession session = _sessionFactory.OpenSession())
        {
            var fromDb = session.Get<Member>(member.Id);
            // Test that the product was successfully inserted
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(member, fromDb);
            Assert.AreEqual(member.Email, fromDb.Email);
            Assert.AreEqual(member.Alias, fromDb.Alias);
        }
    }

    [TestMethod]
    public void Can_update_existing_member()
    {
        IMemberRepository repository = new MemberRepository();
        var fromDb = repository.GetByAlias(_members[1].Alias);
        fromDb.Email = "Yellow Pear";
        repository.Update(fromDb);

        // use session to try to load the product
        using (ISession session = _sessionFactory.OpenSession())
        {
            var fromDb2 = session.Get<Member>(fromDb.Id);
            Assert.AreEqual(fromDb.Email, fromDb2.Email);
        }
    }

    [TestMethod]
    public void Can_remove_existing_member()
    {
        IMemberRepository repository = new MemberRepository();
        var member = repository.GetByAlias(_members[0].Alias);
        repository.Remove(member);

        using (ISession session = _sessionFactory.OpenSession())
        {
            var fromDb = session.Get<Member>(member.Id);
            Assert.IsNull(fromDb);
        }
    }

    [TestMethod]
    public void Can_get_existing_member_by_id()
    {
        IMemberRepository repository = new MemberRepository();
        var member = repository.GetByAlias(_members[2].Alias);
        var fromDb = repository.GetById(member.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreNotSame(_members[2], fromDb);
        Assert.AreEqual(_members[2].Email, fromDb.Email);
    }

    [TestMethod]
    public void Can_get_existing_member_by_alias()
    {
        IMemberRepository repository = new MemberRepository();
        var fromDb = repository.GetByAlias(_members[1].Alias);
        Assert.IsNotNull(fromDb);
        Assert.AreNotSame(_members[1], fromDb);
        Assert.AreEqual(_members[1].Alias, fromDb.Alias);
    }

}