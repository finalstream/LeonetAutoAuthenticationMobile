using Java.Lang;
using Java.Net;

namespace laam.Droid
{
    internal class BasicAuthenticator : Authenticator
    {
        private string _username;
        private string _password;

        public BasicAuthenticator(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public PasswordAuthentication GetPasswordAuthentication()
        {
            return new PasswordAuthentication(_username, _password.ToCharArray());
        }
    }
}