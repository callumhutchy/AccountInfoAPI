using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.IO;

namespace UserInfoWebsite.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(ILogger<RegisterModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            Console.WriteLine("Great 1");

            var Username = Request.Form["username"];
            var EmailAddress = Request.Form["emailaddress"];
            var Password = Request.Form["password"];
            var confirmPassword = Request.Form["confirmpassword"];

            Console.WriteLine(Username + " " + EmailAddress + " " + Password + " " + confirmPassword);

            var request = HttpWebRequest.Create("https://server.callumhutchy.co.uk:5003/api/userinfo/create");
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
    {
        return true; // **** Always accept
    };
            var postData = "{\"username\":\"" + Username + "\",\"email\":\"" + EmailAddress + "\",\"password\":\"" + Password + "\",\"admin\":1}";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Console.WriteLine(response.StatusCode);
        }

    }

    public class RegisterUser
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int admin { get; set; }

    }
}
