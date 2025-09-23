using System.Collections.Generic;
using Feature.Home.Data.Model;

namespace Feature.Home.Data.Mapper
{
    public static class HomeInfoMapper
    {
        public static HomeInfo ToModel(this Dictionary<string, object> map)
        {
            return new HomeInfo(
                map["imageUrl"].ToString(),
                map["searchString"].ToString(),
                int.Parse(map["count"].ToString())
            );
        }
    }
}