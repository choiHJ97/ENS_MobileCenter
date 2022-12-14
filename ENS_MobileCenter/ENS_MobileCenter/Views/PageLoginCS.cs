using MySqlConnector;
using System;
using System.Data;
using Xamarin.Forms;

namespace ENS_MobileCenter.Views
{
    public class PageLoginCS : ContentPage
    {
        string IdString, PwdString, strLogin, connectionString, DBString, strName;
        Entry usernameEntry, passwordEntry;
        Label messageLabel;

        public PageLoginCS()
        {
            /*var toolbarItem = new ToolbarItem
            {
                Text = "Sign Up"
            };
            toolbarItem.Clicked += OnSignUpButtonClicked;
            ToolbarItems.Add(toolbarItem);*/

            messageLabel = new Label();
            usernameEntry = new Entry
            {
                Placeholder = "username"
            };
            passwordEntry = new Entry
            {
                IsPassword = true
            };
            var loginButton = new Button
            {
                Text = "Login"
            };
            loginButton.Clicked += OnLoginButtonClicked;

            Title = "Login";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = {
                    new Label { Text = "Username" },
                    usernameEntry,
                    new Label { Text = "Password" },
                    passwordEntry,
                    loginButton,
                    messageLabel
                }
            };
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageSignUpCS());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
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
                AppShell mainPageCS = new AppShell();
                Navigation.InsertPageBefore(mainPageCS, this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
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
                DBString = "select C_name, p.C_pcode, p.c_pname, C_id, C_pass from tbl_login as s left join tbl_ppcode as p on p.C_pcode = s.C_pcode where C_id = '" + IdString + "' and C_pass = '" + PwdString + "' and s.C_pcode < '71780006';";
                adapter.SelectCommand = new MySqlCommand(DBString, myConn);
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
                    DisplayAlert("확인", strName + "님 환영합니다", "확인");
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
        }

        bool AreCredentialsCorrect(User user)
        {
            return user.Username == Constants.Username && user.Password == Constants.Password;
        }
    }
}
