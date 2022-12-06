using System;
namespace WebAPI.StoredProcedure
{
    public class NewSP
    {
        public const string SP_Add_New = "SP_Add_New @p0,@p1,@p2,@p3,@p4,@p5";
        public const string SP_Get_New = "SP_Get_New";
        public const string SP_Newfeed = "SP_Get_New @p0,@p1";
        public const string SP_Get_New_By_Id = "SP_Get_New @p0";
        public const string SP_Update_New = "SP_Update_New @p0,@p1,@p2,@p3,@p4,@p5";
        public const string SP_Delete_New = "SP_Delete_New @p0";
    }
}
