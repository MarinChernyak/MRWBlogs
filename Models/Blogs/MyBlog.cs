using MRWBlobs_DAL.Entities;

namespace MRWBlogs.Models.Blogs
{
    public class MyBlog: BaseModel
    {
        public int BlogId { get; protected set; }
        public List<BlogMessage> Articles { get; set; } = new();
        public MyBlog(int userId)
        {

        }
        protected void InitMyBlog(int userId)
        {
            UpdateBlogId(userId);
            using MRWBlogsContext context = new(GetContextOptions());
            Articles = context.BlogsMessages.Where(a => a.BlogId == BlogId).ToList();
        }
        protected void UpdateBlogId(int userId)
        {
            using MRWBlogsContext context = new(GetContextOptions());
            var blog = context.Blogs.FirstOrDefault(b => b.UserId == userId);
            if (blog != null)
            {
                BlogId = blog.Id;
            }
        }
    }
}
