using MRWBlobs_DAL.Entities;

namespace MRWBlogs.Models.Blogs
{
    public class MyBlog: BaseModel
    {
        protected const int NumberSumMessagesonPage = 10;
        public int BlogId { get; protected set; }
        public List<BlogMessage> Articles { get; set; } = [];
        public int BlogOwnerId { get; protected set; }
        public int CurUserId { get; protected set; }
        public MyBlog(int blogOwnerId, int currentUserId)
        {
            BlogOwnerId= blogOwnerId;
            CurUserId= currentUserId;
            InitMyBlog();
        }
        protected void InitMyBlog()
        {
            UpdateBlogId(BlogOwnerId);
            using MRWBlogsContext context = new(GetContextOptions());
            Articles = context.BlogsMessages.Where(a => a.BlogId == BlogId).Take(NumberSumMessagesonPage).ToList();
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
