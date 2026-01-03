using MRWBlogs_DAL.Entities;

namespace MRWBlogs.Models.Blogs
{
    public class BlogsMessageSummaryModel:BlogBaseModel
    {
        public List<MessageImage> BlogImages { get; set; } = [];
        public string MessageTitle { get; set; } = string.Empty;
        public string MessageBody { get; set; } = string.Empty;
        public string MessageKeyWords { get; set; } = string.Empty;
        public int BlogOwnerUserId { get; set; }
        public int CurUserId { get; set; }
        public BlogsMessageSummaryModel() { }

    }
}
