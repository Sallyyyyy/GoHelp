using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LB.Model;
using LB.Dao;
using LB.Common;



namespace LB.Dao
{
    public class LBDao : ILBDao
    {
        //取得連線字串(Web.config)
        private string GetDBConnectionString()
        {
            return
                LB.Common.ConfigTool.GetDBConnectionString("DBConn");
        }
        //載入畫面時GET書籍資料放到kendoGrid
        public List<GHPost> GetLibraryData(LBSearchArg viewresult)
        {
            DataTable dt = new DataTable();
            string sql = @"select Nickname,PostTitle,Kind,MeetAddress,StartTime
                                    from Member
                                    inner join Post  on  Member.UserID = Post.UserID
                                    ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }


        //將BookData轉換成List
        private List<GHPost> MapBookDataToList(DataTable Data)
        {
            List<GHPost> result = new List<GHPost>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new GHPost()
                {
                    Nickname = row["Nickname"].ToString(),
                    PostTitle = row["PostTitle"].ToString(),
                    Kind = row["Kind"].ToString(),
                    MeetAddress = row["MeetAddress"].ToString(),
                    StartTime = row["StartTime"].ToString()
                });
            }
            return result;
        }
        //新增書籍
        public int Insert(LBSearchArg viewresult)
        {
            string sql = @" INSERT INTO dbo.Post
         (
        	 BOOK_NAME,BOOK_AUTHOR,BOOK_PUBLISHER,BOOK_NOTE,BOOK_BOUGHT_DATE,BOOK_CLASS_ID,BOOK_STATUS
         )
        VALUES
        (
        	 @BOOK_NAME,@BOOK_AUTHOR,@BOOK_PUBLISHER,@BOOK_NOTE,@BOOK_BOUGHT_DATE,@BOOK_CLASS_ID,@BOOK_STATUS
        )";
            int Id;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@PostID", viewresult.PostID));
                cmd.Parameters.Add(new SqlParameter("@PostTitle", viewresult.PostTitle));
                cmd.Parameters.Add(new SqlParameter("@PostContent", viewresult.PostContent));
                cmd.Parameters.Add(new SqlParameter("@Kind", viewresult.Kind));
                cmd.Parameters.Add(new SqlParameter("@MeetAddress", viewresult.MeetAddress));
                cmd.Parameters.Add(new SqlParameter("@StartTime", viewresult.StartTime));
                cmd.Parameters.Add(new SqlParameter("@EndTime", viewresult.EndTime));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                Id = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return Id;
        }

        //  //查詢書籍
        //  public List<LB.Model.LBBooks> SearchBook(LBSearchArg viewresult)
        //  {
        //      DataTable dt = new DataTable();
        //      string sql = @"Select BOOK_CLASS_NAME,BOOK_NAME,BOOK_BOUGHT_DATE,CODE_NAME,USER_CNAME 
        //                              FROM dbo.BOOK_DATA as e
        //                              LEFT JOIN dbo.BOOK_CLASS as bc
        //                              ON (e.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
        //                              LEFT JOIN dbo.BOOK_CODE as code
        //                              ON (e.BOOK_STATUS = code.CODE_ID)
        //                              LEFT JOIN dbo.MEMBER_M as mm
        //                              ON (e.BOOK_KEEPER = mm.USER_ID)
        //                              Where (e.BOOK_NAME LIKE ('%'+@BOOK_NAME+'%') OR @BOOK_NAME='')
        //                              AND (bc.BOOK_CLASS_NAME LIKE ('%'+@BOOK_CLASS_NAME+'%') OR @BOOK_CLASS_NAME='')
        //                              AND (mm.USER_CNAME LIKE ('%'+@BOOK_KEEPER+'%') OR @BOOK_KEEPER='')
        //                              AND (code.CODE_ID LIKE ('%'+@BOOK_STATUS+'%') OR @BOOK_STATUS='')";
        //      using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
        //      {
        //          conn.Open();
        //          SqlCommand cmd = new SqlCommand(sql, conn);
        //          cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", viewresult.BookName == null ? string.Empty : viewresult.BookName));
        //          cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_NAME", viewresult.BookClassName == null ? string.Empty : viewresult.BookClassName));
        //          cmd.Parameters.Add(new SqlParameter("@BOOK_KEEPER", viewresult.BookKeeper == null ? string.Empty : viewresult.BookKeeper));
        //          cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", viewresult.BookStatus == null ? string.Empty : viewresult.BookStatus));
        //          SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
        //          sqlAdapter.Fill(dt);
        //          conn.Close();
        //      }
        //      return this.MapBookDataToList(dt);
        //  }

        //  //取得下拉式資料
        //  //類別名稱
        //  public List<LBBooks> BookClassDrop()
        //  {
        //      DataTable dt = new DataTable();
        //      string sql = @"Select BOOK_CLASS_NAME,BOOK_CLASS_ID
        //                          FROM dbo.BOOK_CLASS";
        //      using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
        //      {
        //          conn.Open();
        //          SqlCommand cmd = new SqlCommand(sql, conn);
        //          SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
        //          sqlAdapter.Fill(dt);
        //          conn.Close();
        //      }
        //      return this.MapBookClassToList(dt);
        //  }
        //  //借閱狀態下拉式
        //  public List<LBBooks> BookStatusDrop()
        //  {
        //      DataTable dt = new DataTable();
        //      string sql = @"Select BOOK_CODE.CODE_NAME,BOOK_CODE.CODE_ID
        //                          FROM dbo.BOOK_CODE";
        //      using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
        //      {
        //          conn.Open();
        //          SqlCommand cmd = new SqlCommand(sql, conn);
        //          SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
        //          sqlAdapter.Fill(dt);
        //          conn.Close();
        //      }
        //      return this.MapBookStatusToList(dt);
        //  }
        //  //借閱人下拉式
        //  public List<LBBooks> BookKeeperDrop()
        //  {
        //      DataTable dt = new DataTable();
        //      string sql = @"Select MEMBER_M.USER_ID,MEMBER_M.USER_CNAME
        //                              FROM dbo.MEMBER_M";
        //      using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
        //      {
        //          conn.Open();
        //          SqlCommand cmd = new SqlCommand(sql, conn);
        //          SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
        //          sqlAdapter.Fill(dt);
        //          conn.Close();
        //      }
        //      return this.MapBookKeeperToList(dt);
        //  }
        //轉換狀態名稱TOLIST
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


        
    }
}
