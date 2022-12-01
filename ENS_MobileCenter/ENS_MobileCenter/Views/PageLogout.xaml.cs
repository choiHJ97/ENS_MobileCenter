using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENS_MobileCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLogout : ContentPage
    {
        public PageLogout()
        {
            InitializeComponent();
            AnswerMessage();
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