using C1.Xamarin.Forms.Core;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENS_MobileCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthPage : ContentPage
    {
        const int RefreshDuration = 2;
        bool isRefreshing;
        public ObservableCollection<string> Items { get; set; }
        static Hlib.CMariaDB DB = new Hlib.CMariaDB();
        static Hlib.CFunc Func = new Hlib.CFunc();
        string headername;
        public static string sqlMonth,stDate, stpower, month, year, weekDay;
        public static int i, y, m, icnt, totalDay;
        public static float[] fdat = new float[6];

        public MonthPage()
        {
            this.BindingContext = this;
            this.headername = "평균발전시간(h)";
            PopupNavigation.Instance.PushAsync(new Popupload());

            try
            {
                InitializeComponent();
                CreateChart();
                GetCreateGrid();
                this.pvChart.ItemsSource = CreateChart();
                this.pvgrid.ItemsSource = GetCreateGrid();
                PVName.Text = PageLogin.strpvname;
                NowDate.Text = year + "년 " + month + "월 " + "현재";
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //refresh the data
        }


        async void MonthPage_Refreshing(object sender, EventArgs e)
        {
            /*await Task.Delay(2000);*/
            await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
            Refresh();
            RefreshPage.IsRefreshing = false;
        }

        public void Refresh()
        {
            PopupNavigation.Instance.PushAsync(new Popupload());
            try
            {
                pvChart.Refresh();
            pvgrid.Refresh();
            this.pvChart.ItemsSource = CreateChart();
            this.pvgrid.ItemsSource = GetCreateGrid();
            PVName.Text = PageLogin.strpvname;
            NowDate.Text = year + "년 " + month + "월 " + "현재";
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

        public async void OnRefreshBtClicked(object sender, EventArgs e)
        {
            Refresh();
        }
        /*public void CreateChart()

        {
            DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
            string stDate, stpower, month, day, year, weekDay, y, m, d;
            int i,  icnt, today;
            today = Convert.ToInt32(DateTime.Now.ToString("dd"));
            string sqlWeek = @"select SUBSTR(D_date, 1, 10) AS '날짜', max(F_all_power) - MIN(F_all_power) AS '발전량'  FROM tbl_pvdat as c
                            left join tbl_login p on c.C_pcode = p.C_pcode
                            where C_id = '" + PageLogin.IdString + "' and D_date between  '" + DateTime.Now.AddDays(-6) + "%' and '" + DateTime.Now.AddDays(+1) + "%' group by date_format(D_date, '%m%d')";
            float[] fdat = new float[7];
            float sum;

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
                    cd.datetime = stDate.Substring(5,5);
                    cd.powerdata = fdat[i];
                    entityList.Add(cd);
                    grid gd = new grid();
                    gd.gridday = stDate;
                    gd.gridpower = fdat[i];
                    gd.gridtime = string.Format("{0:0.#}",(fdat[i] / 99.9));
                    DateTime dateValue = new DateTime();
                    NowDate.Text = strNow;
                    string f = "yyyyMMdd";
                    GridList.Add(gd);
                    sum = fdat.Sum();
                }
 
            }



        }*/
        public static IEnumerable<Chart> CreateChart()

        {
            sqlMonth = @"select SUBSTR(D_date, 1, 7) AS '날짜', max(F_all_power) - MIN(F_all_power) AS '발전량'  FROM tbl_pvdat as c
                            left join tbl_login p on c.C_pcode = p.C_pcode
                            where c.C_pcode  = '" + DayPage.strCode + "' and SUBSTR(D_date, 1, 7) between  '" + DateTime.Now.AddMonths(-5).ToString("yyyy-MM") + "' and '" + DateTime.Now.AddMonths(+1).ToString("yyyy-MM") + "' group by SUBSTR(D_date, 1, 7)";
            DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
            List<Chart> entityList = new List<Chart>();
            DataRow[] rdb = DB.Fn_Select(sqlMonth);
            icnt = rdb.Length;//레코드갯수
            if (icnt > 0)
            {
                for (i = 0; i < 6; i++)
                {
                    stDate = Convert.ToString(rdb[i]["날짜"]).Substring(2,5);
                    stpower = Convert.ToString(rdb[i]["발전량"]);
                    fdat[i] = float.Parse(stpower);
                    Chart cd = new Chart();
                    cd.datetime = stDate;
                    cd.powerdata = float.Parse(string.Format("{0:0.#}", (fdat[i] / 1000)));
                    entityList.Add(cd);
                }

            }
            return entityList;
        }

        public static IEnumerable<grid> GetCreateGrid()

        {
            {
                sqlMonth = @"select SUBSTR(D_date, 1, 7) AS '날짜', max(F_all_power) - MIN(F_all_power) AS '발전량'  FROM tbl_pvdat as c
                            left join tbl_login p on c.C_pcode = p.C_pcode
                            where c.C_pcode  = '" + DayPage.strCode + "' and SUBSTR(D_date, 1, 7) between  '" + DateTime.Now.AddMonths(-5).ToString("yyyy-MM") + "' and '" + DateTime.Now.AddMonths(+1).ToString("yyyy-MM") + "' group by SUBSTR(D_date, 1, 7)";
                DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
                List<grid> GridList = new List<grid>();
                DataRow[] rdb = DB.Fn_Select(sqlMonth);
                icnt = rdb.Length;//레코드갯수
                if (icnt > 0)
                {
                    for (i = 0; i < 6; i++)
                    {
                        stDate = Convert.ToString(rdb[i]["날짜"]);
                        stpower = Convert.ToString(rdb[i]["발전량"]);
                        fdat[i] = float.Parse(stpower);
                        year = stDate.Substring(0, 4);
                        month = stDate.Substring(5, 2);
                        y = int.Parse(year);
                        m = int.Parse(month);
                        totalDay = DateTime.DaysInMonth(y, m); // 월별 일 수
                        grid gd = new grid();
                        gd.gridday = stDate;
                        gd.strpowerdata = string.Format("{0:0.#}", (fdat[i] / 1000));
                        gd.gridtime = string.Format("{0:0.#}", (fdat[i] / 99.9 / totalDay));
                        GridList.Add(gd);
                    }
                }

                return GridList;

            }
        }
    }
}