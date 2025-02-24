using OsteoMAUIApp.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Authentication
{
    public class LoginResponseModel
    {
        public string status { get; set; }
        public string statusMessage { get; set; }
        public string Message { get; set; }
        public int statusCode { get; set; }
        //public UserModel user { get; set; }

        public UserModel user { get; set; }

    }
}
