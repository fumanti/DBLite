using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFM.DBLite.Test
{
    using SFM.DBLite;
    using SFM.DBLite.Interfaces;
    using SFM.DBLite.Model;

    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void IntegrationTest()
        {
            //delete_repository();
            create_repository();
            insert_repository();
            select_repository();
            update_repository();
            delete_repository();
            Assert.IsTrue(1 == 1);
        }

        [TestMethod]
        public void create_repository()
        {
            IGenericRepository repo = new GenericRepository();
        }

        [TestMethod]
        public void insert_repository()
        {
            IGenericRepository repo = new GenericRepository();
            Allegato a = new Allegato { Progressivo = 1, ProgressivoAllegato = 1, NomeFile = "Prova Repository" };
            int result = repo.Insert(a);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void select_repository()
        {
            IGenericRepository repo = new GenericRepository();
            Allegato a = repo.Select<Allegato>(1);
            Assert.IsTrue(a.ProgressivoAllegato > 0);
        }

        [TestMethod]
        public void update_repository()
        {
            IGenericRepository repo = new GenericRepository();
            var allegato = new Allegato() { Progressivo = 1, ProgressivoAllegato = 1, NomeFile = "Prova update" };
            int result = repo.Update(allegato);
            Assert.IsTrue(result > 0);
        }

        //public void SelectAllRepository()
        //{
        //    IGenericRepository pRep = new PeopleRepository();
        //    foreach (People p in pRep.SelectAll<People>())
        //    {
        //        Console.WriteLine("Selected Id {0}, Name {1} ", p.PeopleId, p.Name);
        //    }
        //}

        [TestMethod]
        public void delete_repository()
        {
            IGenericRepository repo = new GenericRepository();
            bool result = repo.Delete(new Allegato() { ProgressivoAllegato = 1 });
            Assert.IsTrue(result);
        }

    }
}
