using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Kerv.Common
{
    public class LoginHandler
    {
        static String LoginPageUrl = "https://kerv.com/en/loginsignup#login";
        static String LoginUrl = "https://kerv.com/umbraco/Surface/LoginSignup/Login";

        public async Task<bool> Login(string username, string password,
                                      bool remember)
        {
            var response = await RequestHandler.Client.GetAsync(LoginPageUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.Redirect)
            {
                // Already logged in!
                return true;
            }
            var html = await response.Content.ReadAsStringAsync();
            var token = ParseToken(html);

            var formContent = new Dictionary<string, string>();
            formContent.Add("LoginEmail", username);
            formContent.Add("UserPassword", password);
            formContent.Add("__RequestVerificationToken", token);
            var encodedContent = new FormUrlEncodedContent(formContent);
            response = await RequestHandler.Client.PostAsync(LoginUrl, encodedContent);
            if (ValidateLoginResponse(response))
            {
                if (remember)
                {
                    Credentials.Username = username;
                    Credentials.Password = password;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        string ParseToken(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var form = document.GetElementbyId("form0");
            var inputs = form.Elements("input");

            foreach (var input in inputs)
            {
                var name = input.GetAttributeValue("name", "");
                if (name == "__RequestVerificationToken")
                {
                    return input.GetAttributeValue("value", "");
                }
            }
            return "";
        }

        bool ValidateLoginResponse(HttpResponseMessage response)
        {
            IEnumerable<string> cookieValues;
            var hasCookies =
                response.Headers.TryGetValues("Set-Cookie", out cookieValues);
            if (hasCookies)
            {
                foreach (var cookie in cookieValues)
                {
                    if (cookie.StartsWith("kervplatform",
                                          StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}