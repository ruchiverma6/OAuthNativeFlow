using System;
namespace OAuthNativeFlow.loginlayer
{
    public class LogInConstants
    {
        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string iOSClientId = "145184912962-ckgi9jmq8i2qltdj0fs5iboooq5rj5bb.apps.googleusercontent.com";
        public static string AndroidClientId = "145184912962-c0ki7q8jlup95v5av1caqiifch7ctj52.apps.googleusercontent.com";//"<insert Android client ID here>";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl = "com.googleusercontent.apps.145184912962-ckgi9jmq8i2qltdj0fs5iboooq5rj5bb:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.145184912962-c0ki7q8jlup95v5av1caqiifch7ctj52:/oauth2redirect";
    }
}
