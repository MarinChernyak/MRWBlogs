using Microsoft.AspNetCore.Components.Forms;
using MRWBlogs.Models.Shared;
using MRWBlogs_DAL.Entities;
using SMAuthentication.Factories;
using SMAuthentication.Models;
using SMCommonUtilities;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Blogs
{
    public class CreateBlogVM
    {
        public int Id { get; protected set; }
        public string Title { get; set; } = string.Empty;
        public int IdOwner { get; protected set; }
        public string OwnerUserName { get; protected set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; protected set; }= DateTime.Now;
        public DateTime LastUpdatedAt { get; protected set; } = DateTime.Now;

        public IWebHostEnvironment? Environment { get; set; } 

        public CreateBlogVM()
        {
        }

        public async Task<MessageContainer> UploadAsync(IBrowserFile file, string token )
        {

            MessageContainer message = new MessageContainer();
            LogMaster lm = new LogMaster();

            if (Environment != null && !string.IsNullOrEmpty(token))
            {
                MUser? user = await UsersFactoryHelpers.CheckToken(token, Constants.AppId);
                if (user != null)
                {
                    IdOwner = user.Id;
                    OwnerUserName = user.UserName;

                    try
                    {

                        if (file != null)
                        {
                            // Set the maximum allowed file size (e.g., 50kB)
                            var maxFileSize = 50 * 1024;

                            string fname = file.Name;
                            int dotIndex = fname.LastIndexOf('.');
                            string ext = fname.Substring(dotIndex + 1).ToLower();
                            fname = $"avatar_{IdOwner}.{ext}";
                            // Define the server path
                            string uploadPath = Path.Combine(Environment.WebRootPath, "Images/Avatars", fname);
                            try
                            {
                                // Copy the stream to a file on the server
                                await using (var stream = new FileStream(uploadPath, FileMode.Create))
                                {
                                    await file.OpenReadStream(maxFileSize).CopyToAsync(stream);
                                }

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
                }
            }
            else
            {
                string msg = "Environment is not set. Cannot upload files.";
                message = new(msg, MessageType.Warning);
                lm.SetLog(msg);

            }
            return message;
        }
        
    }
}
