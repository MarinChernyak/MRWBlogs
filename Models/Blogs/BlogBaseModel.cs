using MRWBlobs_DAL.Entities;
using MRWBlogs.Models;
using SMAuthentication.Factories;
using SMAuthentication.Models;

namespace MRWBlogs.Models.Blogs
{
    public class BlogBaseModel:BaseModel 
    {
        public int Id { get; protected set; }
        public string Title { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; protected set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; protected set; } = DateTime.Now;
        public int IdOwner { get; protected set; }

        public async Task<bool> IsBlogExisting(string token)
        {
            bool result = false;
            MUser? user = await UsersFactoryHelpers.CheckToken(token, Constants.AppId);
            if(user!=null)
            {
                using MRWBlogsContext context = new(GetContextOptions());
                var blog = context.Blogs.FirstOrDefault(b => b.UserId == user.Id);
                if (blog != null)
                {
                    result = true;
                }
                else
                {
                    IdOwner = user.Id;
                }
            }
                return result;
        }
    }
}
