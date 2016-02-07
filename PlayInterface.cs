using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//引用System.Data.SqlClient命名空間

namespace Youtube_Media_Player
{
    public partial class 播放界面 : Form
    {
        mainform Mainform;

        public 播放界面(mainform Mainform)
        {
            InitializeComponent();

            //歷史紀錄
            Mainform.historylistName.Add(Mainform.playlistTitle[Mainform.currentPlay]);
            //Mainform.playlistName.Add(Mainform.playlistTitle[Mainform.currentPlay]);

            string cn = @"Data Source=(LocalDB)\v11.0;" +
                "AttachDbFilename=|DataDirectory|Database1.mdf;" +
                "Integrated Security=True";

            SqlConnection db = new SqlConnection(cn);//建立連接物件
            db.Open();   //使用Open方法開啟和資料庫的連接
            SqlCommand cmd = new SqlCommand("INSERT INTO 紀錄(順序,帳號,密碼,個人歌單,歌單ID,歷史紀錄,紀錄ID) VALUES(" +
              Mainform.i + ",'" + Mainform.acc + "'" + ",'" + Mainform.psw + "'" + "," + "NULL" + "," + "NULL" +
              ",'" + Mainform.playlistTitle[Mainform.currentPlay] + "'" + ",'" + Mainform.playlistId[Mainform.currentPlay] + "')", db);
            cmd.ExecuteNonQuery();
            Mainform.i++;
            db.Close();   //使用Close方法關閉和資料庫的連接
           
        }


        private void 播放界面_Load(object sender, EventArgs e)
        {
            //設定timer
            timer1.Enabled = true;

            
        }
        //
        public void setForm(mainform f)
        {
            Mainform = f;
  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("播放"))
            {
                axShockwaveFlash1.Movie = "https://youtube.googleapis.com/v/" + Mainform.nowPlay + "?autoplay=1&version=3&enablejsapi=1";

                try
                {
                    string x = axShockwaveFlash1.CallFunction("<invoke name=\"playVideo\" returntype=\"xml\"></invoke>");
                }
                catch
                {
                }

                button1.Text = "暫停";

            }
            else
            {

                try
                {
                    string x = axShockwaveFlash1.CallFunction("<invoke name=\"pauseVideo\" returntype=\"xml\"></invoke>");
                }
                catch
                {
                }
                button1.Text = "播放";

            }
        }
        //中止
        private void button11_Click(object sender, EventArgs e)
        {
            Mainform.currentPlay = 0;
            try
            {
                axShockwaveFlash1.CallFunction("<invoke name=\"stopVideo\" returntype=\"xml\"></invoke>");
                button1.Text = "播放";
            }
            catch
            {
            }

            Mainform.listenPage.showPlaylist();
        }
        //下一首
        private void button12_Click(object sender, EventArgs e)
        {
            Mainform.currentPlay++ ;    //換下一首

            if (Mainform.currentPlay == Mainform.playlistId.Count)  
            {
                Mainform.currentPlay = 0;               //超過範圍
            }

            Mainform.nowPlay = Mainform.playlistId[Mainform.currentPlay];   //存下一首代碼

            if (Mainform.nextPlay != "")
            {
                axShockwaveFlash1.Movie = "https://youtube.googleapis.com/v/" + Mainform.nowPlay + "?autoplay=1&version=3&enablejsapi=1";

                try
                {
                    string x = axShockwaveFlash1.CallFunction("<invoke name=\"playVideo\" returntype=\"xml\"></invoke>");
                }
                catch
                {
                }
            }
            Mainform.listenPage.showPlaylist();
            
            //歷史紀錄
            Mainform.historylistName.Add(Mainform.playlistTitle[Mainform.currentPlay]);
            //Mainform.playlistName.Add(Mainform.playlistTitle[Mainform.currentPlay]);

            string cn = @"Data Source=(LocalDB)\v11.0;" +
                "AttachDbFilename=|DataDirectory|Database1.mdf;" +
                "Integrated Security=True";

            SqlConnection db = new SqlConnection(cn);//建立連接物件
            db.Open();   //使用Open方法開啟和資料庫的連接
            SqlCommand cmd = new SqlCommand("INSERT INTO 紀錄(順序,帳號,密碼,個人歌單,歌單ID,歷史紀錄,紀錄ID) VALUES(" +
              Mainform.i + ",'" + Mainform.acc + "'" + ",'" + Mainform.psw + "'" + "," + "NULL" + "," + "NULL" +
              ",'" + Mainform.playlistTitle[Mainform.currentPlay] + "'" + ",'" + Mainform.playlistId[Mainform.currentPlay] + "')", db);
            cmd.ExecuteNonQuery();
            Mainform.i++;
            db.Close();   //使用Close方法關閉和資料庫的連接

        }
        //上一首
        private void button13_Click(object sender, EventArgs e)
        {

            Mainform.currentPlay--;    //換上一首

            if (Mainform.currentPlay<0)
            {
                Mainform.currentPlay = Mainform.playlistId.Count -1;               //超過範圍
            }

            Mainform.nowPlay = Mainform.playlistId[Mainform.currentPlay];   //存下一首代碼

            if (Mainform.prePlay != "")
            {
                axShockwaveFlash1.Movie = "https://youtube.googleapis.com/v/" + Mainform.nowPlay + "?autoplay=1&version=3&enablejsapi=1";

                try
                {
                    string x = axShockwaveFlash1.CallFunction("<invoke name=\"playVideo\" returntype=\"xml\"></invoke>");
                }
                catch
                {
                }
            }

            Mainform.listenPage.showPlaylist();
        }





