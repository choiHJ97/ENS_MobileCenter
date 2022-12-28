using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data.Common;
using Xamarin.Essentials;
using System.Data;
using System.Security.Cryptography;
using System.Xml;

namespace ENS_MobileCenter.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MemberPage : ContentPage
	{
        static Hlib.CFunc Func = new Hlib.CFunc();
        static Hlib.CUtil util = new Hlib.CUtil();
        static Hlib.CMariaDB DB = new Hlib.CMariaDB();
        public static string strUpdate;
        public static string struser,strpwd,strphone,strmail,pwd;
        /*string strUpdate = @"update tbl_login SET C_name = 'choi2',C_telno = 'pnc001',C_email = 'pnc001',
                  C_pass = 'ens2022!' where C_id = 'choi'";*/

        public MemberPage ()
		{
			InitializeComponent ();
            DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
            PVName.Text = PageLogin.strpvname;
            selectMember();
            struser = usernameEntry.Text;
            strphone = phoneEntry.Text;
            strmail = mailEntry.Text;
            strpwd = pwdEntry.Text;
            /*InsertBt.IsEnabled = false;*/
            

        }



        public async void OnRefreshBtClicked(object sender, EventArgs e)
        {
            PVName.Text = PageLogin.strpvname;
            selectMember();
        }

        public async void BtInsert_Click(object sender, EventArgs e)
        {

            InsertSign();
            await DisplayAlert("수정", "수정완료", "확인");

        }
        public interface IMyInterface

        {

            void SetData(String Data);

        }

        public async void OnPwdChangedBtClicked(object sender, EventArgs e)
        {

            ShowPopupDemo pwdpopup = new ShowPopupDemo(this as IMyInterface);
            /*await DisplayPromptAsync("비밀번호 변경", "기존 비밀번호");*/
            await PopupNavigation.PushAsync(pwdpopup);
        }

        public static void changepwd()

        {
            ShowPopupDemo.pwdText = pwdEntryConstatnts.pwdEntry;
        }


        public void selectMember()
        {
            string strsql = @"select C_name, C_telno, C_pass, C_email from tbl_login where C_id = '" + PageLogin.IdString + "'";
            string name, phone, mail;
            DataRow[] rdb = DB.Fn_Select(strsql);
            int icnt = rdb.Length;
            for (int i = 0; i < icnt; i++)
            {
                name = Convert.ToString(rdb[i]["C_name"]);
                phone = Convert.ToString(rdb[i]["C_telno"]);
                mail = Convert.ToString(rdb[i]["C_email"]);
                pwd = Convert.ToString(rdb[i]["C_pass"]);
                usernameEntry.Text = name;
                phoneEntry.Text = phone;
                mailEntry.Text = mail;
                pwdEntry.Text = pwd;
                pwd = strpwd;
                System.Diagnostics.Debug.WriteLine(name + phone + mail);
            }
        }
        public void selectPwd()
        {
            string strsql = @"select C_pass where C_id = '" + PageLogin.IdString + "'";
            DataRow[] rdb = DB.Fn_Select(strsql);
            int icnt = rdb.Length;
            for (int i = 0; i < icnt; i++)
            {
                pwd = Convert.ToString(rdb[i]["C_pass"]);
                pwd = pwdEntry.Text;
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            selectMember();
            //refresh the data
        }


        public void InsertSign()
        {
            /*if (InsertBt += ClickedEventArgs)
            {
                pwdEntryConstatnts.pwdEntry = pwdEntry.Text;
            }
            else if (pwdEntryConstatnts.pwdEntry != ShowPopupDemo.pwdText)
            {
                selectPwd();
            }*/
            System.Diagnostics.Debug.WriteLine("-----------------------------------------------------" + strpwd);
            struser = usernameEntry.Text;
            strphone = phoneEntry.Text;
            strmail = mailEntry.Text;
            System.Diagnostics.Debug.WriteLine(struser + strpwd + strphone + strmail);
            try
            {
                strUpdate = @"update tbl_login SET C_name = '" + struser + "',C_telno = '" + strphone + "',C_email = '" + strmail + "'," +
                    " C_pass = '" + strpwd + "' where C_id = '" + PageLogin.IdString + "'";
                DataRow[] rdb = DB.Fn_Select(strUpdate);

                System.Diagnostics.Debug.WriteLine("-----------------------------------------------------" + strUpdate);

                selectMember();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            

        }
        bool AreDetailsValid(User user)
        {
            return (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.phone) && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"));
        }

    }
}