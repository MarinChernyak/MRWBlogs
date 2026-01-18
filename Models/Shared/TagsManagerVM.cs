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
        public TagsManagerVM(string tagsLine)
        {
            if (!string.IsNullOrEmpty(tagsLine))
            {
                SelectedTags = tagsLine.Split(',').ToList();
            }
        }
        public string GetTagsAsString()
        {
            return string.Join(",", SelectedTags);
        }
    }
}
