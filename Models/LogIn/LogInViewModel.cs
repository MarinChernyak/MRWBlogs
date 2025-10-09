using SMAuthentication.Main;
using SMAuthentication.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace MRWBlogs.Models.LogIn
{
    public class LogInViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public int ErrCode { get; set; } = 0;
        public bool ShouldRememberMe { get; set; }
        public string UserToken { get; set; } = string.Empty;
        public int UserLevel { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public LogInViewModel()
        {

        }
        public bool Authenticate()
        {
            bool brez = false;
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                Authenticator auth = new (UserName,Password,Constants.AppId);
                StrResponse response=  auth.Authenticate();
                ErrCode= response.ErrCode;
                if (response.ErrCode == SMAuthentication.Constants.ErrorsCodes.NoError)
                {
                    brez = true;
                    UserId = int.Parse(response.GetValueByName(SMAuthentication.Constants.Values.UserId), CultureInfo.InvariantCulture);
                    UserToken = response.GetValueByName(SMAuthentication.Constants.Values.UserToken);
                    var ulevel = response.GetValueByName(SMAuthentication.Constants.Values.UserLevel);
                    if (!string.IsNullOrEmpty(ulevel) && int.TryParse(ulevel, out int ul))
                    {
                        UserLevel = ul;
                    }
                }
                else
                {
                    ErrorMessage = response.GetStringByErrCode();
                }
            }
            return brez;
        }
        public string UpdateToken()
        {
            string new_token;
            MUser user = new (UserId, Constants.AppId);
            new_token = Guid.NewGuid().ToString();
            user.SetUserToken(new_token);
            
            return new_token;
        }
    }
}
