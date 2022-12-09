using System;
namespace WebAPI.StoredProcedure
{
    public class SalesSP
    {
        public const string SP_Get_All = "SP_Get_Order_By_User_All";

        public const string SP_Get_All_By_Date = "SP_Get_Order_By_User_All @p0,@p1";
    }
}
