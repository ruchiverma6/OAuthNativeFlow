using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using OAuthNativeFlow.loginlayer;
using Xamarin.Auth;
using Xamarin.Forms;

namespace OAuthNativeFlow
{
    public class OAuthLogInHandler
    {
        Account account;
        AccountStore store;

        public event EventHandler<OAuthLogInResponseEventArgs> OAuthLogInResponseSuccessEvent;
        public event EventHandler<OAuthLogInResponseEventArgs> OAuthLogInResponseErrorEvent;
        public OAuthLogInHandler()
        {
            store = AccountStore.Create();
        }

        internal void HandleOAuthLogIn()
        {
            string clientId = null;

            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = LogInConstants.iOSClientId;
                    redirectUri = LogInConstants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = LogInConstants.AndroidClientId;
                    redirectUri = LogInConstants.AndroidRedirectUrl;
                    break;
            }

            account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                LogInConstants.Scope,
                new Uri(LogInConstants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(LogInConstants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }


        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

         
            if (e.IsAuthenticated)
            {             
                if (account != null)
                {
                    store.Delete(account, Constants.AppName);
                }

                await store.SaveAsync(account = e.Account, Constants.AppName);

                if (OAuthLogInResponseSuccessEvent != null)
                {
                    OAuthLogInResponseSuccessEvent(this, new OAuthLogInResponseEventArgs());
                }
                // await DisplayAlert("Email address", user.Email, "OK");
            }
        }

      

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
            if (OAuthLogInResponseErrorEvent != null)
            {
                OAuthLogInResponseErrorEvent(this,new OAuthLogInResponseEventArgs());
            }
        }
    }
}
