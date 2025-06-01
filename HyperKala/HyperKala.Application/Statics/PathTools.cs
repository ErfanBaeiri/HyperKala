namespace HyperKala.Application.Statics
{
    public static class PathTools
    {
        // Path for user avatar
        public static string UserAvatarUploadPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/userAvatar/Current");
        public static string UserAvatarReadPath = "/userAvatar/Current";

        // Path for thumb user avatar
        public static string UserThumbAvatarUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userAvatar/thumb");
        public static string UserThumbAvatarReadPath = "/userAvatar/thumb";


        #region product categories

        public static string CategoryOrgin = "/img/category/orgin/";
        public static string CategoryOrginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/category/orgin/");

        public static string CategoryThumb = "/img/category/thumb/";
        public static string CategoryThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/category/thumb/");

        #endregion
    }
}
