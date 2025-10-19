using SMCommonUtilities;
using SMUtilities;
using System.ComponentModel.DataAnnotations;
using SMAuthentication.SMGeneralEntities;
using SMAuthentication.Factories;

namespace MRWBlogs.Models.LogIn
{
    public class ResettterPass
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public ResettterPass() { }
        public ResettterPass(string email)
        {
            Email = email;
        }
        public void  ResetPassword()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrEmpty(Email))
            {
                ErrorMessage= "Email is required!";
            }
            StringGenerator sg = new StringGenerator();
            Password = sg.GenericString;

            string user_name = SetNewPassword(Password);
            if(string.IsNullOrEmpty(ErrorMessage) && !string.IsNullOrEmpty(user_name))
            {
                SMEmailer smEmailer = new SMEmailer(Email);                
                smEmailer.EmailNewPassword(Password, user_name);
            }
        }
        protected string SetNewPassword(string new_pass)
        {
            string user_name = string.Empty;
            using (var context = new SMGeneralContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == Email);
                if(user != null)
                {
                    user_name = user.UserName;
                    user.Password= new_pass;
                    if (!EncryptionHelper.EncryptUser(user))
                    {
                        ErrorMessage = "Error encrypting password";
                    }
                }
                else
                {
                    ErrorMessage = "Email not found";
                }
            }
            return user_name;
        }
        protected string GetUserName()
        {
            using(var context = new SMGeneralContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == Email);
                if (user != null)
                {
                    return user.UserName;
                }
                else
                {
                    ErrorMessage = "Email not found";
                    return string.Empty;
                }
            }

        }
    }
}
