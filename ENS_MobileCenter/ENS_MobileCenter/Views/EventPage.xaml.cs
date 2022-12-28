using C1.Xamarin.Forms.Chart;
using C1.Xamarin.Forms.Grid;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENS_MobileCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventPage : ContentPage
    {
        //public static string strArlam = @"select substr(D_date,12,5) as '시간', t1.I_kind as '항목', t1.I_state as '상태'  from tbl_pvalarm as t1 left join tbl_login t2 on t1.C_pcode = t2.C_pcode  where D_date like '2022-12-08%' and C_Id = '" + PageLogin.IdString + "'order by 시간"; // 이벤트 알림
        public static string strArlam;
        static Hlib.CFunc Func = new Hlib.CFunc();
        static Hlib.CUtil util = new Hlib.CUtil();
        static Hlib.CMariaDB DB = new Hlib.CMariaDB();
        static MySqlConnection cnn;
        static MySqlCommand cmd;
        static MySqlDataReader myReader;
        static MySqlDataAdapter da;
        static DataTable dt;
        public static string Time, kind, state, NewKind, NewState;
        public static int icnt;
        public EventPage()
        {
            InitializeComponent();
            pvgrid.ItemsSource = GetCreateGrid();
            PVName.Text = PageLogin.strpvname;
            DateLabel.Text = DayPage.strNow.Substring(0, 16);
            pvgrid.ColumnHeaderFontFamily = pvgrid.FontFamily = "SpoqaRegular";
        }

        public void Refresh()
        {
            pvgrid.Refresh();
            GetCreateGrid();
            pvgrid.ItemsSource = GetCreateGrid();
            PVName.Text = PageLogin.strpvname;
            DateLabel.Text = DayPage.strNow.Substring(0, 16);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //refresh the data
        }


        public async void OnRefreshBtClicked(object sender, EventArgs e)
        {
            Refresh();
        }


        public static IEnumerable<grid> GetCreateGrid()

        {
            {
                strArlam = @"select  row_number() over(order by substr(D_date,12,10) asc) as '번호' , substr(D_date,12,10) as '시간', t1.I_kind as '항목', t1.I_state as '상태'  from tbl_pvalarm as t1 left join tbl_login t2 on t1.C_pcode = t2.C_pcode  where C_id = '" + PageLogin.IdString + "' and D_date  LIKE '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'order by 시간";
                DB.fn_Open("kmsg007.iptime.org", 3306, "kmsg_inverter", "kmsg2019");
                List<grid> GridList = new List<grid>();
                DataRow[] rdb = DB.Fn_Select(strArlam);
                icnt = rdb.Length;
                for (int i = 0; i < icnt; i++)
                {
                    Time = Convert.ToString(rdb[i]["시간"]);
                    kind = Convert.ToString(rdb[i]["항목"]);
                    state = Convert.ToString(rdb[i]["상태"]);

                    switch (state)
                    {
                        case "0":
                            NewState = "해제";
                            break;
                        case "1":
                            NewState = "발생";
                            break;
                        default:
                            NewState = "Reseved";
                            break;
                    }
                    switch (kind)
                    { 
                        case "0":
                            NewKind = "계통전압 위상동기화 실패";
                            break;
                        case "1":
                            NewKind = "계통전압 과전압";
                            break;
                        case "2":
                            NewKind = "계통전압 저전압";
                            break;
                        case "3":
                            NewKind = "계통전압 과주파수";
                            break;
                        case "4":
                            NewKind = "계통전압 저주파수";
                            break;
                        case "5":
                            NewKind = "입력 과전압";
                            break;
                        case "6":
                            NewKind = "출력 과전류";
                            break;
                        case "7":
                            NewKind = "입력 과전류";
                            break;
                        case "8":
                            NewKind = "출력 과전류";
                            break;
                        case "9":
                            NewKind = "스택 과온도";
                            break;
                        case "10":
                            NewKind = "출력변압기 과온도";
                            break;
                        case "11":
                            NewKind = "출력필터 리액터 과온도";
                            break;
                        case "15":
                            NewKind = "MCU보드 통신연결 상태";
                            break;
                        case "20":
                            NewKind = "U상 전력소자 단락 검지";
                            break;
                        case "21":
                            NewKind = "V상 전력소자 단락 검지";
                            break;
                        case "22":
                            NewKind = "W상 전력소자 단락 검지";
                            break;
                        case "23":
                            NewKind = "입력 과전류 H/W 검지";
                            break;
                        case "24":
                            NewKind = "계통 과전류 H/W 검지";
                            break;
                        case "25":
                            NewKind = "입력 과전압 H/W 검지";
                            break;
                        case "26":
                            NewKind = "계통 전류 불편형 H/W 검지";
                            break;
                        case "27":
                            NewKind = "입력측 써지 전압 검지";
                            break;
                        case "28":
                            NewKind = "접지 고장";
                            break;
                        case "29":
                            NewKind = "계통측 써지 전압 검지";
                            break;
                        case "30":
                            NewKind = "전면부 도어 열림";
                            break;
                        case "31":
                            NewKind = "전면부 긴급정지 버튼 눌림";
                            break;
                        case "32":
                            NewKind = "운전 중 계통 MCB고장";
                            break;
                        case "33":
                            NewKind = "계통측 초기충전 MCB 고장";
                            break;
                        case "34":
                            NewKind = "계통측 전압 상순 오류";
                            break;
                        case "40":
                            NewKind = "DC 입력 측 차단기 상태";
                            break;
                        case "41":
                            NewKind = "AC 출력 측 차단기 상태";
                            break;
                        case "42":
                            NewKind = "MPPT 모드 상태";
                            break;
                        case "43":
                            NewKind = "DOOR OPEN 상태";
                            break;
                        case "48":
                            NewKind = "S/W W상 Gate Fault";
                            break;
                        case "49":
                            NewKind = "S/W V상 Gate Fault";
                            break;
                        case "50":
                            NewKind = "S/W U상 Gate Fault";
                            break;

                        default:
                            NewKind = "Reseved";
                            break;
                    }
                    grid gd = new grid();
                    gd.Time = Time;
                    gd.NewKind = NewKind;
                    gd.NewState = NewState;
                    GridList.Add(gd);
                    
                }
                return GridList;

            }
                
            }
        }

    }
    

