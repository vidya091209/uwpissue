using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await App.PublicClientApp.GetAccountsAsync();
            try
            {
                IAccount currentUserAccount = GetAccountByPolicy(accounts, App.PolicySignUpSignIn);
                authResult = await App.PublicClientApp.AcquireTokenInteractive(App.ApiScopes)
                    .WithAccount(GetAccountByPolicy(accounts, App.PolicySignUpSignIn))
                    .WithPrompt(Prompt.SelectAccount)
                    .ExecuteAsync();
                string startURL = "https://sundaramapptech.b2clogin.com/tfp/sundaramapptech.onmicrosoft.com/b2c_1_login/oauth2/v2.0/authorize?client_id=31508cf8-4b9e-4c0b-a304-a2b97c2e228f&scope=+openid+profile+offline_access&response_type=code&redirect_uri=msal31508cf8-4b9e-4c0b-a304-a2b97c2e228f://auth";
                string endURL = $"msal31508cf8-4b9e-4c0b-a304-a2b97c2e228f://auth";

         //       System.Uri startURI = new System.Uri(startURL);
         //       System.Uri endURI = new System.Uri(endURL);
         //       var webAuthenticationResult =
         //await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
         //Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
         //startURI,
         //endURI);
            }
            catch (MsalUiRequiredException ex)
            {
                authResult = await App.PublicClientApp.AcquireTokenInteractive(App.ApiScopes)
                    .WithAccount(GetAccountByPolicy(accounts, App.PolicySignUpSignIn))
                    .WithPrompt(Prompt.SelectAccount)
                  
                    .ExecuteAsync();
              
            }

            catch (Exception ex)
            {
                ResultText.Text = $"Users:{string.Join(",", accounts.Select(u => u.Username))}{Environment.NewLine}Error Acquiring Token:{Environment.NewLine}{ex}";
            }
        }
        private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
        {
            foreach (var account in accounts)
            {
                string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];
                if (userIdentifier.EndsWith(policy.ToLower())) return account;
            }

            return null;
        }

   
    }
}
