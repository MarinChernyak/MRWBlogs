using Microsoft.AspNetCore.Components.Forms;
using MRWBlogs.Models.Shared;
using System.Reflection.Metadata;

namespace MRWBlogs.Models.Blogs
{
    public class MessageCreate : MessageActions
    {
        /// 
        public MessageCreate() { }
        public MessageCreate(int BlogId) 
        {
            Id = BlogId;
        }

        public override async Task<MessageContainer> DoMessageActionAsync(IWebHostEnvironment? env, IBrowserFile file, string token, string keywords)
        {
            MessageContainer mc = new MessageContainer();
            await Task.CompletedTask;
            return mc;
        }
    }
}
