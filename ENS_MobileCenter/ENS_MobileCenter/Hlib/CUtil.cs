/*
 * 모듈이름: 
 * 작성일자: 2019.
 * 작성자명: Hwang KyuSeok
 * 
 * **/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENS_MobileCenter.Hlib
{
    class CUtil
    {

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Log디렉토리 생성후 Log_yymmdd.log 파일생성후 | [날싸시간] 로그값 등록됨
        /// this.GetType().Name.ToString() 을 사용하여 발생한 클래스이름도 같이 로그에 남기자!
        /// </summary>
        /// <param name="str">로그로남길문자</param>
        //-----------------------------------------------------------------------------------------
        public static void fn_LogWrite(string str)
        {
            string DirPath = Environment.CurrentDirectory + @"\Log";
            string FilePath = DirPath + "\\Log_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string temp;
            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);
            string ntime = DateTime.Now.ToString("HH:mm:ss");
            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        //temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        temp = string.Format("[{0}] {1}", ntime, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        //temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        temp = string.Format("[{0}] {1}", ntime, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e) { e.ToString(); }
        }
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// byte형 배열값 float 데이터로 변환
        /// </summary>
        /// <param name="pdat">버퍼배열</param>
        /// <param name="startindex">시작번지</param>
        /// <param name="plen">byte수 2,4</param>
        /// <param name="pmode">스케일 1,10,100</param>
        /// <returns></returns>
        //-----------------------------------------------------------------------------------------
        public static float fnConversion(byte[] pdat, int startindex, int plen, float pmode) // int pmode -> float pmode
        {
            float ftmp = 0.0f;
            byte[] dat = new byte[plen];
            for (int i = 0; i < plen; i++) dat[i] = pdat[startindex + i];
            //값의 정렬순서른 반대로 바꿈
            Array.Reverse(dat);

            //2byte or 4byte의 byte형을 2btye integer 형으로
            ftmp = BitConverter.ToInt16(dat, 0) / pmode;
            return ftmp;
        }
        //=========================================================================================
        /// <summary> int형을 2byte의 Byte배열로 리턴 </summary>
        /// <param name="ivalue"></param>
        /// <returns></returns>
        //=========================================================================================
        public static byte[] fnIntToByte(int ivalue)
        {
            byte[] ret = BitConverter.GetBytes((Int16)ivalue);
            Array.Reverse(ret);
            return ret;
        }
        //-----------------------------------------------------------------------------------------


        /*
        //delegate void AppendTextDelegate(Control ctrl, string s);
        //AppendTextDelegate _textAppender;
        ///_textAppender = new AppendTextDelegate(AppendText);
        //-----------------------------------------------------------------------------------------
        // 
        //-----------------------------------------------------------------------------------------
        void AppendText(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);
            else
            {
                string source = ctrl.Text;
                ctrl.Text = source + Environment.NewLine + s;
            }
        }
        */
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// ON_OFF_Log_File디렉토리 생성후 ON_OFF_Log_list_(MMdd).log 파일생성하면 해당날짜 | [날싸시간] 로그값 등록됨
        /// </summary>
        /// <param name="str">로그로남길문자</param>
        //-----------------------------------------------------------------------------------------

        public static void fn_ON_OFFLogWrite(string str)
        {
            DateTime NOW_TIME = DateTime.Now;
            string YearFile = NOW_TIME.Year.ToString();
            string DirPath = Environment.CurrentDirectory + @"\ON_OFF_Log_File\" + YearFile;
            string FilePath = DirPath + "\\ON_OFF_Log_list_(" + NOW_TIME.ToString("MMdd") + ").log";
            string temp;
            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);
            string ntime = DateTime.Now.ToString("HH:mm:ss");
            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        //temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        temp = string.Format("[{0}] {1}", ntime, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        //temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        temp = string.Format("[{0}] {1}", ntime, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e) { e.ToString(); }
        }

    }
}
