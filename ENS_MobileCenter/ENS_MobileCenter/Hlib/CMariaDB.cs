/*
 * 모듈이름: MySql 드라이버 
 * 작성일자: 2018.09.
 * 작성자명: Hwang KyuSeok
 * 
 * mysql-connector-net-8.0.12 다운로드
 * 프로젝트-참조-참조추가- MySql.Data.dll 찾아서 추가
 * 프로젝트-(프로젝트명)속성-대상프레임워크-4.5.2 로 (20/04/04)
 * 
 * 업데이트: 2019.09.17 (MariaDB)로...
 *         : 2020. 01. 16 
 *         : 2020. 04. 04(토)
 * 
 * kmsg01.iptime.org / 3306 / root / kmsg2015 
 * 
 *         
 * [참고] Connection  http://www.csharpstudy.com/Practical/Prac-mysql.aspx
 *                    https://doitforyou.tistory.com/57
 * [참고] 트랜잭션    https://slobell.com/blogs/41\
 *                    https://mainia.tistory.com/435
 * autocommit설정확인 show variables like 'autocommit%';
 *           설정or해지
 *           SET AUTOCOMMIT = TRUE; (설정)or FALSE;(해제)
 *           트랜잭션 COMMIT; ROLLBACK;
 *           설정파일 /etc/my.cnf.d/server.cnf에 
 *           [mysqld] autocommit=0  ->해제
 *           
 * **/
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;


namespace ENS_MobileCenter.Hlib
{
    
    public class CMariaDB
    {
        //private string strConn;
        public MySqlConnection sqlConn;

        /// <summary> DB 연결상태 </summary>
        public int isConnect = 0;
        
        //=========================================================================================
        /// <summary> DB Connection </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="dbname"></param>
        /// <param name="pass"></param>
        /// <returns>return 1:정상접속,  -1:오류</returns>
        //=========================================================================================
        public int fn_Open(string ip,int port, string dbname, string pass)
        {
            StringBuilder sbConn = new StringBuilder();
            int iret = 1;
            //strConn = "Data Source=127.0.0.1;Port=3306;Database=kmsg_inverter;User Id=root;Password=kmsg2015";//OK
            //strConn = @"Data Source=127.0.0.1:3306;Initial Catalog=kmsg_inverter;User ID=root;Password=kmsg2015";
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //strConn = "Data Source=127.0.0.1;Port=3306;Database=kmsg_inverter;Uid=root;Pwd=kmsg2015;CharSet=utf8;SSL Mode=None";//SSL Mode=Required
            sbConn.Append("Data Source=");
            sbConn.Append(ip);
            sbConn.Append(";Port=");
            sbConn.Append(port.ToString());
            sbConn.Append(";Database=");
            sbConn.Append(dbname);
            sbConn.Append(";Uid=root;Pwd=");
            sbConn.Append(pass);
            sbConn.Append(";CharSet=utf8;SSL Mode=None");
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            sqlConn = new MySqlConnection(sbConn.ToString());
            try
            {
                sqlConn.Open();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("-----------------------------------------------------" + e.Message.ToString());
                /*MessageBox.Show(e.Message.ToString());*/
                iret = -1;
            }
            isConnect = iret;
            return iret;
        }
        //=========================================================================================
        /// <summary> DB 접속귾기 </summary>
        //=========================================================================================
        public void fn_Close()
        {
            if(isConnect == 1) sqlConn.Close();
        }
        /*
        //SELECT OK
        public void fn_Sql()
        {
            string stmp;
            MySqlCommand cmd = new MySqlCommand("select count(*) as cnt from tbl_login",sqlConn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                ///stmp = reader[0].ToString();//OK
                stmp = reader["cnt"].ToString();
                MessageBox.Show(stmp);
            }
            reader.Close();
        }
        */
        /*
        //OK 
        public void fn_Sql()
        {
            string stmp;
            MySqlCommand cmd = new MySqlCommand("select * from tbl_login", sqlConn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ///stmp = reader[0].ToString();//OK
                stmp = reader["C_pcode"].ToString() + "_"+ reader[2].ToString() + "_" + reader[3].ToString();
                MessageBox.Show(stmp);
            }
            reader.Close();
        }
        */

        /*
        //OK
        public void fn_Sql()
        {
            string stmp="";
            MySqlCommand cmd = new MySqlCommand("select * from tbl_login", sqlConn);
            MySqlDataReader reader = cmd.ExecuteReader();
            var mySqlDataTable = new DataTable();
            mySqlDataTable.Load(reader);

            DataRow[] rows = mySqlDataTable.Select();
            for(int i=0;i<rows.Length;i++)
            {
                stmp += rows[i]["C_pcode"] + "__";
            }

            MessageBox.Show(stmp);
            reader.Close();
        }
        */

