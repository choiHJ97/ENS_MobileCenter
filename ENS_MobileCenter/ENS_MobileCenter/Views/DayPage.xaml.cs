using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;
using System.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;
using System.Windows.Input;
using C1.Xamarin.Forms.Chart;
using System.Xml;
using static ENS_MobileCenter.Views.DayPage;
using C1.Xamarin.Forms.Chart.Annotation;
using System.Xml.Linq;
using C1.Xamarin.Forms.Core;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

namespace ENS_MobileCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayPage : ContentPage
    {
        const int RefreshDuration = 2;
        bool isRefreshing;
        static Hlib.CFunc Func = new Hlib.CFunc();
        static Hlib.CUtil util = new Hlib.CUtil();
        static Hlib.CMariaDB DB = new Hlib.CMariaDB();
        string strDayView;
        public static string strNow, strCode;
        static float fDayPower;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public DayPage()
        {
            try
            {
                InitializeComponent();
                DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
                PVData();
                this.pvChart.BindingX = "datetime";
                this.pvChart.ItemsSource = CreateChart();
                PVName.Text = PageLogin.strpvname;
                pvChart.AnimationMode = C1.Xamarin.Forms.Chart.AnimationMode.All;
                C1Animation loadAnimation = new C1Animation();
                loadAnimation.Duration = new TimeSpan(1500 * 10000);
                loadAnimation.Easing = Xamarin.Forms.Easing.CubicInOut;
                pvChart.LoadAnimation = loadAnimation;
            }
            catch
            {
                DisplayAlert("연결 실패", "네트워크 확인이 필요합니다", "확인");
            }
          
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //refresh the data
        }

        public static IEnumerable<Chart> CreateChart()
        {
                Hlib.CMariaDB DB = new Hlib.CMariaDB();
                DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
                string strQuery = @"select substr(D_date,12,5) as 시간, ifnull((F_day_power/10) - (lag(F_day_power,1)over(order by F_day_power)/10),0) as '발전량', c.C_pcode as '코드'
                             from tbl_pvdat as c
                             left join tbl_login p on c.C_pcode = p.C_pcode  
                             where C_id = '" + PageLogin.IdString + "' and D_date  LIKE '" + DateTime.Now.ToString("yyyy-MM-dd") + "%' group by date_format(D_date, '%y%m%d%H시')";
                string stpower, stDate, chartDate;
                float[] fdat = new float[24];
            int i, h, icnt;
                DataRow[] rdb = DB.Fn_Select(strQuery);
                icnt = rdb.Length;//레코드갯수
                List<Chart> entityList = new List<Chart>();
                if (icnt > 0)
                {
                    for (i = 0; i < icnt; i++)
                    {
                        stDate = Convert.ToString(rdb[i]["시간"]);
                        stpower = Convert.ToString(rdb[i]["발전량"]);
                    strCode = Convert.ToString(rdb[1]["코드"]);
                    h = int.Parse(stDate.Substring(0, 2)); //시간 
                        fdat[h] = float.Parse(stpower); //0시 =  배열 0번, 0 ~ 23시 시간당 발전량
                    }

                    for (i = 4; i < 24; i++)
                    {
                        Chart cd = new Chart();
                        chartDate = string.Format(i + "h");
                        cd.datetime = chartDate;
                        cd.powerdata = fdat[i];
                        entityList.Add(cd);
                    }
            }
            return entityList;
        }

        void AddItems()
        {
            PVData();
            pvChart.Refresh();
            this.pvChart.BindingX = "datetime";
            this.pvChart.ItemsSource = CreateChart();
            PVName.Text = PageLogin.strpvname;
            pvChart.AnimationMode = C1.Xamarin.Forms.Chart.AnimationMode.All;
            C1Animation loadAnimation = new C1Animation();
            loadAnimation.Duration = new TimeSpan(1500 * 10000);
            loadAnimation.Easing = Xamarin.Forms.Easing.CubicInOut;
            pvChart.LoadAnimation = loadAnimation;
        }


        async void DayPage_Refreshing(object sender, EventArgs e)
        {
            /*await Task.Delay(2000);*/
            await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
            AddItems();
            RefreshPage.IsRefreshing = false;
        }

        public async void OnRefreshBtClicked(object sender, EventArgs e)
        {
            AddItems();
        }


        public void PVData()
        {
            string strSql = @"select substr(D_date,1,16) as '날짜', F_rpower as '순간출력', F_all_power as '누적발전량' from tbl_pvdat as c
left join tbl_login p on c.C_pcode = p.C_pcode
where C_id = '" + PageLogin.IdString + "' and c.D_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%' group by substr(c.D_date, 12, 5)";
            string strToday = "select substr(날짜,1,4) as 연, substr(날짜,6,2) as 월, substr(날짜,9,2) as 일 , max(누적발전량) - MIN(누적발전량) as '발전량' from(" + strSql + ")as t1 group by date_format(t1.날짜, '%m%d')";
            string strArlam = @"select count(*) as cnt FROM (select substr(D_date,12,5) as '시간', t1.I_kind as '항목', t1.I_state as '상태'  from tbl_pvalarm as t1 left join tbl_login t2 on t1.C_pcode = t2.C_pcode  where D_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + @"%' and C_Id = '" + PageLogin.IdString + "'order by 시간)as t1"; // 이벤트 알림
            string myConnection = PageLogin.connectionString; //DB
            string month, year, day;
            int totalDay, icnt, acnt;
            MySqlConnection cnn;
            MySqlCommand cmd;
            MySqlDataReader myReader;
            cnn = new MySqlConnection(myConnection);
            cmd = new MySqlCommand(strToday);
            cmd.Connection = cnn;
            cmd.CommandText = strToday;
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                fDayPower = myReader.GetFloat("발전량"); //발전량
                double dTime = Math.Round(fDayPower / 99.9, 2); //금일 발전 시간 발전량 / 99.9
                year = myReader.GetString("연");
                month = myReader.GetString("월");
                day = myReader.GetString("일");
                totalDay = DateTime.DaysInMonth(int.Parse(year), int.Parse(month)) + 1;
                float[] fdat = new float[totalDay];
                DataRow[] rdb = DB.Fn_Select(strSql);
                DataRow[] rdb2 = DB.Fn_Select(strArlam);
                icnt = rdb.Length;//레코드갯수
                acnt = rdb2.Length;//레코드갯수
                string Allpower = Convert.ToString(rdb[icnt - 1]["순간출력"]);
                string nowTime = Convert.ToString(rdb[icnt - 1]["날짜"]).Substring(11, 5);
                string f = "yyyyMMdd"; //날짜 형식
                DateTime dateValue = new DateTime();
                string strday = Convert.ToString((String.Format("{0:00}", day))); //일 두자리수 변환
                string datestring = string.Format(year + month + strday); //날짜변환
                dateValue = DateTime.ParseExact(datestring, f, null); //날짜변환
                string weekDay = Func.GetDayofWeek(dateValue); //요일
                strNow = year + "년 " + month + "월 " + strday + "일" + weekDay + " " + nowTime + " 현재";
                DateLabel.Text = strNow; //날짜
                daypower.Text = fDayPower.ToString(); // 금일 발전량
                daytime.Text = dTime.ToString() + " 시간"; //금일 발전시간
                rpower.Text = Allpower + " kW"; //현재 순간발전`
                int alramCount = Convert.ToInt32(rdb2[acnt - 1]["cnt"]);
                alramcount.Text = alramCount.ToString() + " 건";
            }
            myReader.Close();
            cnn.Close();
            string strURL = "https://www.kma.go.kr/wid/queryDFSRSS.jsp?zone=4713025000";
            todayWeather.Text = Func.WeatherText(strURL); // 날씨 아이콘 띄우기
        }
    }
}

    

