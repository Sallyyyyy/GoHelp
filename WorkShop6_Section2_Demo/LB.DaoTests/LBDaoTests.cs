using Microsoft.VisualStudio.TestTools.UnitTesting;
using LB.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Moq;
using LB.Model;

namespace LB.Dao.Tests
{
    [TestClass()]
    public class LBDaoTests
    {
        [TestMethod()]
        public void SearchBookTest()
        {
            //Arrange

            //IList<KeyValuePair<string, string>> Prms = new List<KeyValuePair<string, string>>();
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_NAME", string.Empty));
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_CLASS_NAME", string.Empty));
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_KEEPER", "張三"));
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_STATUS", string.Empty));
            //DataTable dt = new DataTable();
            //dt.Columns.Add("BOOK_NAME");
            //dt.Columns.Add("BOOK_CLASS_NAME");
            //dt.Columns.Add("BOOK_KEEPER");
            //dt.Columns.Add("BOOK_STATUS");
            //dt.Rows.Add("123", "生活類", "張三", "A");
            //var mock = new Mock<IDataAccessTool>();
            //mock.Setup(m => m.Query(It.IsAny<string>(), It.IsAny<string>(), Prms)).Returns(dt);
            
            Assert.Fail();
        }
    }
}
