using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LB.Model;
using LB.Dao;
using Spring.Context.Support;

namespace LB.Service
{
    public class LBService : ILBService
    {
        public ILBDao lbDao { get; set; }
        //載入畫面時GET書籍資料放到kendoGrid
        public List<LBBooks> GetLibraryData(LBSearchArg viewresult)
        {
            return lbDao.GetLibraryData(viewresult);
        }
        //查詢書籍
        public List<LBBooks> SearchBook(LBSearchArg viewresult)
        {
                return lbDao.SearchBook(viewresult);
        }
        //新增書籍
        public int InsertBook(LBSearchArg viewresult)
        {
            return lbDao.InsertBook(viewresult);
        }
        //刪除書籍
        public int DeleteBook(string viewresult)
        {
            return lbDao.DeleteBook(viewresult);
        }
        //修改書籍明細
        public List<LBBooks> UpdateDetail(string Data)
        {
            return lbDao.UpdateDetail(Data);
        }
        //修改
        public int Update(LBSearchArg viewresult)
        {
            return lbDao.Update(viewresult);
        }
        //取得下拉式資料
        //類別下拉式
        public List<LBBooks> BookClassDrop()
        {
            return lbDao.BookClassDrop();
        }
        //借閱狀態下拉式
        public List<LBBooks> BookStatusDrop()
        {
            return lbDao.BookStatusDrop();
        }
        //借閱人下拉式
        public List<LBBooks> BookKeeperDrop()
        {
            return lbDao.BookKeeperDrop();
        }

        
    }
}

