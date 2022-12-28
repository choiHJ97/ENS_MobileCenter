using System;
using ENS_MobileCenter.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("NotoSansKR_Regular.otf", Alias = "RegularFont")]
[assembly: ExportFont("NotoSansKR_Thin.otf", Alias = "ThinFont")]
[assembly: ExportFont("NotoSansKR_Bold.otf", Alias = "BoldFont")]
[assembly: ExportFont("NotoSansKR_Medium.otf", Alias = "MediumFont")]
[assembly: ExportFont("NotoSansKR_Light.otf", Alias = "LightFont")]
[assembly: ExportFont("SpoqaHanSansNeo_Regular.ttf", Alias = "SpoqaRegular")]
namespace ENS_MobileCenter
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public App()
        {
            Application.Current.Resources.Add(new Xamarin.Forms.Style(typeof(Page))
            {
                Setters = {
                   /* new Xamarin.Forms.Setter { Property = PageLogin.BackgroundImageProperty, Value = "bgimg.jpg"},*/
                }
            });
            if (!IsUserLoggedIn)
            {
                MainPage = new SplashPage();
            }
            else
            {
                /*MainPage = new NavigationPage(new ENS_MobileCenter.AppShell());*/
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
