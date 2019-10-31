using System;
using System.Collections.Generic;
using LB.Model;

namespace LB.Service
{
    public interface ILBService
    {
        //List<LBBooks> BookClassDrop();
        //List<LBBooks> BookKeeperDrop();
        //List<LBBooks> BookStatusDrop();
        List<GHPost> GetLibraryData(LBSearchArg viewresult);
        int Insert(LBSearchArg viewresult);
        int SignUp(User viewresult);
        Boolean Login(Login login);
        List<GHHistory> GetHistoryData(string userId);
        List<GHRank> GetRankData();
        List<GHMyHelp> GetMyHelpData(string userId);
        List<GHUdata> GetUdata(string userId);
        List<GHPost> GetTypeArgData(string type);
        //List<LBBooks> SearchBook(LBSearchArg viewresult);
    }
}