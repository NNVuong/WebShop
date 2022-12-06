using System;
namespace WebAPI.StoredProcedure
{
    public class OrderSP
    {
        public const string SP_Add_OrderDetail = "SP_Add_OrderDetail @p0,@p1,@p2,@p3,@p4";
        public const string SP_Get_Order_By_User = "SP_Get_Order_By_User @p0";
        public const string SP_Get_Order = "SP_Get_Order @p0,@p1";
        public const string SP_Get_List_OrderDetail = "SP_Get_List_OrderDetail @p0";
        public const string SP_Get_All = "SP_Get_Order_By_User";
        public const string SP_Get_Order_By_Id = "SP_Get_Order_By_Id @p0";
        public const string SP_Get_TransacStatus = "SP_Get_TransacStatus";
    }
}
