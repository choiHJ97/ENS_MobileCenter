using MySqlConnector;
using System;
using System.Data;
using ENS_MobileCenter;
using Xamarin.Forms;

namespace ENS_MobileCenter.Views
{
    /*[XamlCompilation(XamlCompilationOptions.Compile)]*/
    public partial class PageLogin : ContentPage
    {
        string IdString, PwdString, strLogin, connectionString, DBString, strName;
        public PageLogin()
        {
            InitializeComponent();
            //SplashPage 상단 네비게이션 바를 안보이도록 설정
            NavigationPage.SetHasNavigationBar(this, false);
        }
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {

            /* await Navigation.PushAsync(new PageSignUp());*/
            /*Application.Current.MainPage = new PageSignUp();*/
            await Navigation.PushModalAsync(new PageSignUp());


        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            };
            Connet();
            var isValid = AreCredentialsCorrect(user);
            if (isValid)
            {
                App.IsUserLoggedIn = true;
                /*Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();*/
                Application.Current.MainPage = new AppShell();
               /* await Navigation.PushAsync(new AppShell());*//*
*/               /* await Navigation.PushModalAsync(new AppShell());*/

            }
        
   
            else
            {
                messageLabel.Text = "로그인 실패";
                passwordEntry.Text = string.Empty;
            }
        }

        public void Connet()
        {
            DataRowCollection Rs = null;
            IdString = usernameEntry.Text;
            PwdString = passwordEntry.Text;
            strLogin = "update tbl_login set D_indate  = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where C_id = '" + IdString + "'";
            connectionString = "datasource = kmsg007.iptime.org; port=3306; DATABASE=kmsg_inverter; username=root; password=kmsg2019";
            MySqlConnection myConn = new MySqlConnection(connectionString);
            myConn.Open();
            // MySqlCommand에 해당 SQL문을 지정하여 실행한다
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd1 = new MySqlCommand(strLogin, myConn);
                DBString = "select C_name, p.C_pcode, p.c_pname, C_id, C_pass from tbl_login as s left join tbl_ppcode as p on p.C_pcode = s.C_pcode where C_id = '" + IdString + "' and C_pass = '" + PwdString + "' and s.C_pcode < '71780006';";
                adapter.SelectCommand = new MySqlCommand(DBString, myConn);
                MySqlDataReader myReader;
                // MySqlDataReader는 연결모드로 데이터를 서버에서 가져온다.
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable table = ds.Tables[0];
                Rs = table.Rows;


                if (Rs != null && Rs.Count > 0)
                {
                    Constants.Username = Rs[0]["C_id"].ToString();
                    Constants.Password = Rs[0]["C_pass"].ToString();
                    strName = Rs[0]["C_name"].ToString();
                    DisplayAlert("로그인 성공", strName + "님 환영합니다", "확인");
                    myReader = cmd1.ExecuteReader();
                }
            }
            catch (Exception Ex)
            {
                myConn.Close();
                DisplayAlert("오류창", Ex.Message, "오류");
            }
            finally
            {
                if (Rs != null) { Rs.Clear(); Rs = null; }
                myConn.Close();
            }
        }








        bool AreCredentialsCorrect(User user)
        {
            return user.Username == Constants.Username && user.Password == Constants.Password;
        }
        protected override bool OnBackButtonPressed()
        {
            // true or false to disable or enable the action
            return base.OnBackButtonPressed();
        }
    }
}





/*https://phantom00.tistory.com/41 */