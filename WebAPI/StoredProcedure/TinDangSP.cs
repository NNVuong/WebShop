using System;
namespace WebAPI.StoredProcedure
{
    public class TinDangSP
    {
        public const string SP_Add_TinDang = "SP_Add_TinDang @p0,@p1,@p2,@p3,@p4,@p5";
        public const string SP_Update_TinDang = "SP_Update_TinDang @p0,@p1,@p2,@p3,@p4,@p5,@p6";
        public const string SP_Get_All_TinDang = "SP_Get_All_TinDang";
        public const string SP_Delete_TinDang = "SP_Delete_TinDang @p0";
    }
}
