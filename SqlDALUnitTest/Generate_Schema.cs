using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SqlDAL.Domain;

namespace FirstSolution.Tests
{
    [TestClass]
    public class GenerateSchema_Fixture
    {
        [TestMethod]
        public void Can_generate_schema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Member).Assembly);

            new SchemaExport(cfg).Execute(true, true, false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }

    }
}