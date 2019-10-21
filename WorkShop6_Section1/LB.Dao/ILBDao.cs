using System.Collections.Generic;
using LB.Model;

namespace LB.Dao
{
    public interface ILBDao
    {
        //List<LBBooks> BookClassDrop();
        //List<LBBooks> BookKeeperDrop();
        //List<LBBooks> BookStatusDrop();
        List<GHPost> GetLibraryData(LBSearchArg viewresult);
        int Insert(LBSearchArg viewresult);
        //List<LBBooks> SearchBook(LBSearchArg viewresult);
    }
}