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
            string sql = @"select Posts.rowid,Nickname,PostTitle,Kind,MeetAddress,StartTime,EndTime,PostContent,UserID,HelpUserID,Status,PostLat,PostLong,City
                                    from USERS
                                    inner join Posts  on  USERS.User_Account = Posts.UserID
                                    where Status='unfinish'
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
                    PostContent = row["PostContent"].ToString(),
                    PostID = (int)row["rowid"],
                    Nickname = row["Nickname"].ToString(),
                    PostTitle = row["PostTitle"].ToString(),
                    Kind = row["Kind"].ToString(),
                    MeetAddress = row["MeetAddress"].ToString(),
                    StartTime = row["StartTime"].ToString(),
                    EndTime = row["EndTime"].ToString(),
                    UserID = row["UserID"].ToString(),
                    Status = row["Status"].ToString(),
                    HelpUserID = row["HelpUserID"].ToString(),
                    PostLat = System.Convert.ToSingle(row["PostLat"]),
                    PostLong = System.Convert.ToSingle(row["PostLong"]),
                    City = row["City"].ToString()
                });
            }
            return result;
        }

        // 問題分類轉換顯示
        public List<GHPost> GetTypeArgData(string type)
        {
            DataTable dt = new DataTable();
            string sql = @"select Posts.rowid,Nickname,PostTitle,Kind,MeetAddress,StartTime,EndTime,PostContent,UserID,HelpUserID,Status,PostLat,PostLong,City
                                    from USERS
                                    inner join Posts  on  USERS.User_Account = Posts.UserID
                                    where Status='unfinish'
                                    and Kind=@type
                                    ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@type", type));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }

        //新增貼文
        public int Insert(LBSearchArg viewresult)
        {
            string sql = @"INSERT INTO dbo.Posts(
                                     PostTitle,PostContent,Kind,MeetAddress,StartTime,EndTime,UserID,Status,PostLat,PostLong,City)
                                VALUES(
        	                         @PostTitle,@PostContent,@Kind,@MeetAddress,@StartTime,@EndTime,@UserID,@Status,@PostLat,@PostLong,@City
                                )";
            int Id;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@UserID", viewresult.UserID));
                cmd.Parameters.Add(new SqlParameter("@PostTitle", viewresult.PostTitle));
                cmd.Parameters.Add(new SqlParameter("@PostContent", viewresult.PostContent));
                cmd.Parameters.Add(new SqlParameter("@Kind", viewresult.Kind));
                cmd.Parameters.Add(new SqlParameter("@MeetAddress", viewresult.MeetAddress));
                cmd.Parameters.Add(new SqlParameter("@StartTime", DateTime.Parse(viewresult.StartTime)));
                cmd.Parameters.Add(new SqlParameter("@EndTime", DateTime.Parse(viewresult.EndTime)));
                cmd.Parameters.Add(new SqlParameter("@PostLat", viewresult.PostLat));
                cmd.Parameters.Add(new SqlParameter("@PostLong", viewresult.PostLong));
                cmd.Parameters.Add(new SqlParameter("@City", viewresult.City));
                cmd.Parameters.Add(new SqlParameter("@Status", "unFinish"));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                Id = Convert.ToInt32(cmd.ExecuteNonQuery());
                conn.Close();
            }
            return Id;
        }
        //註冊
        public int SignUp(User viewresult)
        {
            string sql = @"INSERT INTO dbo.USERS(
                                     User_Account,User_Password,Nickname,Phone,Gender,Email)
                                VALUES(
        	                         @User_Account,@User_Password,@Nickname,@Phone,@Gender,@Email
                                )";
            int Id;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@User_Account", viewresult.User_Account));
                cmd.Parameters.Add(new SqlParameter("@User_Password", viewresult.User_Password));
                cmd.Parameters.Add(new SqlParameter("@Nickname", viewresult.Nickname));
                cmd.Parameters.Add(new SqlParameter("@Phone", viewresult.Phone));
                cmd.Parameters.Add(new SqlParameter("@Gender", viewresult.Gender));
                cmd.Parameters.Add(new SqlParameter("@Email", viewresult.Email));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                Id = Convert.ToInt32(cmd.ExecuteNonQuery());
                conn.Close();
            }
            return Id;
        }

        public Boolean Login(Login viewresult)
        {
            int Id;
            string sql = @"Select 1
                                    From USERS
                                    Where User_Account = @User_Account
                                    and User_Password = @User_Password
                                    ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@User_Account", viewresult.Account));
                cmd.Parameters.Add(new SqlParameter("@User_Password", viewresult.Passwd));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                Id = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            if (Id == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // 救助紀錄
        public List<GHHistory> GetHistoryData(string userId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  PostTitle,Kind,PostContent,MeetAddress,Status,City
                                    FROM  USERS
                                    inner join Posts  on  USERS.User_Account = Posts.UserID 
                                    WHERE User_Account = @userId
                                    ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapHistoryDataToList(dt);
        }

        private List<GHHistory> MapHistoryDataToList(DataTable Data)
        {
            List<GHHistory> result = new List<GHHistory>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new GHHistory()
                {
                    PostTitle = row["PostTitle"].ToString(),
                    Kind = row["Kind"].ToString(),
                    PostContent = row["PostContent"].ToString(),
                    MeetAddress = row["MeetAddress"].ToString(),
                    Status = row["Status"].ToString(),
                    City = row["City"].ToString()
                });
            }
            return result;
        }


        // 助人事蹟
        public List<GHMyHelp> GetMyHelpData(string userId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  PostTitle,Kind,PostContent,MeetAddress,Status,EndTime,City
                                    FROM  USERS
                                    inner join Posts  on  USERS.User_Account = Posts.UserID 
                                    WHERE HelpUserID = @userId
                                    ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapMyHelpDataToList(dt);
        }

        private List<GHMyHelp> MapMyHelpDataToList(DataTable Data)
        {
            List<GHMyHelp> result = new List<GHMyHelp>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new GHMyHelp()
                {
                    PostTitle = row["PostTitle"].ToString(),
                    Kind = row["Kind"].ToString(),
                    PostContent = row["PostContent"].ToString(),
                    EndTime = row["EndTime"].ToString(),
                });
            }
            return result;
        }

        // 個人資料
        public List<GHUdata> GetUdata(string userId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT  User_Account,User_Password,Nickname,Phone,Gender,Email,Post_Cnt,Score
                                    FROM  USERS
                                    WHERE User_Account = @userId
                                    ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapGetUdataToList(dt);
        }

        private List<GHUdata> MapGetUdataToList(DataTable Data)
        {
            List<GHUdata> result = new List<GHUdata>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new GHUdata()
                {
                    UserID = row["User_Account"].ToString(),
                    User_Password = row["User_Password"].ToString(),
                    Nickname = row["Nickname"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Gender = row["Gender"].ToString(),
                    Email = row["Email"].ToString(),
                    Post_Cnt = row["Post_Cnt"].ToString(),
                    Score = row["Score"].ToString()
                });
            }
            return result;
        }

        // 排行榜
        public List<GHRank> GetRankData()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT COUNT(a2.Score) Rank, a1.Nickname,a1.Gender,a1.Score
                                    FROM [dbo].[USERS] a1, [dbo].[USERS] a2
                                    WHERE a1.Score <= a2.Score OR (a1.Score=a2.Score AND a1.Nickname = a2.Nickname)
                                    GROUP BY a1.Nickname, a1.Score,a1.Gender,a1.Score
                                    ORDER BY a1.Score DESC, a1.Nickname DESC
                                    ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MaptRankDataToList(dt);
        }

        private List<GHRank> MaptRankDataToList(DataTable Data)
        {
            List<GHRank> result = new List<GHRank>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new GHRank()
                {
                    Rank = row["Rank"].ToString(),
                    Nickname = row["Nickname"].ToString(),
                    Gender = row["Gender"].ToString(),
                    Score = row["Score"].ToString(),
                });
            }
            return result;
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
