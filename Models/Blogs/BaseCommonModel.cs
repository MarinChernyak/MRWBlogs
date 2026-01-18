using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MRWBlogs.Models.Shared;
using MRWBlogs_DAL.Entities;
using SMAuthentication.Factories;
using SMAuthentication.Models;
using SMCommonUtilities;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Blogs
{
    public class BaseCommonModel: BaseModel
    {
        public int IdOwner { get; protected set; }
        public HolderMessageVM HolderMessageVM = new ();
        public async Task<bool> IsBlogExisting(string token)
        {
            bool result = false;
            MUser? user = await UsersFactoryHelpers.CheckToken(token, Constants.AppId);
            if (user != null)
            {
                using MRWBlogsContext context = new(GetContextOptions());
                var blog = context.Blogs.FirstOrDefault(b => b.UserId == user.Id);
                if (blog != null)
                {
                    result = true;
                }
                else
                {
                    IdOwner = user.Id;
                }
            }
            return result;
        }
        protected virtual string GetImageFileName(IBrowserFile file)
        {
            string fname = Guid.NewGuid().ToString() + Path.GetExtension(file.Name);
            return fname;
        }
        protected virtual int GetMaxSize()
        {
            return 0;
        }
        protected virtual string GetImgFolder()
        {
            return Constants.ImagesFolder;
        }
        protected async Task<MessageContainer> UploadAsync(IWebHostEnvironment? env, IBrowserFile file)
        {
            MessageContainer message = new();
            LogMaster lm = new();
            try
            {

                if (file != null && env != null && !string.IsNullOrEmpty(file.Name))
                {
                    // Set the maximum allowed file size (e.g., 50kB)
                    var maxFileSize = GetMaxSize();

                    string fname = GetImageFileName(file);
                    // Define the server path
                    string uploadPath = Path.Combine(env.WebRootPath, GetImgFolder(), fname);
                    try
                    {
                        // Copy the stream to a file on the server
                        await using var stream = new FileStream(uploadPath, FileMode.Create);
                        await file.OpenReadStream(maxFileSize).CopyToAsync(stream);


                    }
                    catch (Exception ex)
                    {

                        lm.SetLogException("CreateBlogVM", "UploadAsync", ex.Message);
                        message = new($"The selected image is exciding {maxFileSize}kb. Please select another one.", MessageType.Error);
                    }
                }
                if (message.MsgType == MessageType.None)
                    message = new($"The selected image uploaded successfully!", MessageType.Info);

            }
            catch (Exception ex)
            {
                lm.SetLogException("CreateBlogVM", "UploadAsync", ex.Message);
                message = new($"Error uploading files: {ex.Message}", MessageType.Error);
            }


            return message;
        }
    }
}
