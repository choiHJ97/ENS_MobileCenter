using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ENS_MobileCenter.Views.MemberPage;

namespace ENS_MobileCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    
    public partial class ShowPopupDemo : PopupPage
    {
        private IMyInterface pwdpopup = null;
        public static string pwdText , pwdlabel, pwd;
        static Hlib.CMariaDB DB = new Hlib.CMariaDB();
        public ShowPopupDemo(IMyInterface pwdpopup)
        {
            InitializeComponent();
            string strsql = @"select C_name, C_telno, C_pass, C_email from tbl_login where C_id = '" + PageLogin.IdString + "'";
            DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
            DataRow[] rdb = DB.Fn_Select(strsql);
            int icnt = rdb.Length;
            for (int i = 0; i < icnt; i++)
            {
                pwd = Convert.ToString(rdb[i]["C_pass"]);
                
            }
            
            this.pwdpopup= pwdpopup;
        }

        public async void CancelBt_Clicked(object sender, EventArgs e)
        {
            pwdText = pwd;
            MemberPage.strpwd = pwd;
            oldpwdEntry.Text =string.Empty;
            newpwdEntry.Text = string.Empty;
            conpwdEntry.Text = string.Empty;
            System.Diagnostics.Debug.WriteLine("-----------------------------------------------------" + MemberPage.strpwd);
            await PopupNavigation.PopAsync(true);
        }

        public async void inserBt_Clicked(object sender, EventArgs e)
        {
            var user = new User()
            {
                Password= newpwdEntry.Text
            };
            
            pwdText = this.newpwdEntry.Text;
            if (pwdMessage.Text == "*확인" && RepwdMessage.Text == "비밀번호가 일치합니다")
            {
                changepwd();
                MemberPage.strpwd = newpwdEntry.Text;
                await PopupNavigation.PopAsync(true);
                System.Diagnostics.Debug.WriteLine("-----------------------------------------------------" + MemberPage.strpwd);
            }

        }
        public async void OnconpwdChanged(object sender, EventArgs e)
        {
            if ( conpwdEntry.Text.Length > 4 ) 
            {
            if (conpwdEntry.Text == newpwdEntry.Text)
                {
                    RepwdMessage.Text = "비밀번호가 일치합니다";
                    RepwdMessage.TextColor= Color.Black;
                }
            else if (conpwdEntry.Text != newpwdEntry.Text)
                {
                    RepwdMessage.Text = "비밀번호가 일치하지 않습니다";
                }
                if (conpwdEntry.Text == pwd)
                {
                    RepwdMessage.Text = "기존 비밀번호와 일치합니다";
                }
                else if (conpwdEntry.Text != pwd)
                {

                }
            }
            else
            {
                RepwdMessage.Text = "비밀번호를 4자리 이상 입력해주세요";
                RepwdMessage.TextColor = Color.Red;
            }
        }
        public async void OnoldpwdChanged(object sender, EventArgs e)
        {
            if (oldpwdEntry.Text.Length > 4)
            {
                if (oldpwdEntry.Text == pwd)
                {
                    pwdMessage.Text = "*확인";
                    pwdMessage.TextColor = Color.Black;
                }
                else
                {
                    pwdMessage.Text = "비밀번호가 틀렸습니다.";
                    pwdMessage.TextColor = Color.Red;
                }
            }
            else
            {
                pwdMessage.Text = "비밀번호를 4자리 이상 입력해주세요";
                pwdMessage.TextColor = Color.Red;
            }
        }


    }
}