        /* 
        //Insert 예 OK
        public void fn_Sql()
        {
            string stmp;
            int i;
            MySqlCommand cmd = new MySqlCommand("INSERT INTO tbl_login (C_pcode,C_id,C_pass,C_name,C_telno,D_indate) VALUES ('20192018','kmsg2011','1233','씨발','010-543-1234','2019-1-4 23:12:00')", sqlConn);
            try
            {
                cmd.ExecuteNonQuery();//정상이면 1 리턴
                 

            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }

        }
        */
        /*
        //Insert 트랜잭션을 위한 예 OK
        public void fn_Sql()
        {
            string stmp;
            int i;
            MySqlCommand cmd = sqlConn.CreateCommand();
            MySqlTransaction tran;// = sqlConn.BeginTransaction();
            
            // Start a local transaction
            tran = sqlConn.BeginTransaction();
            
            // Must assign both transaction object and connection
            // to Command object for a pending local transaction
            cmd.Connection = sqlConn;
            cmd.Transaction = tran;
            try
            {
                cmd.CommandText = "INSERT INTO tbl_login (C_pcode,C_id,C_pass,C_name,C_telno,D_indate) VALUES ('20192025','kmsg2017','1233','씨발','010-543-1234','2019-1-4 23:12:00')";
                cmd.ExecuteNonQuery();//정상이면 1 리턴

                cmd.CommandText = "INSERT INTO tbl_login (C_pcode,C_id,C_pass,C_name,C_telno,D_indate) VALUES ('20192026','kmsg2018','1233','씨발','010-543-1234','2019-1-4 23:12:00')";
                i=cmd.ExecuteNonQuery();//정상이면 1 리턴

                tran.Commit();
                MessageBox.Show(i.ToString());
            }
            catch (Exception e)
            {
                tran.Rollback();
                MessageBox.Show(e.StackTrace);
            }
        }
        */
        //=========================================================================================
        /// <summary> 트랜잭션기능이 있는 Insert,Update 쿼리문수행 </summary>
        /// <param name="strSql">궈리문 string[]배열</param>
        /// <returns>integer 수행쿼리문갯수</returns>
        //=========================================================================================
        public int fn_ExecuteNonQuery(string[] strSql)
        {




            int iret=0;
            int i;
            MySqlCommand cmd = sqlConn.CreateCommand();
            MySqlTransaction tran = sqlConn.BeginTransaction();// Start a local transaction
            // Must assign both transaction object and connection
            // to Command object for a pending local transaction
            cmd.Connection = sqlConn;
            cmd.Transaction = tran;
            try
            {
                for(i=0;i< strSql.Length;i++)
                {
                    cmd.CommandText = strSql[i];
                    iret += cmd.ExecuteNonQuery();//정상이면 1 리턴
                }
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                iret = -1;
                Hlib.CUtil.fn_LogWrite("DB저장오류: " + e.Message);
            }
            finally { }
            return iret;
        }
        //=========================================================================================
        /// <summary> Select 쿼리문 실행 </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        //=========================================================================================
        public DataRow[] Fn_Select(string strsql)
        {
            MySqlCommand cmd = new MySqlCommand(strsql, sqlConn);
            MySqlDataReader reader = cmd.ExecuteReader();
            var mySqlDataTable = new DataTable();
            mySqlDataTable.Load(reader);
            DataRow[] rows = mySqlDataTable.Select();
            reader.Close();
            return rows;
        }
        /*
        //=========================================================================================
        /// <summary> Select 쿼리함수 [한재원] </summary>
        /// <param name="strSql"> 쿼리문 </param>
        /// <returns></returns>
        //=========================================================================================
        public DataTable fn_ExecuteReader2(string strSql)
        {
            DataTable Dt = new DataTable();
            MySqlCommand cmd = sqlConn.CreateCommand();
            cmd.Connection = sqlConn;
            try
            {
                cmd.CommandText = strSql;
                // MessageBox.Show(strSql);
                MySqlDataAdapter DataAdapter = new MySqlDataAdapter(cmd);
                MySqlCommandBuilder ComBuilder = new MySqlCommandBuilder(DataAdapter);
                DataAdapter.Fill(Dt);
            }
            catch (Exception e)
            {
                sqlConn.Close();
            }
            return Dt;
        }
        */
    }
}
