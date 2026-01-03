using Authentication.Migrations;
using MRWBlobs_DAL.Entities;

namespace MRWBlogs.Models.Blogs
{
    public class BlogsList: BlogBaseModel
    {
        public List<BlogSummaryModel> Blogs { get; set; } = new();
        public BlogsList()
        {
            InitModel();
        }
        protected void InitModel()
        {
            using MRWBlogsContext context = new(GetContextOptions());
            var blogs = context.Blogs.OrderByDescending(x=>x.CreatedAt).Take(10).ToList();
            foreach (var blog in blogs) 
            {
                Blogs.Add(new BlogSummaryModel(blog.Id));
            }
        }
    }
}
