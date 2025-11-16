using SMAuthentication.Factories;
using SMAuthentication.Models;

namespace MRWBlogs.Models.LogIn
{
    public class MyAccountViewModel : RegistrationViewModel
    {
        public MyAccountViewModel() { }

        public bool SaveUserData(string token)
        {
            bool bRez = EncryptionHelper.EncryptUser(new SMAuthentication.SMGeneralEntities.User
            {
                Id = Id,
                UserName = UserName,
                Email = Email,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName,
                CountryId = CountryId,
                IsActive = IsActive,
                Token = token
            });
            return bRez;
        }
        public void InitModel(string token)
        {
            MUser? _user =  EncryptionHelper.GetDecryptedUserByToken(token, Constants.AppId);
            if (_user!=null)
            {
                Id = _user.Id;
                UserName = _user.UserName;
                if (_user.ActivationDate != null)
                    CreatedDate = _user.ActivationDate.Value;
                Email = _user.Email;
                Password = _user.Password;
                FirstName = _user.FirstName;
                LastName = _user.LastName;
                if(_user.CountryId.HasValue)
                    CountryId = _user.CountryId.Value;
                IsActive = _user.IsActive??false;

            }
        }
    }
}
