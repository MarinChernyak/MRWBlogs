using SMAuthentication.Factories;
using SMAuthentication.Models;

namespace MRWBlogs.Models.Shared
{
    public class TagsManagerVM
    {
        public Dictionary<int, string> ExistingTags { get; set; } = new ();
        public Dictionary<int, string> SelectedTags { get; set; } = new();

        public TagsManagerVM()
        {
            InitExistingTags();
        }
        protected void InitExistingTags()
        {
            ExistingTags.Add(1, "C#");
            ExistingTags.Add(2, "ASP.NET Core");
            ExistingTags.Add(3, "Entity Framework");
            ExistingTags.Add(4, "Blazor");
            ExistingTags.Add(5, "Razor Pages");
            ExistingTags.Add(6, "LINQ");
            ExistingTags.Add(7, "Dependency Injection");
            ExistingTags.Add(8, "Web API");
            ExistingTags.Add(9, "SignalR");
            ExistingTags.Add(10, "Identity");
        }
        public string getTagDesription(int tagId)
        {
            return ExistingTags.ContainsKey(tagId) ? ExistingTags[tagId] : string.Empty;
        }
        public async Task<int> getUseracessLevel(string token)
        {
            int iLevel = 0;
            MUser? user = await UsersFactoryHelpers.CheckToken(token, Constants.AppId);
            if(user!=null)
            {
                iLevel = user.UserAccessLevel;

            }

            return iLevel;
        }
    }
}
