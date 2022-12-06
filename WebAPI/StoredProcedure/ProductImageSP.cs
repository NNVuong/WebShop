using System;
namespace WebAPI.StoredProcedure
{
    public class ProductImageSP
    {
        public const string SP_UpLoad_ProductImage = "SP_UpLoad_ProductImage @p0,@p1";
        public const string SP_Get_ProductImage = "SP_Get_ProductImage @p0";
        public const string SP_Delete_ProductImage = "SP_Delete_ProductImage @p0";
        public const string SP_Get_ProductImage_By_Id = "SP_Get_ProductImage_By_Id @p0";
    }
}
