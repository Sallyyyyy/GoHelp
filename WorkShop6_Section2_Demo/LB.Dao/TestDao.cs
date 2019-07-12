using LB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB.Dao
{
    public class TestDao:ILBDao
    {
        public List<LBBooks> GetLibraryData(LBSearchArg viewresult)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BOOK_NAME");
            dt.Columns.Add("BOOK_CLASS_NAME");
            dt.Columns.Add("BOOK_KEEPER");
            dt.Columns.Add("BOOK_STATUS");
            dt.Rows.Add("123", "生活類", "張三", "A");
            dt.Rows.Add("456", "旅遊類", "張三", "A");
            dt.Rows.Add("XXX", "生活類", "李四", "A");
            dt.Rows.Add("YYY", "旅遊類", "李四", "A");
            dt.Rows.Add("ZZZ", "生活類", "張三", "A");
            dt.Rows.Add("UUU", "旅遊類", "張三", "A");
            return this.MapBookDataToList(dt);
        }

        public int InsertBook(LBSearchArg viewresult)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BOOK_NAME");
            dt.Columns.Add("BOOK_CLASS_NAME");
            dt.Columns.Add("BOOK_KEEPER");
            dt.Columns.Add("BOOK_STATUS");
            dt.Rows.Add(viewresult.BookName, "旅遊類", "王五", "A");
            var list = this.MapBookDataToList(dt);
            return list.Count(); 
        }

        public List<LB.Model.LBBooks> SearchBook(LBSearchArg viewresult)
        {
            //IList<KeyValuePair<string, string>> Prms = new List<KeyValuePair<string, string>>();
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_NAME", string.Empty));
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_CLASS_NAME", string.Empty));
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_KEEPER", "張三"));
            //Prms.Add(new KeyValuePair<string, string>("@BOOK_STATUS", string.Empty));
            DataTable dt = new DataTable();
            dt.Columns.Add("BOOK_NAME");
            dt.Columns.Add("BOOK_CLASS_NAME");
            dt.Columns.Add("BOOK_KEEPER");
            dt.Columns.Add("BOOK_STATUS");
            dt.Rows.Add(viewresult.BookName, "旅遊類", "王五", "A");

            return this.MapBookDataToList(dt); 
        }
        private List<LBBooks> MapBookDataToList(DataTable Data)
        {
            List<LBBooks> result = new List<LBBooks>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new LBBooks()
                {
                    BookClassName = row["BOOK_CLASS_NAME"].ToString(),
                    BookName = row["BOOK_NAME"].ToString(),
                    BookStatus = row["BOOK_KEEPER"].ToString(),
                    BookKeeper = row["BOOK_STATUS"].ToString()
                });
            }
            return result;
        }
        //下拉式選單
        public List<LBBooks> BookClassDrop()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BOOK_CLASS_NAME");
            dt.Columns.Add("BOOK_CLASS_ID");
            dt.Rows.Add("旅遊類","Travel");
            dt.Rows.Add("生活類", "Life");
            return this.MapBookClassToList(dt);
            throw new NotImplementedException();
        }

        public List<LBBooks> BookKeeperDrop()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("USER_ID");
            dt.Columns.Add("USER_CNAME");
            dt.Rows.Add("1", "張三");
            dt.Rows.Add("2", "李四");
            return this.MapBookKeeperToList(dt);
            throw new NotImplementedException();
        }

        public List<LBBooks> BookStatusDrop()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CODE_ID");
            dt.Columns.Add("CODE_NAME");
            dt.Rows.Add("A","可以借出");
            dt.Rows.Add("B", "不可借出");
            return this.MapBookStatusToList(dt);
            throw new NotImplementedException();
        }
        //轉換狀態名稱TOLIST
        private List<LBBooks> MapBookStatusToList(DataTable bookClass)
        {
            List<LBBooks> result = new List<LBBooks>();
            foreach (DataRow row in bookClass.Rows)
            {
                result.Add(new LBBooks()
                {
                    BookStatus = row["CODE_ID"].ToString(),
                    BookStatusName = row["CODE_NAME"].ToString()
                });
            }
            return result;
        }
        //轉換借閱人名稱TOLIST
        private List<LBBooks> MapBookKeeperToList(DataTable bookClass)
        {
            List<LBBooks> result = new List<LBBooks>();
            foreach (DataRow row in bookClass.Rows)
            {
                result.Add(new LBBooks()
                {
                    BookKeeper = row["USER_ID"].ToString(),
                    BookKeeperName = row["USER_CNAME"].ToString()
                });
            }
            return result;
        }
        //轉換類別名稱TOLIST
        private List<LBBooks> MapBookClassToList(DataTable bookClass)
        {
            List<LBBooks> result = new List<LBBooks>();
            foreach (DataRow row in bookClass.Rows)
            {
                result.Add(new LBBooks()
                {
                    BookClassName = row["BOOK_CLASS_NAME"].ToString(),
                    BookClassId = row["BOOK_CLASS_ID"].ToString()
                });
            }
            return result;
        }

        public int DeleteBook(string viewresult)
        {
            throw new NotImplementedException();
        }

        public List<LBBooks> UpdateDetail(string data)
        {
            throw new NotImplementedException();
        }

        public int Update(LBSearchArg viewresult)
        {
            throw new NotImplementedException();
        }
    }
}
