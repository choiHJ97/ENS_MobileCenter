using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; //스트림
using System.Xml;
using System.Net; //통신을 위해서 필요함
using System.Drawing;
using Xamarin.Forms;

namespace ENS_MobileCenter.Hlib
{
    public class CFunc
    {
        string weekDay;
        public string GetDayofWeek(DateTime dateValue)
        {
            string weekDay = "";
            var dt = dateValue.DayOfWeek;
            switch (dt)
            {
                case DayOfWeek.Monday: //월요일
                    weekDay = "(월)";
                    break;
                case DayOfWeek.Tuesday: //화요일
                    weekDay = "(화)";
                    break;
                case DayOfWeek.Wednesday: //수요일
                    weekDay = "(수)";
                    break;
                case DayOfWeek.Thursday: //목요일
                    weekDay = "(목)";
                    break;
                case DayOfWeek.Friday: //금요익
                    weekDay = "(금)";
                    break;
                case DayOfWeek.Saturday: //토요일
                    weekDay = "(토)";
                    break;
                case DayOfWeek.Sunday: //일요일
                    weekDay = "(일)";
                    break;
            }
            return weekDay;
        }


      
        public string WeatherText(string strURL)
        {
            string stWeather = "";
            WebRequest wr = WebRequest.Create(strURL);
            wr.Method = "GET";

            //Response를 받는다!
            WebResponse wrs = wr.GetResponse();
            Stream s = wrs.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            string response = sr.ReadToEnd();

            //richTextBox1.Text = response;

            //response 받은 것을 xml 파싱한다!

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(response);

            //제목이랑 중요한정보 처리하는 부분
            XmlNode channel = xd["rss"]["channel"];
            string city = (channel["title"].InnerText);
            string date = channel["pubDate"].InnerText;

            //데이터 처리하는 부분
            XmlNode xn = xd["rss"]["channel"]["item"]["description"]["body"];

            for (int i = 0; i < 4; i++)
            {
                string strWs = xn.ChildNodes[i]["ws"].InnerText;
                string wf = xn.ChildNodes[1]["wfKor"].InnerText;
                string temp = xn.ChildNodes[i]["temp"].InnerText + "'C";
                stWeather = wf + "  "+ temp;
            }
            return stWeather;
        }
    }
}
