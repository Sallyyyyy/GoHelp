using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCWorkShop2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LB.Model;
using LB.Service;
using FluentAssertions;
using Moq;

namespace MVCWorkShop2.Controllers.Tests
{
    [TestClass]
    public class LibraryControllerTests
    {
        private ILBService lbService { get; set; }
        [TestMethod]
        public void IndexTest()
        {
            LBSearchArg viewresult = new LBSearchArg();
            viewresult.BookName = "789";
            Mock<ILBService> mockService = new Mock<ILBService>();
            mockService.Setup(x => x.InsertBook(viewresult)).Returns(1);
            var controller = new LibraryController();
            controller.Insert(viewresult);


        }
    }
}