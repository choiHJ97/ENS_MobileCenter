using MySqlConnector;
using System;
using System.Data;
using ENS_MobileCenter;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;


namespace ENS_MobileCenter.Views
{
    /*[XamlCompilation(XamlCompilationOptions.Compile)]*/
    public partial class PageLogin : ContentPage
    {
       public static string IdString, PwdString, strLogin, connectionString, DBString, strName, strpvname, strpcode;
        static bool checkpwd,checkId;
        public PageLogin()
        {
            InitializeComponent();
            /*NavigationPage.SetHasNavigationBar(this, false);*/
            if (Application.Current.Properties.ContainsKey("Password"))
            {
                var pwd = (string)Application.Current.Properties["Password"];
                passwordEntry.Text = pwd;
                PWDSave.IsChecked = checkpwd;
            }
            if (Application.Current.Properties.ContainsKey("SID"))
            {
                var id = (string)Application.Current.Properties["SID"];
                usernameEntry.Text = id;
                IDSave.IsChecked = checkId;
                /*IDSave.IsChecked = Application.Current.Properties.ContainsKey("SID");*/
            }
            if (usernameEntry.Text != string.Empty)
            {
                IDSave.IsChecked = true;
            }
            if (passwordEntry.Text != string.Empty)
            {
                PWDSave.IsChecked = true;
            }

            //SplashPage 상단 네비게이션 바를 안보이도록 설정
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //refresh the data
        }
        private static void NotSaveID()
        {
            Application.Current.Properties["SID"] = string.Empty;
            checkId = false;
            Application.Current.SavePropertiesAsync();
        }
        private void SaveID()
        {
            Application.Current.Properties["SID"] = this.usernameEntry.Text;
            checkId = true;
            Application.Current.SavePropertiesAsync();
        }
        private static void NotSavePWD()
        {
            Application.Current.Properties["Password"] = string.Empty;
            checkpwd = false;
            Application.Current.SavePropertiesAsync();
        }
        private void SavePWD()
        {
            Application.Current.Properties["Password"] = this.passwordEntry.Text;
            checkpwd = true;
            Application.Current.SavePropertiesAsync();
        }


        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                login();
            }
            catch
            {
                await DisplayAlert("로그인 실패", "네트워크 확인이 필요합니다", "확인");
            }
            
        }
        
        public void login()
        {
            IdString = this.usernameEntry.Text;
            PwdString = this.passwordEntry.Text;
            DBString = "select C_name, p.C_pcode, p.c_pname, C_id from tbl_login as s left join tbl_ppcode as p on p.C_pcode = s.C_pcode where C_id = '" + IdString + "' and C_pass = '" + PwdString + "' and s.C_pcode > '41424012';";
            strLogin = "update tbl_login set D_indate  = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where C_id = '" + IdString + "'";
            connectionString = "datasource = kmsg007.iptime.org; port=3306; DATABASE=kmsg_inverter; username=root; password=kmsg2019";
            MySqlConnection myConn = new MySqlConnection(connectionString);
            // MySqlCommand에 해당 SQL문을 지정하여 실행한다
            MySqlCommand SelectCommand = new MySqlCommand(DBString, myConn);
            MySqlCommand cmd1 = new MySqlCommand(strLogin, myConn);
            // MySqlDataReader는 연결모드로 데이터를 서버에서 가져온다.
            MySqlDataReader myReader;
            MySqlDataReader myReader2;
            myConn.Open();
            myReader = SelectCommand.ExecuteReader();

            int count = 0;
            while (myReader.Read())
            {
                count = count + 1;
                strName = myReader.GetString("C_name"); //C_name(사용자 이름) 
                strpvname = myReader.GetString("C_pname");//발전소명
                strpcode = myReader.GetString("C_pcode"); //발전소 코드
            }

            if (count == 1)
            {
                if (Convert.ToInt32(strpcode) >= 71780001 && Convert.ToInt32(strpcode) <= 71780005)
                {
                    PopupNavigation.PushAsync(new ProgressBarPopup());
                }
                else if (Convert.ToInt32(strpcode) == 71780000)
                {
                    DisplayAlert("준비중", "Kstar 현재 준비중", "확인");
                }
                else if (Convert.ToInt32(strpcode) >= 71780006 && Convert.ToInt32(strpcode) <= 717800010)
                {
                     DisplayAlert("준비중", "Kaco 현재 준비중", "확인");
                }
                else
                {
                    DisplayAlert("로그인 실패", "현재 준비중", "확인");
                }
                myReader.Close();
                myReader2 = cmd1.ExecuteReader();
                Hlib.CUtil.fn_LogWrite("로그인 ID : " + IdString);
            }
            else if (count > 1)
            {
                DisplayAlert("로그인 실패", "아이디 혹은 비밀번호 중복", "확인");
                messageLabel.Text = "로그인 실패";
                passwordEntry.Text = string.Empty;
            }
            else
            {
                DisplayAlert("로그인 실패", "아이디 혹은 비밀번호가 틀렸습니다", "확인");
                messageLabel.Text = "로그인 실패";
                passwordEntry.Text = string.Empty;
            }
            if (IDSave.IsChecked)
            {
                SaveID();
            }
            else
            {
                NotSaveID();
            }
            if (PWDSave.IsChecked)
            {
                SavePWD();
            }
            else
            {
                NotSavePWD();
            }
        }



        bool AreCredentialsCorrect(User user)
        {
            return user.Username == Constants.Username && user.Password == Constants.Password;
        }
      /* protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("종료", "종료하시겠습니까?", "취소", "확인");

                if (result == false)
                {

                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); // Or anything else
                }
                else
                {
                    await Navigation.PopAsync(true);
                }
            });
            return true;
        }*/
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryOutlined), null);

        private void passwordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(passwordEntry.Text.Length > 3)
            {
                messageLabel.Text = string.Empty;
            }
        }

        private void usernameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (usernameEntry.Text.Length > 3)
            {
                messageLabel.Text = string.Empty;
            }
        }

        private void IDSave_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            /*if (IDSave.IsChecked == true)
            {
                SaveID();
            }
            else
            {
                NotSaveID();
            }*/
            
        }


        private void PWDSave_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
           /* if (PWDSave.IsChecked == true)
            {
                SavePWD();
            }
            else
            {
                NotSavePWD();
            }*/
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryOutlined), null);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(EntryOutlined), Color.Blue);

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(EntryOutlined), Color.Blue);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }




    }

}





