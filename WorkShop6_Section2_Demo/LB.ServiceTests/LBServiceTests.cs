using Microsoft.VisualStudio.TestTools.UnitTesting;
using LB.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LB.Model;
using LB.Dao;
using LB.Service;
using Moq;
using FluentAssertions;

namespace LB.Service.Tests
{

    [TestClass()]
    public class LBServiceTests
    {
        [TestMethod()]
        public void SearchBookTest()
        {
            //Arrange
            //假設使用者查詢書名"456"
            LBSearchArg viewresult = new LBSearchArg();
            viewresult.BookName = "456";
            LBService service = new LBService();
            service.lbDao = new TestDao();
            //Act
            var testRes = service.SearchBook(viewresult);
            //Assert
            //和丟入的書名相同
            Assert.AreEqual("456", testRes[0].BookName);
        }

        [TestMethod()]
        public void GetLibraryDataTest()
        {
            LBSearchArg viewresult = new LBSearchArg();
            LBService service = new LBService();
            service.lbDao = new TestDao();
            var testRes = service.GetLibraryData(viewresult);
            //因為建構假Dao時回傳的dt為兩筆，因此count=6時表示成功
            Assert.AreEqual(6, testRes.Count());
        }

        [TestMethod()]
        public void BookClassDropTest()
        {
            LBService service = new LBService();
            service.lbDao = new TestDao();
            var testRes = service.BookClassDrop();
            //因為建構假Dao時回傳的dt為兩筆，因此count=2時表示成功
            Assert.AreEqual(2, testRes.Count());
        }

        [TestMethod()]
        public void BookStatusDropTest()
        {
            LBService service = new LBService();
            service.lbDao = new TestDao();
            var testRes = service.BookStatusDrop();
            //因為建構假Dao時回傳的dt為兩筆，因此count=2時表示成功
            Assert.AreEqual(2, testRes.Count());
        }

        [TestMethod()]
        public void BookKeeperDropTest()
        {
            //Arrange
            LBService service = new LBService();
            service.lbDao = new TestDao();
            //Act
            var testRes = service.BookKeeperDrop();
            //Assert
            //因為建構假Dao時回傳的dt為兩筆，因此count=2時表示成功
            Assert.AreEqual(2, testRes.Count());
        }

        [TestMethod()]
        public void InsertBookTest()
        {
            //Arrange
            //假設使用者新增書名"789"
            LBSearchArg viewresult = new LBSearchArg();
            viewresult.BookName = "789";
            LBService service = new LBService();
            service.lbDao = new TestDao();
            //Act
            var testRes = service.InsertBook(viewresult);
            //Assert
            //新增一筆
            Assert.AreEqual(1, testRes);
        }

        [TestMethod()]
        public void InsertBookTest_Moq()
        {
            LBSearchArg viewresult = new LBSearchArg();
            viewresult.BookName = "789";
            Mock<ILBService> mockService = new Mock<ILBService>();
            mockService.Setup(x => x.InsertBook(viewresult)).Returns(1);
            //var service = new LBService();
            //service.Should().NotBeNull();
            //Assert.Fail();
        }
    }
}