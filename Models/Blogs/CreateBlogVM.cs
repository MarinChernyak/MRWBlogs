using Microsoft.AspNetCore.Components.Forms;
using MRWBlobs_DAL.Entities;
using MRWBlogs.Models.Shared;
using SMCommonUtilities;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Blogs
{
    public class CreateBlogVM: BlogBaseModel
    {
     
        public CreateBlogVM()
        {
        }

        protected async Task<MessageContainer> UploadAsync(IWebHostEnvironment? env, IBrowserFile file )
        {

            MessageContainer message = new ();
            LogMaster lm = new ();
            try
            {

                if (file != null && env!=null )
                {
                    // Set the maximum allowed file size (e.g., 50kB)
                    var maxFileSize = 50 * 1024;

                    string fname = GetAvatarFileName(file);
                    // Define the server path
                    string uploadPath = Path.Combine(env.WebRootPath, Constants.AvatarsFolder, fname);
                    try
                    {
                        // Copy the stream to a file on the server
                        await using var stream = new FileStream(uploadPath, FileMode.Create);
                        await file.OpenReadStream(maxFileSize).CopyToAsync(stream);


                    }
                    catch (Exception ex)
                    {

                        lm.SetLogException("CreateBlogVM", "UploadAsync", ex.Message);
                        message= new( $"The selected avtar is exciding 50kb. Please select another one.", MessageType.Error);
                    }
                }
                message = new($"The selected avatar uploaded successfully!", MessageType.Info);

            }
            catch (Exception ex)
            {
                lm.SetLogException("CreateBlogVM", "UploadAsync", ex.Message);
                message = new($"Error uploading files: {ex.Message}", MessageType.Error);
            }
               
           
            return message;
        }
        private string GetAvatarFileName(IBrowserFile file)
        {
            string fname = file.Name;
            int dotIndex = fname.LastIndexOf('.');
            string ext = fname.Substring(dotIndex + 1).ToLower();
            fname = $"avatar_{IdOwner}.{ext}";
            return fname;
        }
        public async Task<MessageContainer> CreateBlogAsync(IWebHostEnvironment? env, IBrowserFile file, string token, string keywords)
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
                        Avatar = GetAvatarFileName(file),
                        UserId = IdOwner,
                        IsPublished = true,
                        Keywords = keywords,
                        VisibilityId = 1
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
                        string uploadPath = Path.Combine(env.WebRootPath, Constants.AvatarsFolder, GetAvatarFileName(file));
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
