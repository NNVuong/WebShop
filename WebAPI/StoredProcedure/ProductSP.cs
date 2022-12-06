using System;
namespace WebAPI.StoredProcedure
{
    public class ProductSP
    {
        public const string SP_Add_Product = "SP_Add_Product @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12";
        public const string SP_Update_Product = "SP_Update_Product @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12";
        public const string SP_Delete_Product = "SP_Delete_Product @p0";
        public const string SP_FindProduct = "SP_FindProduct @p0,@p1,@p2,@p3,@p4";
        public const string SP_Count_Product = "SP_Count_Product @p0 OUT";
        public const string SP_Get_Product_By_Id = "SP_Get_Product_By_Id @p0";
        public const string SP_Get_All = "SP_Get_Product_By_Id";
        public const string SP_Get_Product_By_Category = "SP_Get_Product_By_Category";
    }
}
