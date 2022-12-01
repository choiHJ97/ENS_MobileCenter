using System;
using System.Collections.Generic;
using ENS_MobileCenter.Views;
using ENS_MobileCenter.Models;
using Xamarin.Forms;

namespace ENS_MobileCenter
{
    public partial class MainPage : Xamarin.Forms.Shell
    {
        public MainPage()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DayPage), typeof(DayPage));
            Routing.RegisterRoute(nameof(HistroyPage), typeof(HistroyPage));
            Routing.RegisterRoute(nameof(EventPage), typeof(EventPage));
            Routing.RegisterRoute(nameof(MemberPage), typeof(MemberPage));
            Routing.RegisterRoute(nameof(PageLogin), typeof(PageLogin));
        }
        public void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            Navigation.InsertPageBefore(new PageLogin(), this);
            Navigation.PopAsync();
        }
        /*protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new MainViewModel();
        }*/
    }
}
/*< Button Grid.Row = "3"    Padding = "30" VerticalOptions = "EndAndExpand"
                HorizontalOptions = "CenterAndExpand" Text = "Logout"
                CornerRadius = "30"
                Margin = "0,0,0,20"
                FontAttributes = "Bold"
                Clicked = "OnLogoutButtonClicked" />*/