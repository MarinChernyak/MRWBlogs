using Authentication.Migrations;
using MRWBlobs_DAL.Entities;
using SMAuthentication.SMGeneralEntities;

namespace MRWBlogs.Models.Blogs
{
    public class BlogSummaryModel:BlogBaseModel
    {
        public string? Keywords { get; set; }
        public int NumberMessages { get; set; }
        public string OwnerName { get; set; } = "Unknown";

        public BlogSummaryModel(int id)
        {
            Id = id;
            InitModel();
        }
        protected void InitModel()
        {
            using MRWBlogsContext context = new(GetContextOptions());
            var blog = context.Blogs.FirstOrDefault(x=>x.Id==Id);
            int OwnerId = 0;
            if (blog != null)
            {
                Keywords = blog.Keywords;
                Title = blog.Title;
                AvatarUrl = blog.Avatar??string.Empty;
                CreatedAt = blog.CreatedAt;
                OwnerId = blog.UserId;
                NumberMessages = context.BlogsMessages.Where(x => x.BlogId == Id).ToList().Count;                
            }
            if (OwnerId > 0)
            {
                using SMGeneralContext smcontext = new();
                var user = smcontext.Users.FirstOrDefault(x => x.Id == OwnerId);
                if (user != null)
                {
                    string temp = $"{user.FirstName} {user.LastName}";
                    if(!string.IsNullOrEmpty(temp))
                    {
                        OwnerName = temp ;
                    }
                }
            };
        }
    }
}
