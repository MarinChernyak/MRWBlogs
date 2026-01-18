using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Identity.Client;
using MRWBlogs.Models.Shared;
using SMCommonUtilities;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Blogs
{
    public abstract class BlogActions : BlogBaseModel
    {
        public TagsManagerVM TagsManagerVM { get; set; }=new TagsManagerVM();
       
        protected override string GetImageFileName(IBrowserFile file)
        {
            string fname = file.Name;
            if (!string.IsNullOrEmpty(fname))
            {
                int dotIndex = fname.LastIndexOf('.');
                string ext = fname.Substring(dotIndex + 1).ToLower();
                fname = $"avatar_{IdOwner}.{ext}";
            }
            return fname;
        }
        public abstract Task<MessageContainer> DoBlogActionAsync(IWebHostEnvironment? env, IBrowserFile file, string token, string keywords);
        public string GetLabelForAvatarSelection()
        { 
            string labelout= "Select a picture for avatar";
            if (!string.IsNullOrEmpty(AvatarUrl))
            {
                labelout = "Change a picture for avatar"; 
            }
            return labelout;
        }
    }
}
