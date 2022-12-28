using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ENS_MobileCenter;
using ENS_MobileCenter.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace ENS_MobileCenter
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static string strID = PageLogin.IdString;
        public static string strCode = PageLogin.strpcode;
        public AppShell()
        {
            InitializeComponent();
            DayPage.CreateChart();
            WeekPage.CreateChart();
            WeekPage.GetCreateGrid();
            MonthPage.GetCreateGrid();
            NavigationPage.SetHasNavigationBar(this, true);
            DependencyService.Get<IStatusBar>().ShowStatusBar();//상단바 복구
            Routing.RegisterRoute(nameof(DayPage), typeof(DayPage));
            Routing.RegisterRoute(nameof(MonthPage), typeof(MonthPage));
            Routing.RegisterRoute(nameof(EventPage), typeof(EventPage));
            Routing.RegisterRoute(nameof(MemberPage), typeof(MemberPage));
            Routing.RegisterRoute(nameof(PageLogin), typeof(PageLogin));
            Routing.RegisterRoute(nameof(PageLogout), typeof(PageLogout));
            Routing.RegisterRoute(nameof(NoticePage), typeof(NoticePage));
            BindingContext = this;
    }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //refresh the data
        }



        public ICommand ExecuteLogout =>
new Command(async () => {
                var result = await this.DisplayAlert("로그아웃", "로그아웃 하시겠습니까?", "취소", "확인");

                if (result == false)
                {
        Navigation.RemovePage(this);
        Application.Current.MainPage.Navigation.PopAsync();
        Application.Current.MainPage = new PageLogin();
    }
                else
                {
        await Navigation.PopAsync(true);
    }
            });
       
    }
}

/*https://social.msdn.microsoft.com/Forums/en-US/1f1f97a5-f4cc-41d9-bfaf-4391aa5959cf/xamarinforms-change-page-after-login-properly?forum=xamarinforms*/