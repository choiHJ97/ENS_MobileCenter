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
	public partial class Popupload : PopupPage
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
            public Popupload()
            {
                InitializeComponent();
            loadlabel.Text = "로딩 중";
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    if (progress >= 1)
                    {
                        /*Navigation.RemovePopupPageAsync(this);*/
                        istimerRunning = false;
                    }
                    else
                    {
                        progress += maxValue / progressmax;
                    }
                    return istimerRunning;
                });
            }
        }
    }