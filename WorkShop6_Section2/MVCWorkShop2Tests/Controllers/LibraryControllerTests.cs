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


namespace MVCWorkShop2.Controllers.Tests
{
    [TestClass]
    public class LibraryControllerTests
    {
     

        [TestMethod]
        public void IndexTest()
        {
            var result = new LBSearchArg();
            result.BookKeeper = "張三";
            var controller = new LibraryController();
            var testRes = controller.Search(result);
            Assert.AreEqual(1, testRes);
        }
    }
}