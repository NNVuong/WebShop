using System;
namespace WebAPI.StoredProcedure
{
    public class CategorySP
    {
        public const string SP_Add_Category = "SP_Add_Category @p0,@p1,@p2";
        public const string SP_Update_Category = "SP_Update_Category @p0,@p1,@p2,@p3";
        public const string SP_Get_Category_By_Id = "SP_Get_Category_By_Id @p0";
        public const string SP_Get_All = "SP_Get_Category_By_Id";
        public const string SP_Delete_Category = "SP_Delete_Category @p0";
    }
}
