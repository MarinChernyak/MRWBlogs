using MRWBlobs_DAL.Entities;
using MRWBlogs.Models;
using MRWBlogs.Models.Shared;
using SMAuthentication.Factories;
using SMAuthentication.Models;

namespace MRWBlogs.Models.Blogs
{
    public class BlogBaseModel: BaseCommonModel
    {
        public int Id { get; protected set; }
        public string Title { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Description { get; set; } = "No Description";

        public DateTime CreatedAt { get; protected set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; protected set; } = DateTime.Now;
        
        public int VisabilityId { get; protected set; }

        
    }
}
