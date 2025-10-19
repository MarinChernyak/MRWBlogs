using SMAuthentication.Authentication;
using SMAuthentication.Models;
using SMGeo.GeoEntities;

namespace MRWBlogs.Models.LogIn
{
    public class RegistrationViewModel
    {
        public int Id { get; set; } 
        public string UserName { get; set; }=string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ErrorMessage {  get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public short CountryId { get; set; } = 0;
        public List<Country> Countries { get; set; } = new List<Country>();
        //public List<Country>
        public RegistrationViewModel()
        {
            InitCollections();
        }
        protected void InitCollections()
        {
            using (var context = new SMGeoContext())
            {
                Countries = context.Countries.OrderBy(c => c.CountryName).ToList();
            }
        }
        public async Task<StrResponse> SaveRegistration()
        {
            RegistrationModel model = new (Constants.AppId)
            {
                UserAccessLevel = 1,
                Email = Email,
                Password = Password,
                UserName = UserName,
                FirstName = FirstName,
                LastName = LastName,
                CountryId = CountryId
            };
            StrResponse sr = await model.SaveNewUserOrUpdateAsMemeberProject();
            return sr;
        }

    }
}
