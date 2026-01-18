using Microsoft.AspNetCore.Components.Forms;
using MRWBlobs_DAL.Entities;
using MRWBlogs.Models.Shared;
using MRWBlogs_DAL.Entities;
using SMCommonUtilities;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Blogs
{
    public class CreateBlogVM: BlogActions
    {
     
        public CreateBlogVM()
        {
        }

        protected override int GetMaxSize()
        {
            return 50 * 1024; // 50 KB
        }
        protected override string GetImgFolder()
        {
            return Constants.AvatarsFolder;
        }
        protected override string GetImageFileName(IBrowserFile file)
        {
            string fname = Guid.NewGuid().ToString() + Path.GetExtension(file.Name);
            return fname;
        }
        public override async Task<MessageContainer> DoBlogActionAsync(IWebHostEnvironment? env, IBrowserFile file, string token, string keywords)
        {
            MessageContainer? msg = null;
            if (! await IsBlogExisting(token)) //it is just to prevent double clicking
            {
                msg = await UploadAsync(env, file);
                if (msg != null && msg.MsgType != MessageType.Error && msg.MsgType != MessageType.Warning && IdOwner > 0)
                {
                    using MRWBlogsContext context = new(GetContextOptions());
                    Blog newBlog = new()
                    {
                        Title = Title,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Avatar = GetImageFileName(file),
                        UserId = IdOwner,
                        IsPublished = true,
                        Keywords = keywords,
                        VisibilityId = 1,
                        Description = Description
                    };
                    context.Blogs.Add(newBlog);
                    try
                    {
                        await context.SaveChangesAsync();
                        Id = newBlog.Id;
                        msg = new MessageContainer($"Blog created successfully! Id={Id}", MessageType.Success);
                    }
                    catch (Exception ex)
                    {
                        LogMaster lm = new();
                        lm.SetLogException("CreateBlogVM", "CreateBlogAsync", ex.Message);
                        msg = new MessageContainer($"Error creating blog: {ex.Message}", MessageType.Error);
                    }
                }
                else msg ??= new("Unknown error during blog creation.", MessageType.Error);
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
                                lm.SetLogException("CreateBlogVM", "CreateBlogAsync - Cleanup", ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                msg = new MessageContainer("User already has a blog. Cannot create another one.", MessageType.Warning);
            }
            return msg;
        }
    }
}
