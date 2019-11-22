using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Financial_Portal.Helpers
{
    public class AvatarHelper
    {
        public static List<string> ModalAvatarList()
        {
            var avatarList = new List<string>();
            var avatarPath = HttpContext.Current.Server.MapPath("~/Avatars/");
            foreach (var file in Directory.GetFiles(avatarPath, "*.png"))
            {
                avatarList.Add($"/Avatars/{Path.GetFileName(file)}");
            }

            return avatarList;
        }
    }
}