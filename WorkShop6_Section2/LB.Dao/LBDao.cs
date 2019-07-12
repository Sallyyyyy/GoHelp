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
        public List<LBBooks> GetLibraryData(LBSearchArg viewresult)
        {
            DataTable dt = new DataTable();
            string sql = @"Select BOOK_ID,BOOK_CLASS_NAME,BOOK_NAME,BOOK_BOUGHT_DATE,CODE_NAME,USER_CNAME 
                                    FROM dbo.BOOK_DATA as e
                                    LEFT JOIN dbo.BOOK_CLASS as bc
                                    ON (e.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
                                    LEFT JOIN dbo.BOOK_CODE as code
                                    ON (e.BOOK_STATUS = code.CODE_ID)
                                    LEFT JOIN dbo.MEMBER_M as mm
                                    ON (e.BOOK_KEEPER = mm.USER_ID)
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
        //查詢書籍
        public List<LBBooks> SearchBook(LBSearchArg viewresult)
        {
            DataTable dt = new DataTable();
            string sql = @"Select BOOK_ID,BOOK_CLASS_NAME,BOOK_NAME,BOOK_BOUGHT_DATE,CODE_NAME,USER_CNAME 
                                    FROM dbo.BOOK_DATA as e
                                    LEFT JOIN dbo.BOOK_CLASS as bc
                                    ON (e.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
                                    LEFT JOIN dbo.BOOK_CODE as code
                                    ON (e.BOOK_STATUS = code.CODE_ID)
                                    LEFT JOIN dbo.MEMBER_M as mm
                                    ON (e.BOOK_KEEPER = mm.USER_ID)
                                    Where (e.BOOK_NAME LIKE ('%'+@BOOK_NAME+'%') OR @BOOK_NAME='')
                                    AND (bc.BOOK_CLASS_NAME LIKE ('%'+@BOOK_CLASS_NAME+'%') OR @BOOK_CLASS_NAME='')
                                    AND (mm.USER_CNAME LIKE ('%'+@BOOK_KEEPER+'%') OR @BOOK_KEEPER='')
                                    AND (code.CODE_ID LIKE ('%'+@BOOK_STATUS+'%') OR @BOOK_STATUS='')";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", viewresult.BookName == null ? string.Empty : viewresult.BookName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_NAME", viewresult.BookClassName == null ? string.Empty : viewresult.BookClassName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_KEEPER", viewresult.BookKeeper == null ? string.Empty : viewresult.BookKeeper));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", viewresult.BookStatus == null ? string.Empty : viewresult.BookStatus));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }
        //新增書籍
        public int InsertBook(LBSearchArg viewresult)
        {
            string sql = @" INSERT INTO dbo.BOOK_DATA
						 (
							 BOOK_NAME,BOOK_AUTHOR,BOOK_PUBLISHER,BOOK_NOTE
                            ,BOOK_BOUGHT_DATE,BOOK_CLASS_ID,BOOK_STATUS,BOOK_KEEPER
						 )
						VALUES
						(
							 @BOOK_NAME,@BOOK_AUTHOR,@BOOK_PUBLISHER,@BOOK_NOTE
                            ,@BOOK_BOUGHT_DATE,@BOOK_CLASS_ID,@BOOK_STATUS,@BOOK_KEEPER
						)";
            int Id;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", viewresult.BookName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", viewresult.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", viewresult.Pubilsher));
                cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", viewresult.BookIntroduce));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", viewresult.BoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", viewresult.BookClassName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", viewresult.BookStatus));
                cmd.Parameters.Add(new SqlParameter("@BOOK_KEEPER", viewresult.BookKeeper));
                Id = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return Id;
        }
        //刪除書籍
        public int DeleteBook(string viewresult)
        {
            string sql = @" DELETE 
                                        FROM dbo.BOOK_DATA
                                        WHERE dbo.BOOK_DATA.BOOK_ID = @BOOK_ID";
            int Id;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", viewresult));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                Id = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return Id;
        }
        //修改書籍明細
        public List<LBBooks> UpdateDetail(string data)
        {
            DataTable dt = new DataTable();
            string sql = @"
                                    Select BOOK_ID,BOOK_AUTHOR,BOOK_NAME,BOOK_BOUGHT_DATE,
                                                BOOK_PUBLISHER,BOOK_NOTE,CODE_ID,BOOK_CLASS_NAME
                                    FROM dbo.BOOK_DATA as e
                                    LEFT JOIN dbo.BOOK_CLASS as bc
                                    ON (e.BOOK_CLASS_ID = bc.BOOK_CLASS_ID)
                                    LEFT JOIN dbo.BOOK_CODE as code
                                    ON (e.BOOK_STATUS = code.CODE_ID)
                                    LEFT JOIN dbo.MEMBER_M as mm
                                    ON (e.BOOK_KEEPER = mm.USER_ID)
									WHERE CODE_TYPE = 'BOOK_STATUS'
                                    AND BOOK_ID=@BOOK_ID
                                    ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", data));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.UpdateDetailToList(dt);
        }
        //修改
        public int Update(LBSearchArg viewresult)
        {
            string sql = @" 
                                UPDATE  dbo.BOOK_DATA
                                SET
							    BOOK_NAME=@BOOK_NAME,
                                BOOK_AUTHOR=@BOOK_AUTHOR,
                                BOOK_PUBLISHER=@BOOK_PUBLISHER,
                                BOOK_NOTE=@BOOK_NOTE,
                                BOOK_BOUGHT_DATE=@BOOK_BOUGHT_DATE,
                                BOOK_CLASS_ID=@BOOK_CLASS_ID,
                                BOOK_STATUS=@BOOK_STATUS";
            int Id;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", viewresult.BookName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", viewresult.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", viewresult.Pubilsher));
                cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", viewresult.BookIntroduce));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", viewresult.BoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", viewresult.BookClassName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", viewresult.BookStatus));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                Id = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return Id;
        }
        //取得下拉式資料
        //類別下拉式
        public List<LBBooks> BookClassDrop()
        {
            DataTable dt = new DataTable();
            string sql = @"Select BOOK_CLASS_NAME,BOOK_CLASS_ID
                                FROM dbo.BOOK_CLASS";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookClassToList(dt);
        }
        //借閱狀態下拉式
        public List<LBBooks> BookStatusDrop()
        {
            DataTable dt = new DataTable();
            string sql = @"Select BOOK_CODE.CODE_NAME,BOOK_CODE.CODE_ID
                                FROM dbo.BOOK_CODE";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookStatusToList(dt);
        }
        //借閱人下拉式
        public List<LBBooks> BookKeeperDrop()
        {
            DataTable dt = new DataTable();
            string sql = @"Select MEMBER_M.USER_ID,MEMBER_M.USER_CNAME
                                    FROM dbo.MEMBER_M";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookKeeperToList(dt);
        }

        //轉換DataTable TO List 型態
        //轉換借閱人名稱
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
        //轉換狀態名稱
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
        //轉換類別名稱
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
        //轉換BookData
        private List<LBBooks> MapBookDataToList(DataTable Data)
        {
            List<LBBooks> result = new List<LBBooks>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new LBBooks()
                {
                    BookId = row["BOOK_ID"].ToString(),
                    BookClassName = row["BOOK_CLASS_NAME"].ToString(),
                    BookName = row["BOOK_NAME"].ToString(),
                    BoughtDate = row["BOOK_BOUGHT_DATE"].ToString(),
                    BookStatus = row["CODE_NAME"].ToString(),
                    BookKeeper = row["USER_CNAME"].ToString()
                });
            }
            return result;
        }
        private List<LBBooks> UpdateDetailToList(DataTable Data)
        {
            List<LBBooks> result = new List<LBBooks>();
            foreach (DataRow row in Data.Rows)
            {
                result.Add(new LBBooks()
                {
                    BookId = row["BOOK_ID"].ToString(),
                    BookClassName = row["BOOK_CLASS_NAME"].ToString(),
                    BookName = row["BOOK_NAME"].ToString(),
                    BoughtDate = row["BOOK_BOUGHT_DATE"].ToString(),
                    BookStatus = row["CODE_ID"].ToString(),
                    BookAuthor = row["BOOK_AUTHOR"].ToString(),
                    BookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    BookNote = row["BOOK_NOTE"].ToString()
                });
            }
            return result;
        }

        
    }
}
