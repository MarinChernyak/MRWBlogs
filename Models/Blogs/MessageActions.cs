using Microsoft.AspNetCore.Components.Forms;
using MRWBlogs.Models.Shared;

namespace MRWBlogs.Models.Blogs
{
    public class MessageActions:MessageActionBase
    {
        public List<IBrowserFile> SelectedFiles = new List<IBrowserFile>();
        public MessageActions()
        { }

        public TagsManagerVM TagsManagerVM { get; set; } = new TagsManagerVM();
        protected async Task<MessageContainer> UploadAsync(IWebHostEnvironment? env, IBrowserFile file)
        {
            await Task.CompletedTask;
            return new MessageContainer();
        }
        public virtual Task<MessageContainer> DoMessageActionAsync(IWebHostEnvironment? env, IBrowserFile file, string token, string keywords)
        {
            return Task.FromResult(new MessageContainer());
        }

    }
}