        private void button2_Click(object sender, EventArgs e)
        {

            string x="";
            try
            {
                x = axShockwaveFlash1.CallFunction("<invoke name=\"getPlayerState\" returntype=\"xml\"></invoke>");
              //  label2.Text = x.Length + x;
                if (x.Equals("<number>1</number>"))
                {
                    label2.Text = "撥放中";
                }
                else
                {
                    label2.Text = x.Length + x;
                }


            }
            catch
            {
                x = "xxx";
            }
         //   string x = axShockwaveFlash1.CallFunction("<invoke name=\"pauseVideo\" returntype=\"xml\"></invoke>");
              
            //label2.Text = "1+"+x;
        
        }

        //監測撥放器狀態

        private void timer1_Tick(object sender, EventArgs e)
        {
       
            string x = "";
            //得到撥放器狀態
            try
            {
                x = axShockwaveFlash1.CallFunction("<invoke name=\"getPlayerState\" returntype=\"xml\"></invoke>");
                //  label2.Text = x.Length + x;
            }
            catch
            {
                x = "xxx";
            }

            if (x.Equals("<number>1</number>"))
            {
                label2.Text = "撥放中";
            }
            else if (x.Equals("<number>0</number>"))    //已結束 換下一首
            {
                label2.Text = "換歌請等待";

                Mainform.currentPlay++;    //換下一首

                if (Mainform.currentPlay == Mainform.playlistId.Count)
                {
                    Mainform.currentPlay = 0;               //超過範圍
                }

                Mainform.nowPlay = Mainform.playlistId[Mainform.currentPlay];   //存下一首代碼

                if (Mainform.nextPlay != "")
                {
                    axShockwaveFlash1.Movie = "https://youtube.googleapis.com/v/" + Mainform.nowPlay + "?autoplay=1&version=3&enablejsapi=1";

                    try
                    {
                        string y = axShockwaveFlash1.CallFunction("<invoke name=\"playVideo\" returntype=\"xml\"></invoke>");
                    }
                    catch
                    {
                    }
                }
                Mainform.listenPage.showPlaylist();

            }
            else if (x.Equals("<number>-1</number>"))    //未開始
            {
                label2.Text = "未開始";
            }
            else if (x.Equals("<number>2</number>"))    //已暫停
            {
                label2.Text = "已暫停";
            }
            else if (x.Equals("<number>3</number>"))    //正在緩衝
            {
                label2.Text = "正在緩衝";
            }
            else if (x.Equals("<number>5</number>"))    //已插入視頻
            {
                label2.Text = "已插入視頻";
            }
        }

        private void button3_Click(object sender, EventArgs e)//like 存進歌單資料庫
        {
            Mainform.playlistName.Add(Mainform.playlistTitle[Mainform.currentPlay]);

            string cn = @"Data Source=(LocalDB)\v11.0;" +
                "AttachDbFilename=|DataDirectory|Database1.mdf;" +
                "Integrated Security=True";

            SqlConnection db = new SqlConnection(cn);//建立連接物件
            db.Open();   //使用Open方法開啟和資料庫的連接
            SqlCommand cmd = new SqlCommand("INSERT INTO 紀錄(順序,帳號,密碼,個人歌單,歌單ID,歷史紀錄,紀錄ID) VALUES(" +
              Mainform.i + ",'" + Mainform.acc + "'" + ",'" + Mainform.psw + "'" + ",'" + Mainform.playlistTitle[Mainform.currentPlay] + "'" + ",'" + Mainform.playlistId[Mainform.currentPlay] +
              "'" + "," + "NULL" + "," + "NULL" + ")", db);
            cmd.ExecuteNonQuery();
            Mainform.i++;
            db.Close();   //使用Close方法關閉和資料庫的連接
        }
        




    }
}
