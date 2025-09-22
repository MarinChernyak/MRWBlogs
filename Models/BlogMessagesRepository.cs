using Microsoft.EntityFrameworkCore;
using MRWBlobs_DAL.Entities;

namespace MRWBlogs.Models
{
    public class BlogMessagesRepository:BaseModel
    {
        public List<BlogMessage> BlogMessages { get; set; }=new List<BlogMessage>(); 
        public BlogMessagesRepository() { }
        public BlogMessagesRepository(int blogId)
        {
            GetRepository(blogId);
        }
        protected void GetRepository(int blogId)
        {
            using (MRWBlogsContext context = new MRWBlogsContext(GetContextOptions()))
            {
                var bm = context.BlogsMessages.Where(x=>x.BlogId == blogId).ToList();
                if (bm != null)
                    BlogMessages = bm;
            }
        }
    }
}
