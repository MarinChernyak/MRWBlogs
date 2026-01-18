using Microsoft.AspNetCore.Components.Forms;
using MRWBlobs_DAL.Entities;
using MRWBlogs.Models.Shared;
using MRWBlogs_DAL.Entities;
using SMCommonUtilities;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Blogs
{
    public class ArticleCreateVM :ArticleActionsBase
    {
        public List<IBrowserFile> SelectedFiles = new();
        protected string ImgFileName = string.Empty;
        public ArticleCreateVM() { }
        public ArticleCreateVM(int blogId)
        {
            BlogId = blogId;
        }
        protected override int GetMaxSize()
        {
            return 200 * 1024; // 200 KB
        }
        
        protected override string GetImageFileName(IBrowserFile file)
        {
            string fname = file.Name;
            if (!string.IsNullOrEmpty(fname))
            {
                int dotIndex = fname.LastIndexOf('.');
                string ext = fname.Substring(dotIndex + 1).ToLower();
                ImgFileName = $"Img_{Id}.{ext}";
            }
            return ImgFileName;
        }
        public async Task<MessageContainer> DoMessageActionAsync(IWebHostEnvironment? env, IBrowserFile file, string token, string keywords)
        {
            MessageContainer msg = new();

            using MRWBlogsContext context = new(GetContextOptions());
            try {
                BlogMessage newArticle = new()
                {
                    BlogId = BlogId,
                    Title = Title,
                    Content = Content,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsPublished = true,
                    Tags = keywords,
                    VisibilityId = 1

                };
                context.BlogsMessages.Add(newArticle);

                await context.SaveChangesAsync();
                Id = newArticle.Id;
               
                if(Id >0)
                {                    
                    msg = await UploadAsync(env, file);
                    if(msg != null && (msg.MsgType == MessageType.Info || msg.MsgType == MessageType.None))
                    {
                               
                        context.MessageImages.Add (new MessageImage()
                        {
                            MessageId = Id,
                            ImageUrl = ImgFileName
                        });
                    }

                    await context.SaveChangesAsync();
                    msg = new MessageContainer($"The article created successfully! Id={Id}", MessageType.Success);
                }
            }
            catch (Exception ex)
            {
                LogMaster lm = new();
                lm.SetLogException("ArticleCreateVM", "DoMessageActionAsync", ex.Message);
                msg = new MessageContainer($"Error creating blog's article: {ex.Message}", MessageType.Error);
            }
            
            if (env != null &&(msg != null && msg.MsgType == MessageType.Error))
            {
                string uploadPath = Path.Combine(env.WebRootPath, Constants.ImagesFolder, GetImageFileName(file));
                if (File.Exists(uploadPath))
                {
                    try
                    {
                        File.Delete(uploadPath);
                    }
                    catch (Exception ex)
                    {
                        LogMaster lm = new();
                        lm.SetLogException("ArticleCreateVM", "DoMessageActionAsync - Cleanup", ex.Message);
                    }
                }
            }

            return msg??new MessageContainer();
        }
    }
}
