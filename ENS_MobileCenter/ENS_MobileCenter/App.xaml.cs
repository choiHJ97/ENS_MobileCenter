using System;
using ENS_MobileCenter.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                    new Xamarin.Forms.Setter { Property = PageLogin.BackgroundImageProperty, Value = "bgimg.jpg"},
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
