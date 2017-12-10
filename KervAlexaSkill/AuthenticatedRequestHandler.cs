using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kerv.Common
{
    public interface ILoggedOutListener {
        void OnLoggedOut();
    }

    public class AuthenticatedRequestHandler
    {
        private readonly ILoggedOutListener _loggedOutListener;

        public AuthenticatedRequestHandler(ILoggedOutListener listener) 
                => _loggedOutListener = listener;

        protected async Task<HttpResponseMessage> Get(String url) {
            var response = await RequestHandler.Client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK) {
                return response;
            }

            if (String.IsNullOrEmpty(Credentials.Password)) {
                _loggedOutListener.OnLoggedOut();
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }

            var loginHandler = new LoginHandler();
            var success = await loginHandler.Login(Credentials.Username, 
                                                   Credentials.Password, true);

            if (success) {
                response = await RequestHandler.Client.GetAsync(url);
                return response;
            } else {
                _loggedOutListener.OnLoggedOut();
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}
