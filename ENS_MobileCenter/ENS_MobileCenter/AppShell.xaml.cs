using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Scheduler;
using ENS_MobileCenter;
using ENS_MobileCenter.Models;
using ENS_MobileCenter.Views;
using Xamarin.Forms;

namespace ENS_MobileCenter
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DayPage), typeof(DayPage));
            Routing.RegisterRoute(nameof(HistroyPage), typeof(HistroyPage));
            Routing.RegisterRoute(nameof(EventPage), typeof(EventPage));
            Routing.RegisterRoute(nameof(MemberPage), typeof(MemberPage));
            Routing.RegisterRoute(nameof(PageLogin), typeof(PageLogin));
        }
        public override bool OnBackButtonPressed()
        {

            ServiceLocator.Current.GetInstance<IDialogService>().DisplayAlert("로그아웃", "로그아웃 하시겠습니까?", "취소", "확인").ContinueWith(task =>
            {
                if (task.Result)
                {
                    ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
                }
            });

            return true;
        }

   
         async void AnswerMessage()
        {
            bool answer = await DisplayAlert("로그아웃", "로그아웃 하시겠습니까?", "취소", "확인");
            if (answer == false)
            {
                Application.Current.MainPage = new PageLogin();
            }
            else
            {
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}

/*https://social.msdn.microsoft.com/Forums/en-US/1f1f97a5-f4cc-41d9-bfaf-4391aa5959cf/xamarinforms-change-page-after-login-properly?forum=xamarinforms*/