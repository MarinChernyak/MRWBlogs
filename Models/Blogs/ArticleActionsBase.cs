using MRWBlogs.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace MRWBlogs.Models.Blogs
{
    public class ArticleActionsBase: BaseCommonModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int BlogId { get; set; }
        public bool IsPublished { get; set; } = false;

        [MaxLength(500)]
        public string Tags { get; set; } = string.Empty;



    }
}
