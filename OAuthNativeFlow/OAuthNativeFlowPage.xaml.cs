using System;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Auth;
using OAuthNativeFlow.loginlayer;
using System.Collections.Generic;

namespace OAuthNativeFlow
{
    public partial class OAuthNativeFlowPage : ContentPage
    {
        OAuthLogInHandler oAuthLogInHandler;

        public OAuthNativeFlowPage()
        {
            InitializeComponent();
        }

        void OnLoginClicked(object sender, EventArgs e)
        {
            oAuthLogInHandler = new OAuthLogInHandler();
            oAuthLogInHandler.OAuthLogInResponseSuccessEvent += OAuthLogInResponseSuccess;
            oAuthLogInHandler.OAuthLogInResponseErrorEvent += OAuthLogInResponseError;

            oAuthLogInHandler.HandleOAuthLogIn();
           
        }

         async void OAuthLogInResponseSuccess(object sender, OAuthLogInResponseEventArgs e)
        {
            Console.WriteLine("OAuthLogInResponseSuccess");
            if (oAuthLogInHandler != null)
            {
                oAuthLogInHandler.OAuthLogInResponseSuccessEvent -= OAuthLogInResponseSuccess;
                oAuthLogInHandler.OAuthLogInResponseErrorEvent -= OAuthLogInResponseError;
            }
           AccountStore store= AccountStore.Create();
           IEnumerable<Account> account = store.FindAccountsForService(Constants.AppName);


           await FetchUserDataAsync(account.ElementAt(0));
            // await DisplayAlert("Email address", user.Email, "OK");
        }

        void OAuthLogInResponseError(object sender, OAuthLogInResponseEventArgs e)
        {
            Console.WriteLine("OAuthLogInResponseError");
        }

        private async System.Threading.Tasks.Task FetchUserDataAsync(Account account)
        {
            User user = null;
            // If the user is authenticated, request their basic user data from Google
            // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
            var request = new OAuth2Request("GET", new Uri(LogInConstants.UserInfoUrl), null, account);
            var response = await request.GetResponseAsync();
            if (response != null)
            {
                // Deserialize the data and store it in the account store
                // The users email address will be used to identify data in SimpleDB
                string userJson = await response.GetResponseTextAsync();
                user = JsonConvert.DeserializeObject<User>(userJson);
            }
        }
    }
}
