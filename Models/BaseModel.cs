using Microsoft.EntityFrameworkCore;
using MRWBlobs_DAL.Entities;

namespace MRWBlogs.Models
{
    public class BaseModel
    {
        protected DbContextOptions GetContextOptions()
        {
            string constring = "MRWBlogsConnection";
            var optionsBuilder = new DbContextOptionsBuilder<MRWBlogsContext>();
            optionsBuilder.UseSqlServer(constring);
            return optionsBuilder.Options;
        }
    }
}
