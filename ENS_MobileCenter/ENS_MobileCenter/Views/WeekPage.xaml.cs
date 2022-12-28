
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ENS_MobileCenter.Views.DayPage;
using System.Xml;
using C1.Xamarin.Forms.Grid;
using static Xamarin.Forms.Grid;
using C1.Xamarin.Forms.Core;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using C1.Xamarin.Forms.Chart.Annotation;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

namespace ENS_MobileCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class WeekPage : ContentPage
    {
        const int RefreshDuration = 2;
        bool isRefreshing;
        static string strPcode = PageLogin.strpcode;
        public ObservableCollection<string> Items { get; set; }
        static Hlib.CMariaDB DB = new Hlib.CMariaDB();
        static Hlib.CFunc Func = new Hlib.CFunc();
        public static string stDate, stpower, month, day, year, weekDay;
        public static int i, icnt, today;
        public static float[] fdat = new float[7];
        float m = fdat.Max();
        public WeekPage()
        {
            
            /*PopupNavigation.Instance.PushAsync(new Popupload());*/    
            try
            {
                PopupNavigation.PushAsync(new Popupload());
            }
            catch
            {
                DisplayAlert("연결 실패", "네트워크 확인이 필요합니다", "확인");
            }
            finally
            {
                InitializeComponent();
                PopupNavigation.PopAsync();
                // simulate long-running operation
                Task.Delay(2000);
                CreateChart();
                GetCreateGrid();
                this.pvChart.AxisY.Max = m * 1.1;
                this.pvChart.BindingX = "datetime";
                this.pvChart.ItemsSource = CreateChart();
                this.pvgrid.ItemsSource = GetCreateGrid();
                PVName.Text = PageLogin.strpvname;
                NowDate.Text = year + "년 " + month + "월 " + day + "일 " + weekDay + " 현재";
                pvChart.AnimationMode = C1.Xamarin.Forms.Chart.AnimationMode.All;
                C1Animation loadAnimation = new C1Animation();
                loadAnimation.Duration = new TimeSpan(1500 * 10000);
                loadAnimation.Easing = Xamarin.Forms.Easing.CubicInOut;
                pvChart.LoadAnimation = loadAnimation;
                
            }
            

            
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //refresh the data
            
        }

        

        public async void OnRefreshBtClicked(object sender, EventArgs e)
        {
            ReFresh();
            /*activityIndicator.IsRunning = false;*/
        }

        public void ReFresh()
        {
            PopupNavigation.Instance.PushAsync(new Popupload());
            try
            {
                pvChart.Refresh();
                pvgrid.Refresh();
                CreateChart();
                GetCreateGrid();
                this.pvChart.ItemsSource = CreateChart();
                this.pvgrid.ItemsSource = GetCreateGrid();
                this.pvChart.AxisY.Max = m * 1.1;
                PVName.Text = PageLogin.strpvname;
                NowDate.Text = year + "년 " + month + "월 " + day + "일 " + weekDay + " 현재";
                pvChart.AnimationMode = C1.Xamarin.Forms.Chart.AnimationMode.All;
                C1Animation loadAnimation = new C1Animation();
                loadAnimation.Duration = new TimeSpan(1500 * 10000);
                loadAnimation.Easing = Xamarin.Forms.Easing.CubicInOut;
                pvChart.LoadAnimation = loadAnimation;
            }
            finally
            {
                PopupNavigation.Instance.PopAsync();
            }
        }

        async void WeekPage_Refreshing(object sender, EventArgs e)
        {
            /*await Task.Delay(2000);*/
            await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
            ReFresh();
            RefreshPage.IsRefreshing = false;
        }
        public static IEnumerable<Chart> CreateChart()

        {
            string sqlWeek = @"select SUBSTR(D_date, 1, 10) AS '날짜', max(F_all_power) - MIN(F_all_power) AS '발전량'  FROM tbl_pvdat
                            where C_pcode  = '" + DayPage.strCode + "' and D_date between  '" + DateTime.Now.AddDays(-6) + "%' and '" + DateTime.Now.AddDays(+1) + "%' group by date_format(D_date, '%m%d')";
            DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
            today = Convert.ToInt32(DateTime.Now.ToString("dd"));
            List<Chart> entityList = new List<Chart>();
            DataRow[] rdb = DB.Fn_Select(sqlWeek);
            icnt = rdb.Length;//레코드갯수
            if (icnt > 0)
            {
                for (i = 0; i < 7; i++)
                {
                    stDate = Convert.ToString(rdb[i]["날짜"]);
                    stpower = Convert.ToString(rdb[i]["발전량"]);
                    fdat[i] = float.Parse(stpower);
                    Chart cd = new Chart();
                    cd.datetime = stDate.Substring(5, 5);
                    cd.powerdata = fdat[i];
                    entityList.Add(cd);
                }
            }
            return entityList;
        }

        public static IEnumerable<grid> GetCreateGrid()

        {
            {
                string sqlWeek = @"select SUBSTR(D_date, 1, 10) AS '날짜', max(F_all_power) - MIN(F_all_power) AS '발전량'  FROM tbl_pvdat
                            where C_pcode  = '" + DayPage.strCode + "' and D_date between  '" + DateTime.Now.AddDays(-6) + "%' and '" + DateTime.Now.AddDays(+1) + "%' group by date_format(D_date, '%m%d')";
                DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
                float[] fdat = new float[7];
                List<grid> GridList = new List<grid>();
                DataRow[] rdb = DB.Fn_Select(sqlWeek);
                icnt = rdb.Length;//레코드갯수
                if (icnt > 0)
                {
                    for (i = 0; i < 7; i++)
                    {
                        stDate = Convert.ToString(rdb[i]["날짜"]);
                        stpower = Convert.ToString(rdb[i]["발전량"]);
                        fdat[i] = float.Parse(stpower);
                        string f = "yyyyMMdd"; //날짜 형식
                        DateTime dateValue = new DateTime();
                        year = stDate.Substring(0, 4);
                        month = stDate.Substring(5, 2);
                        day = stDate.Substring(8, 2);
                        string datestring = string.Format(year + month + day); //날짜변환
                        dateValue = DateTime.ParseExact(datestring, f, null); //날짜변환
                        weekDay = Func.GetDayofWeek(dateValue); //요일
                        grid gd = new grid();
                        gd.gridday = stDate + " " + weekDay;
                        gd.gridpower = fdat[i];
                        gd.gridtime = string.Format("{0:0.#}", (fdat[i] / 99.9));
                        GridList.Add(gd);
                    }
                }
                return GridList;
            }
        }
    }
}