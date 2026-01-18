using MRWBlogs_DAL.Entities;
using SMAuthentication.Factories;
using SMAuthentication.Models;

namespace MRWBlogs.Models.Blogs
{
    public class BlogDetailsModel:BlogBaseModel
    {
        public List<BlogsMessageSummaryModel> BlogMessages { get; set; } = new();
        public int BlogId { get; protected set; }
        public int CurUserId { get; protected set; } = 0;
        public int CurPageNum { get; set; } = 1;
        public int NumberMessagesPerPage { get; set; } = 10;

        public BlogDetailsModel() { }
        public BlogDetailsModel(int blogId, string token, int pageNum = 1, int numPerPage = 10) 
        {
            CurPageNum = pageNum;
            NumberMessagesPerPage = numPerPage;
            CurUserId = 0;
            BlogId = blogId;
            _=getCurUserId(token);
            InitModel();
        }
        protected async Task getCurUserId(string token)
        {
            MUser? user = await UsersFactoryHelpers.CheckToken(token, Constants.AppId);
            if (user != null)
            {
                CurUserId= user.Id;
            }
        }
        protected void InitModel()
        {
            using MRWBlogsContext context = new(GetContextOptions());

            var blog = context.Blogs.FirstOrDefault(b => b.Id == BlogId);
            if (blog != null)
            {                
                IdOwner = blog.UserId;              
            }
            var bmessages = context.BlogsMessages.Where(x => x.BlogId == BlogId).ToList();
            if (bmessages != null)
            {
                foreach (var bm in bmessages)
                {
                    BlogMessages.Add(new BlogsMessageSummaryModel(bm, IdOwner, CurUserId));                    
                }                
            }
        }
    }
}