/*https://phantom00.tistory.com/41 */
/*https://stackoverflow.com/questions/32163471/confirmation-dialog-on-back-button-press-event-xamarin-forms*/












/* public async void OnLoginButtonClicked(object sender, EventArgs e)
 {
     try
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
             *//*Navigation.InsertPageBefore(new MainPage(), this);
             await Navigation.PopAsync();*/
/*Application.Current.MainPage = new AppShell();*/
/*Application.Current.MainPage = new AppShell();*/
/* await Navigation.PushAsync(new AppShell());*//*
*/               /* await Navigation.PushModalAsync(new AppShell());*//*
           }
       else
       {
           messageLabel.Text = "로그인 실패";
           passwordEntry.Text = string.Empty;
       }
   }
       catch (Exception Ex)
       {
          await DisplayAlert("오류", "서버에 연결하는 중 오류가 발생하였습니다", "확인");
       }
   }*/

/*  public void Connet()
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
              strpvname = Rs[0]["C_pname"].ToString();//발전소명
              strName = Rs[0]["C_name"].ToString();
              PopupNavigation.PushAsync(new ProgressBarPopup());
              *//*DisplayAlert("로그인 성공", strName + "님 환영합니다", "확인");*//*
              myReader = cmd1.ExecuteReader();
          }
      }
      catch (Exception Ex)
      {
          myConn.Close();
      }
      finally
      {
          if (Rs != null) { Rs.Clear(); Rs = null; }
          myConn.Close();
      }
  }*/