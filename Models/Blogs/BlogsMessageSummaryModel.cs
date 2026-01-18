using Microsoft.Identity.Client;
using MRWBlobs_DAL.Entities;
using MRWBlogs_DAL.Entities;
using SMAuthentication.Factories;
using SMAuthentication.Models;

namespace MRWBlogs.Models.Blogs
{
    public class BlogsMessageSummaryModel: BaseCommonModel
    {
        public List<MessageImage> ArticleImages { get; set; } = [];
        public string Content { get; set; } = string.Empty;
        public string KeyWords { get; set; } = string.Empty;
        public int BlogOwnerUserId { get; set; }
        public int CurUserId { get; set; }
        public int MessageId { get; protected set; }
        public string Title { get; set; } = string.Empty;
        public int BlogId { get; set; }

        public DateTime CreatedAt { get; protected set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; protected set; } = DateTime.Now;

        public int VisabilityId { get; protected set; }
        public BlogsMessageSummaryModel() { }
        public BlogsMessageSummaryModel(int ArticleId, string token) 
        {
            var user = UsersFactoryHelpers.CheckToken(token, Constants.AppId);
            if(user != null)
            {
                CurUserId = user.Id;
            }
            using MRWBlogsContext context = new(GetContextOptions());
            var bm = context.BlogsMessages.FirstOrDefault(x => x.Id == ArticleId);
            if (bm != null)
            {
                var blog = context.Blogs.FirstOrDefault(b => b.Id == bm.BlogId);
                if (blog != null)
                { 
                    Title = bm.Title;
                    Content = bm.Content;
                    KeyWords = bm.Tags;
                    BlogId = bm.BlogId;
                    MessageId = bm.Id;
                    VisabilityId = bm.VisibilityId;
                    CreatedAt = bm.CreatedAt;
                    BlogOwnerUserId = blog.UserId;

                    GetImages();
                }
            }
        }
        public BlogsMessageSummaryModel(BlogMessage bm, int blogOwnerUserId, int curUserId)
        {
            Title = bm.Title;
            Content = bm.Content;
            KeyWords = bm.Tags;
            BlogId = bm.BlogId;
            MessageId = bm.Id;
            VisabilityId = bm.VisibilityId;
            CreatedAt = bm.CreatedAt;
            BlogOwnerUserId = blogOwnerUserId;
            CurUserId = curUserId;

            GetImages();
        }
        protected void GetImages()
        {
            using MRWBlogsContext context = new(GetContextOptions());
            var images = context.MessageImages.Where(x => x.MessageId == MessageId);
            if(images!=null)
            {
                ArticleImages = images.ToList();
            }
        }
        public string GetImgURL(int index=0)
        {
            string url = string.Empty;
            if(ArticleImages.Count> index)
            {
                url = ArticleImages[index].ImageUrl;

            }
            return url;
        }
    }
}
