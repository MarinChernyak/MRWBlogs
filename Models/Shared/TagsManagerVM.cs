using SMAuthentication.Factories;
using SMAuthentication.Models;

namespace MRWBlogs.Models.Shared
{
    public class TagsManagerVM
    {
        public List<string> SelectedTags { get; set; } = new();

        public TagsManagerVM()
        {
           
        }
        public string GetTagsAsString()
        {
            return string.Join(",", SelectedTags);
        }
    }
}
