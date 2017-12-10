using System;
using System.Net;
using System.Net.Http;

namespace Kerv.Common
{
    public class RequestHandler
    {
        static CookieContainer cookies;
        public static readonly HttpClient Client = 
            new HttpClient(new HttpClientHandler {
                CookieContainer = cookies,
                AllowAutoRedirect = false,
                UseCookies = true
        });
    }
}
