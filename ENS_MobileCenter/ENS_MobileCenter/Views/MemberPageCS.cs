using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ENS_MobileCenter.Views
{
    public class MemberPageCS : ContentPage
    {
        Entry usernameEntry,  mailEntry, phoneEntry;
        Label messageLabel;
        public MemberPageCS()
        {
            usernameEntry = new Entry
            {
                Placeholder = "사용자명 입력"
            };
            mailEntry = new Entry();
            phoneEntry = new Entry();
            var signUpButton = new Button
            {
                Text = "설정 저장"
            };
            signUpButton.Clicked += BtInsert_Click;

            Title = "Sign Up";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = {
                    new Label { Text = "사용자명" },
                    usernameEntry,
                    new Label { Text = "사용자 메일" },
                    mailEntry,
                    new Label { Text = "사용자 연락처" },
                    phoneEntry,
                    signUpButton,
                }
            };
        }

        async void BtInsert_Click(object sender, EventArgs e)
        {
            var user = new User()
            {
                Username = usernameEntry.Text,
                Email = mailEntry.Text,
                phone =  phoneEntry.Text,
            };

            CreateSign();

            // Sign up logic goes here

            var signUpSucceeded = AreDetailsValid(user);
            if (signUpSucceeded)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    Application.Current.MainPage = new AppShell();
                }
            }
            else
            {
                messageLabel.Text = "Sign up failed";
            }
        }

        public void CreateSign()
        {
            string strUpdate = @"update tbl_login SET C_name = '" + usernameEntry.Text + "',C_telno = '" + phoneEntry.Text + "',C_email = '" + mailEntry.Text + "'," +
                   " C_pass = '" + ShowPopupDemo.pwdText + "' where C_id = '" + PageLogin.IdString + "'";
            /*string strUpdate = @"update tbl_login SET C_name = 'choi2',C_telno = 'pnc001',C_email = 'pnc001',
                  C_pass = 'ens2022!' where C_id = 'choi'";*/
            MySqlConnection cnn = new MySqlConnection(PageLogin.connectionString);
            MySqlCommand cmd = new MySqlCommand(strUpdate, cnn);
            cnn.Open();
            try
            {

                cmd.Connection = cnn;
                cmd.CommandText = strUpdate;
                cmd.ExecuteNonQuery();
                cnn.Close();
                DisplayAlert("", "수정되었습니다.", "");
                usernameEntry.Text = string.Empty;
                phoneEntry.Text = string.Empty;
                mailEntry.Text = string.Empty;

            }
            catch (Exception ex)
            {
                cnn.Close();
                DisplayAlert("", ex.Message, "");
            }
            finally
            {
                cnn.Close();
            }
        }

        bool AreDetailsValid(User user)
        {
            return (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.phone) && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"));
        }
    }
}