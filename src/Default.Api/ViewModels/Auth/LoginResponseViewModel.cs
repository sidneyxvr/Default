using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Default.Api.ViewModels.Auth
{
    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public UserTokenViewModel UserToken { get; set; }
    }
}
