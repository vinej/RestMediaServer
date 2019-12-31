using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System;
using System.Collections.Generic;


[TestClass]
public class MemberTest
{
    private static MemberDal _dal;

    private static readonly Member[] _members = new[]
                 {
                     new Member { Email = "Melon", Alias = "Fruits1", IsActive = true, Dob = DateTime.Now },
                     new Member { Email = "Pear", Alias = "Fruits2", IsActive = true, Dob = DateTime.Now},
                     new Member { Email = "Milk", Alias = "Beverages1", IsActive = false, Dob = DateTime.Now},
                     new Member { Email = "Coca Cola", Alias = "Beverages2", IsActive = false, Dob = DateTime.Now},
                     new Member { Email = "Pepsi Cola", Alias = "Beverages3", IsActive = false, Dob = DateTime.Now},
                 };

    /*
    private bool IsInCollection(Member member, ICollection<Member> fromDb)
    {
        foreach (var item in fromDb)
            if (member.Id == item.Id)
                return true;
        return false;
    }
    */

    private static void CreateInitialData()
    {
        foreach (var member in _members) {
            _ = _dal.Insert(member);
        }
    }

    [ClassInitialize]
    public static void TestFixtureSetUp(TestContext context)
    {
        _ = context;
        _dal = new MemberDal();
        CreateInitialData();
    }

    [ClassCleanup]
    public static void TestFixtureCleanUp()
    {
        foreach (Member member in _members)
        {
            _ = _dal.Delete(member.Id);
        }
    }

    [TestMethod]
    public void Can_add_new_member()
    {
        var member = new Member { Email = "Apple44", Alias = "Fruits44", IsActive = true, Dob = DateTime.Now };
        _ = _dal.Insert(member);

        var fromDb = _dal.GetById(member.Id);
        // Test that the product was successfully inserted
        Assert.IsNotNull(fromDb);
        Assert.AreNotSame(member, fromDb);
        Assert.AreEqual(member.Email, fromDb.Email);
        Assert.AreEqual(member.Alias, fromDb.Alias);
    }

    [TestMethod]
    public void Can_update_existing_member()
    {
        var fromDb = _dal.GetByAlias("Fruits2");
        fromDb.Email = "Yellow Pear";
        _ = _dal.Update(fromDb);
        var fromDb2 = _dal.GetById(fromDb.Id);
        Assert.AreEqual(fromDb.Email, fromDb2.Email);
    }

    [TestMethod]
    public void Can_remove_existing_member()
    {
        var member = _dal.GetByAlias("Fruits44");
        _ = _dal.Delete(member.Id);
        var fromDb = _dal.GetById(member.Id);
        Assert.AreEqual(fromDb.Id, -1);
    }

    [TestMethod]
    public void Can_get_existing_member_by_id()
    {
        var member =  _dal.GetByAlias(_members[2].Alias);
        var fromDb = _dal.GetById(member.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual(_members[2].Email, fromDb.Email);
    }

    [TestMethod]
    public void Can_get_existing_member_by_alias()
    {
        var fromDb = _dal.GetByAlias(_members[1].Alias);
        Assert.IsNotNull(fromDb);
        Assert.AreNotSame(_members[1], fromDb);
        Assert.AreEqual(_members[1].Alias, fromDb.Alias);
    }

    /*
    [TestMethod]
    public void Can_add_friend()
    {
        IMemberRepository repository = new MemberRepository();
        var from = repository.GetByAlias(_members[1].Alias);
        var friend = repository.GetByAlias(_members[0].Alias);
        repository.AddFriend(from, friend);
        Assert.IsNotNull(from);
    }
    */
}