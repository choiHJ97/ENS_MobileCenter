using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;
using System.Windows.Input;

namespace ENS_MobileCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressBarPopup : PopupPage
    {
        double maxValue = 1;
        float progressmax = 1;
        bool istimerRunning = true;
        double progress;
        int counter = 1;
        const int RefreshDuration = 2;
        int itemNumber = 1;
        readonly Random random;
        bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());
        public ProgressBarPopup()
        {
            InitializeComponent();
            userlabel.Text = PageLogin.strName + "님 환영합니다. \n 잠시만 기다려주세요";
            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                if (progress >= 1)
                {
                    Navigation.PopPopupAsync();
                    istimerRunning = false;
                }
                else
                {
                    try
                    {
                        Xamarin.Forms.Application.Current.MainPage = new AppShell();
                        progress += maxValue / progressmax;
                    }
                    catch
                    {
                        DisplayAlert("연결 실패", "네트워크 확인이 필요합니다", "확인");
                    }
                    

                }
                return istimerRunning;
            });
        }

        async Task RefreshItemsAsync()
        {
            Circle.IsRunning = true;
            Circle.IsRunning = false;
        }
    }
}