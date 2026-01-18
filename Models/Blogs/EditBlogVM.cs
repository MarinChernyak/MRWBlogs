using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MRWBlogs.Models.Shared;
using MRWBlogs_DAL.Entities;
using SMAuthentication.SMGeneralEntities;
using SMCommonUtilities;
using System.Reflection.Metadata;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Blogs
{
    public class EditBlogVM : BlogActions
    {
        public EditBlogVM() { }
        public EditBlogVM(int idBlog)
        { 
            Id = idBlog;
            InitBlog();
        }
        protected void InitBlog()
        {
            if (Id > 0)
            {
                using MRWBlogsContext context = new(GetContextOptions());
                var blog = context.Blogs.FirstOrDefault(b => b.Id == Id);
                if (blog != null)
                {
                    Title = blog.Title;
                    Description = blog.Description;
                    CreatedAt = blog.CreatedAt;
                    LastUpdatedAt = blog.UpdatedAt??DateTime.Now;
                    AvatarUrl = blog.Avatar ?? string.Empty;
                    IdOwner = blog.UserId;
                    TagsManagerVM = new TagsManagerVM(blog.Keywords);
                    VisabilityId = blog.VisibilityId;
                    Description = blog.Description;
                }
            }

        }

        public override async Task<MessageContainer> DoBlogActionAsync(IWebHostEnvironment? env, IBrowserFile file, string token, string keywords)
        {
            MessageContainer msg = new();

            msg = await UploadAsync(env, file);
            if (msg != null && msg.MsgType != MessageType.Error && msg.MsgType != MessageType.Warning)
            {
                using MRWBlogsContext context = new(GetContextOptions());
                var entityToUpdate = context.Blogs.FirstOrDefault(x => x.Id == Id);
                if (entityToUpdate != null)
                {
                    entityToUpdate.Title = Title;
                    entityToUpdate.UpdatedAt = DateTime.Now;

                    string avatar = GetImageFileName(file);
                    if (!string.IsNullOrEmpty(avatar))
                    {
                        entityToUpdate.Avatar=avatar;
                    }
                    entityToUpdate.Keywords = keywords;
                    entityToUpdate.Description = Description;

                    try
                    {
                        context.Entry(entityToUpdate).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        msg = new MessageContainer($"Blog updated successfully!", MessageType.Success);
                    }
                    catch (Exception ex)
                    {
                        LogMaster lm = new();
                        lm.SetLogException("EditBlogVM", "EditBlogAsync", ex.Message);
                        msg = new MessageContainer($"Error updating blog: {ex.Message}", MessageType.Error);
                    }
                }
                else msg ??= new("Unknown error during blog updating.", MessageType.Error);
                if (msg.MsgType == MessageType.Error || msg.MsgType == MessageType.Warning)
                {
                    // If there was an error or warning during upload, we might want to clean up the uploaded file
                    if (env != null)
                    {
                        string uploadPath = Path.Combine(env.WebRootPath, Constants.AvatarsFolder, GetImageFileName(file));
                        if (File.Exists(uploadPath))
                        {
                            try
                            {
                                File.Delete(uploadPath);
                            }
                            catch (Exception ex)
                            {
                                LogMaster lm = new();
                                lm.SetLogException("EditBlogVM", "EditBlogAsync - Cleanup", ex.Message);
                            }
                        }
                    }
                }
            }
            msg=msg ?? new MessageContainer( "Unknown Error", MessageType.Warning);

            return msg;
        }
    }
}
