using System.Collections.Generic;
using LB.Model;

namespace LB.Dao
{
    public interface ILBDao
    {
        List<LBBooks> BookClassDrop();
        List<LBBooks> BookKeeperDrop();
        List<LBBooks> BookStatusDrop();
        List<LBBooks> GetLibraryData(LBSearchArg viewresult);
        int InsertBook(LBSearchArg viewresult);
        List<LBBooks> SearchBook(LBSearchArg viewresult);
    }
}