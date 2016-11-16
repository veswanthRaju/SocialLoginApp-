namespace SampleFbLoginApp
{
    internal class Constants
    {   
        //** Application strings **//
        public static string WAIT = "Please wait.";
        public static string USERNAME = "name";
        public static string HELLO = "Hello ";
        public static string REST_TYPE = "GET";
        public static string CHECKING_INFO = "Checking user Info..";
        public static string SERVICE_ID = "userdata";
        public static string USER_KEY = "userkey";
        public static string WELCOME = "Welcome ";
        public static string LOGGED_OUT = "You are LoggedOut!!";
        public static string FAIL_AUTH = "Authentication is cancelled!";
        public const string GMAIL = "Gmail";
        public const string FACEBOOK = "Facebook";
        public const string MICROSOFT = "Microsoft";

        //** For Facebook **//
        public static string FB_APPID = "Your Id here";
        public static string FB_SCOPE = "";
        public static string FB_AUTHURL = "https://m.facebook.com/dialog/oauth/";
        public static string FB_REDIRECTURL= "http://www.facebook.com/connect/login_success.html";
        public static string FB_REQUESTURL = "https://graph.facebook.com/me?fields=id,name,email,picture.type(large)";
        
        //** For Twitter **//
        public static string TWITTER_KEY = "Your key here";
        public static string TWITTE_SECRET = "Your secret here";        
        public static string TWITTE_REQ_TOKEN = "https://api.twitter.com/oauth/request_token";
        public static string TWITTER_AUTH = "https://api.twitter.com/oauth/authorize";
        public static string TWITTER_ACCESS_TOKEN = "https://api.twitter.com/oauth/access_token";
        public static string TWITTE_CALLBACKURL = "http://mobile.twitter.com";
        public static string TWITTER_REQUESTURL = "https://api.twitter.com/1.1/account/verify_credentials.json";
        
        //** For Gmail **//
        public static readonly string GMAIL_ID = "***********.apps.googleusercontent.com";
        public static readonly string GMAIL_SCOPE = "https://www.googleapis.com/auth/userinfo.email";
        public static readonly string GMAIL_AUTH = "https://accounts.google.com/o/oauth2/auth";
        public static readonly string GMAIL_REDIRECTURL = "https://www.googleapis.com/plus/v1/people/me";
        public static readonly string GMAIL_REQUESTURL = "https://www.googleapis.com/oauth2/v2/userinfo";

        //** For Microsoft **//
        public static string MS_ID = "Your Id here";
        public static string MS_SCOPE = "https://graph.microsoft.com/user.read";
        public static string MS_AUTHURL = "https://login.microsoftonline.com/common/oauth2/V2.0/authorize";
        public static string MS_REDIRECTURL = "urn:ietf:wg:oauth:2.0:oob";
        public static string MS_REQUESTURL = "https://graph.microsoft.com/v1.0/me";
    }
}