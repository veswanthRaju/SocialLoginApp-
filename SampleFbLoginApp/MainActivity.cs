using Android.App;
using Android.OS;
using Android.Widget;
using Xamarin.Auth;
using System;
using System.Linq;
using System.Json;
using Android.Support.V7.App;

namespace SampleFbLoginApp
{
    [Activity(Label = "@string/AppName", Theme = "@style/MyTheme", MainLauncher = true, Icon = "@drawable/AppLogo")]
    public class MainActivity : AppCompatActivity
    {
        ProgressDialog progressDialog;
        Button GmailBtn, FbBtn, TwitterBtn, MSBtn, LogoutBtn;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            GmailBtn = FindViewById<Button>(Resource.Id.GmailButton);
            FbBtn = FindViewById<Button>(Resource.Id.FbButton);
            TwitterBtn = FindViewById<Button>(Resource.Id.TwitterButton);
            MSBtn = FindViewById<Button>(Resource.Id.MicrosoftButton);
            LogoutBtn = FindViewById<Button>(Resource.Id.LogoutButton);

            CachedUserData();
            WireEvents();
        }

        void CachedUserData()
        {
            var cache = AccountStore.Create().FindAccountsForService(Constants.SERVICE_ID).FirstOrDefault();
            if (cache != null)
            {
                Toast.MakeText(this, Constants.HELLO + cache.Properties[Constants.USER_KEY], ToastLength.Long).Show();
                FbBtn.Enabled = GmailBtn.Enabled = TwitterBtn.Enabled = MSBtn.Enabled = false;
                LogoutBtn.Enabled = true;
            }
            else
            {
                FbBtn.Enabled = GmailBtn.Enabled = TwitterBtn.Enabled = MSBtn.Enabled = true;
                LogoutBtn.Enabled = false;
            }
        }


        void WireEvents()
        {
            GmailBtn.Click += delegate { Authenticate(Constants.GMAIL); };
            FbBtn.Click += delegate { Authenticate(Constants.FACEBOOK); };
            MSBtn.Click += delegate { Authenticate(Constants.MICROSOFT); };
            TwitterBtn.Click += TwitterAuth;
            LogoutBtn.Click += Logout;
        }

        void Authenticate(string authValue)
        {
            switch (authValue)
            {
                case Constants.GMAIL:
                    Authentcation(Constants.GMAIL_ID, Constants.GMAIL_SCOPE, Constants.GMAIL_AUTH, Constants.GMAIL_REDIRECTURL, Constants.GMAIL_REQUESTURL);
                    break;

                case Constants.FACEBOOK:
                    Authentcation(Constants.FB_APPID, Constants.FB_SCOPE, Constants.FB_AUTHURL, Constants.FB_REDIRECTURL, Constants.FB_REQUESTURL);
                    break;

                case Constants.MICROSOFT:
                    Authentcation(Constants.MS_ID, Constants.MS_SCOPE, Constants.MS_AUTHURL, Constants.MS_REDIRECTURL, Constants.MS_REQUESTURL);
                    break;
            }
        }

        #region For Gmail, Facebook and Microsoft
        void Authentcation(string id, string scope, string authurl, string redirecturl, string requesturl)
        {
            var auth = new OAuth2Authenticator(id, scope, new Uri(authurl), new Uri(redirecturl));

            auth.AllowCancel = true;
            StartActivity(auth.GetUI(this));

            auth.Completed += async (sender, e) =>
            {
                if (!e.IsAuthenticated)
                {
                    Toast.MakeText(this, Constants.FAIL_AUTH, ToastLength.Short).Show();
                    return;
                }
                
                progressDialog = ProgressDialog.Show(this, Constants.WAIT, Constants.CHECKING_INFO, true);
                var request = new OAuth2Request(Constants.REST_TYPE, new Uri(requesturl), null, e.Account);
                var response = await request.GetResponseAsync();

                if (response != null)
                {
                    progressDialog.Hide();
                    var userJson = response.GetResponseText();
                    StoringDataIntoCache(userJson);
                }
            };
        }
        #endregion

        #region For Twitter
        private void TwitterAuth(object sender, EventArgs ee)
        {
            var auth = new OAuth1Authenticator(
                                Constants.TWITTER_KEY,
                                Constants.TWITTE_SECRET,
                                new Uri(Constants.TWITTE_REQ_TOKEN),
                                new Uri(Constants.TWITTER_AUTH),
                                new Uri(Constants.TWITTER_ACCESS_TOKEN),
                                new Uri(Constants.TWITTE_CALLBACKURL));

            auth.AllowCancel = true;
            StartActivity(auth.GetUI(this));

            auth.Completed += async (s, e) =>
            {
                if (!e.IsAuthenticated)
                {
                    Toast.MakeText(this, Constants.FAIL_AUTH, ToastLength.Short).Show();
                    return;
                }

                progressDialog = ProgressDialog.Show(this, Constants.WAIT, Constants.CHECKING_INFO, true);
                var request = new OAuth1Request("GET", new Uri(Constants.TWITTER_REQUESTURL), null, e.Account);
                var response = await request.GetResponseAsync();

                if (response != null)
                {
                    progressDialog.Hide();
                    var userJson = response.GetResponseText();
                    StoringDataIntoCache(userJson);
                }
            };
        }
        #endregion

        void StoringDataIntoCache(string userData)
        {
            var data = JsonValue.Parse(userData);

            Account account = new Account();
            account.Properties.Add(Constants.USER_KEY, data[Constants.USERNAME]);
            AccountStore.Create(this).Save(account, Constants.SERVICE_ID);

            Toast.MakeText(this, Constants.WELCOME + data[Constants.USERNAME], ToastLength.Long).Show();
            FbBtn.Enabled = GmailBtn.Enabled = TwitterBtn.Enabled = MSBtn.Enabled = false;
            LogoutBtn.Enabled = true;
        }

        void Logout(object sender, EventArgs e)
        {
            var data = AccountStore.Create(this).FindAccountsForService(Constants.SERVICE_ID).FirstOrDefault();
            if (data != null)
            {
                AccountStore.Create(this).Delete(data, Constants.SERVICE_ID);

                FbBtn.Enabled = GmailBtn.Enabled = TwitterBtn.Enabled = MSBtn.Enabled = true;
                LogoutBtn.Enabled = false;
                Toast.MakeText(this, Constants.LOGGED_OUT, ToastLength.Short).Show();
            }
        }
    }
}