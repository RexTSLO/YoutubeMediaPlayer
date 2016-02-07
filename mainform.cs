using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;

using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.GData.Extensions;
using Google.GData.Client;

using System.Data.SqlClient;//引用System.Data.SqlClient命名空間


namespace Youtube_Media_Player
{
    public partial class mainform : Form
    {

        public string nowPlay = "", nextPlay = "", prePlay = "", iniUrl = "";
        public int ResultNumber = 0;            //紀錄有幾個回傳結果
        public int Page = 0;                    //紀錄回傳結果的頁數
        public int ResultIndex = 0;             //紀錄第幾筆回傳結果  
        public int playingTrue = 0;             //是否正在播放
        public int currentPlay = 0;              //播放到第幾首

        public List<string> videos;
        public List<string> channels;
        public List<string> videoTitle = new List<string>();             //搜尋到影片名
        public List<string> videoId    = new List<string>();                //搜尋到影片代碼
        public List<ThumbnailDetails> vedioThumb = new List<ThumbnailDetails>();

        public List<string> historyVideoTitle = new List<string>();             //歷史紀錄的影片名稱
        public List<string> historyVideoid    = new List<string>();             //歷史紀錄的影片代碼

        public List<string> playlistTitle    = new List<string>();             //現在播放歌單名稱
        public List<string> playlistId       = new List<string>();              //現在播放歌單影片代碼
        
        public int temp = 5;
        public 播放界面 playInterface;
        public listenlist listenPage;

        public string acc = null, psw = null;
        public int i = 3;
        public List<string> playlistName = new List<string>();
        public List<string> historylistName = new List<string>();

        public mainform()
        {
            InitializeComponent();

            nowPlay = "qXv7POo5MNI";
            nextPlay = "8E1x4CzsbaQ";
            prePlay = "e9cNXZ-o4yo";

            playlistName.Add("蘇打綠-小情歌(台)");
            playlistName.Add("五月天-忘詞");

            //初始值放三首歌(測試用)

            playlistTitle.Add("蘇打綠-小情歌(台)");
            playlistTitle.Add("五月天-忘詞");
            //playlistTitle.Add("五月天-聽不到");
            playlistId.Add("qXv7POo5MNI");
            playlistId.Add("8E1x4CzsbaQ");
            //playlistId.Add("U-C0Z3ys_C8");*/
   
 
            //初始設定
            pictureBox1.Visible = false; label3.Visible = false; button1.Visible = false;
            pictureBox4.Visible = false; label6.Visible = false; button8.Visible = false;
            pictureBox6.Visible = false; label8.Visible = false; button10.Visible = false;
            pictureBox2.Visible = false; label4.Visible = false; button6.Visible = false;
            pictureBox3.Visible = false; label5.Visible = false; button7.Visible = false;
            pictureBox5.Visible = false; label7.Visible = false; button9.Visible = false;
        }

        private void mainform_Load(object sender, EventArgs e)
        {
            string cn = @"Data Source=(LocalDB)\v11.0;" +
                "AttachDbFilename=|DataDirectory|Database1.mdf;" +
                "Integrated Security=True";

            SqlConnection db = new SqlConnection(cn);//建立連接物件
            //da帶入查詢的SQL語法為toolStripTextBox1文字方塊的內容
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM 紀錄", db);
            //建立DataSet物件ds
            DataSet ds = new DataSet();
            //將da物件所取得的資料填入ds物件
            da.Fill(ds);
        }

        //Search
        private async  void button1_Click(object sender, EventArgs e)
       // private  void button1_Click(object sender, EventArgs e)
        {

        
            if (textBox1.Text != "")
            {
                await Run();             //搜尋
        //  textBox2.Text = textBox2.Text + "有" + videoId.Count() + "筆資料";
        //        SearchPage   searchPage =new SearchPage();
        //        searchPage.Show();
        //        searchPage.setForm(this);
                
                Page = 1;
                
                ResultNumber = videoId.Count;
                if (ResultNumber > 6)
                {
                    button11.Visible = true;
                }
                showSearchReSult(Page, ResultNumber);
                label9.Text = "有 " + ResultNumber + " 項搜尋結果";
                label9.Visible = true;
          //      textBox2.Text += "\r\nPage:" + Page + "\r\nResultNumber:" + ResultNumber;
          //      textBox2.Text += "\r\n" + videoId[0];
          //      textBox2.Text += "\r\n" + videoTitle[0];
            }
            else
            {
                label9.Text = "請輸入查詢詞";
                label9.Visible = true;
            }
            

        }


        public async Task<string> Run()
        {
            // Create the service.
            var youtubeService = new Google.Apis.YouTube.v3.YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBaEizFXx0bkO7AhQaiYzwvG9-hv2STMNo",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = textBox1.Text; // Replace with your search term.
            searchListRequest.MaxResults = 50;
            int resultCount=0;      //結果大小
            int Max_result=40;       //最大獲得數

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            videos = new List<string>();
            channels = new List<string>();

            videoTitle.Clear();
            videoId.Clear();



            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {

                if (resultCount >= Max_result)
                    break;
                else
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            videoTitle.Add(searchResult.Snippet.Title);             //儲存搜尋結果
                            videoId.Add(searchResult.Id.VideoId);
                            resultCount++;
                            break;

                    }
                }
            }
     //       textBox2.Text += String.Format("videoTitle:\r\n{0}\r\n", string.Join("\r\n", videoTitle));
     //       textBox2.Text += "\r\n";
      //      textBox2.Text += String.Format("videoId:\r\n{0}\r\n", string.Join("\r\n", videoId));

            return "111";
        }


        /*
         * 
         * 換頁
         * page:頁數
         * ResultNumber:呈現的資料數
         * 
         * */

        public void showSearchReSult(int page,int ResultNumber){
            int srartPosition = 0;
            if (page > 0)
            {
                ResultIndex = (page - 1) * 6;

                pictureBox1.Visible = false;  label3.Visible = false;   button1.Visible = false;
                pictureBox4.Visible = false; label6.Visible = false; button8.Visible = false;
                pictureBox6.Visible = false; label8.Visible = false; button10.Visible = false;
                pictureBox2.Visible = false; label4.Visible = false; button6.Visible = false;
                pictureBox3.Visible = false; label5.Visible = false; button7.Visible = false;
                pictureBox5.Visible = false; label7.Visible = false; button9.Visible = false;


                srartPosition = (page - 1) * 6;             //計算起始座標
                if (srartPosition < ResultNumber)           //秀出第一張圖
                {
                    pictureBox1.Visible = true; label3.Visible = true; button1.Visible = true;
                    pictureBox1.ImageLocation = "http://img.youtube.com/vi/" + videoId[srartPosition ] + "/1.jpg"; 
                    label3.Text = videoTitle[srartPosition];
                    showViedioTitle(label3, videoTitle[srartPosition]);

                }
                if (srartPosition + 1 < ResultNumber)
                {
                    pictureBox4.Visible = true; label6.Visible = true; button8.Visible = true;
                    pictureBox4.ImageLocation = "http://img.youtube.com/vi/" + videoId[srartPosition+1] + "/1.jpg";
                    label6.Text = videoTitle[srartPosition + 1];
                    showViedioTitle(label6, videoTitle[srartPosition+1]);
                }
                if (srartPosition + 2 < ResultNumber)
                {
                    pictureBox6.Visible = true; label8.Visible = true; button10.Visible = true;
                    pictureBox6.ImageLocation = "http://img.youtube.com/vi/" + videoId[srartPosition + 2] + "/1.jpg";
                    label8.Text = videoTitle[srartPosition + 2];
                    showViedioTitle(label8, videoTitle[srartPosition + 2]);

                }
                if (srartPosition + 3 < ResultNumber)
                {
                    pictureBox2.Visible = true; label4.Visible = true; button6.Visible = true;
                    pictureBox2.ImageLocation = "http://img.youtube.com/vi/" + videoId[srartPosition + 3] + "/1.jpg";
                    label4.Text = videoTitle[srartPosition + 3];
                    showViedioTitle(label4, videoTitle[srartPosition + 3]);
                }
                if (srartPosition + 4 < ResultNumber)
                {
                    pictureBox3.Visible = true; label5.Visible = true; button7.Visible = true;
                    pictureBox3.ImageLocation = "http://img.youtube.com/vi/" + videoId[srartPosition + 4] + "/1.jpg";
                    label5.Text = videoTitle[srartPosition + 4];
                    showViedioTitle(label5, videoTitle[srartPosition + 4]);
                }
                if (srartPosition + 5 < ResultNumber)
                {
                    pictureBox5.Visible = true; label7.Visible = true; button9.Visible = true;
                    pictureBox5.ImageLocation = "http://img.youtube.com/vi/" + videoId[srartPosition + 4] + "/1.jpg";
                    label7.Text = videoTitle[srartPosition + 5];
                    showViedioTitle(label7, videoTitle[srartPosition + 5]);
                }
            }
            else                                             //不顯示groupbox
            {
                groupBox1.Visible = false;
            }


        }

        //顯示影片資訊時自動斷行
        public void showViedioTitle(Label label, String title)
        {
            string temp = title;
            if (temp.Length > 45)
            {
                temp = temp.Insert(30, "\r\n");
                temp = temp.Insert(15, "\r\n");
                label.Text = temp.Substring(0, 27) + ".....";
            }
            else if (temp.Length > 30)
            {
                temp = temp.Insert(30, "\r\n");
                temp = temp.Insert(15, "\r\n");
                label.Text = temp;
            }
            else if (temp.Length > 15)
            {
                temp = temp.Insert(15, "\r\n");
                label.Text = temp;
            }
            else
            {
                label.Text = temp;
            } 

        }

        public void loadListenList(List<string> ListenList)
        {

        }


        private void button11_Click(object sender, EventArgs e)
        {

        }


        //開啟播放介面
        private void button5_Click(object sender, EventArgs e)
        {
          //  播放界面 playInterface = new 播放界面(); 
          //      SearchPage   searchPage =new SearchPage();
          //      searchPage.Show();
          //     searchPage.setForm(this);

            currentPlay = 0;        //從第一首開始播放
            nowPlay = playlistId[currentPlay];


            if (playingTrue==0)
            {

                playInterface = new 播放界面(this); 
                playInterface.setForm(this);
                playInterface.Show();
                

                playInterface.axShockwaveFlash1.Movie = "https://youtube.googleapis.com/v/" + nowPlay + "?autoplay=1&version=3&enablejsapi=1";

                try
                {
                    string x = playInterface.axShockwaveFlash1.CallFunction("<invoke name=\"playVideo\" returntype=\"xml\"></invoke>");
                }
                catch
                {
                }
            
                button5.Text = "關閉";

                playingTrue =1;
               // playingTrue = ++;
            }
            else
            {
                playInterface.Close();
             //   playInterface.Hide();
             //   playInterface.Controls.Clear();

                button5.Text = "播放";
                playingTrue = 0;
            }

            
            //重整撥放清單
            
        }


        //選擇第一項
        private void button1_Click_1(object sender, EventArgs e)
        {
            int temp = ((Page - 1) * 6);
            playlistTitle.Add(videoTitle[temp]);    //新稱歌曲標題
            playlistName.Add(videoTitle[temp]); 
            playlistId.Add(videoId[temp]);          //新增歌曲代碼
            listenPage.showPlaylist();              //重新顯示歌單

            //historyVideoTitle.Add(videoTitle[temp]);
        }
        //選擇第二項
        private void button8_Click(object sender, EventArgs e)
        {
            int temp = ((Page - 1) * 6) + 1;
            playlistTitle.Add(videoTitle[temp]);
            playlistName.Add(videoTitle[temp]); 
            playlistId.Add(videoId[temp]);
            listenPage.showPlaylist();              //重新顯示歌單
        }
        //選擇第三項
        private void button10_Click(object sender, EventArgs e)
        {
            int temp = ((Page - 1) * 6) + 2;
            playlistTitle.Add(videoTitle[temp]);
            playlistName.Add(videoTitle[temp]); 
            playlistId.Add(videoId[temp]);
            listenPage.showPlaylist();              //重新顯示歌單
        }
        //選擇第四項
        private void button6_Click(object sender, EventArgs e)
        {
            int temp = ((Page - 1) * 6) + 3;
            playlistTitle.Add(videoTitle[temp]);
            playlistName.Add(videoTitle[temp]); 
            playlistId.Add(videoId[temp]);
            listenPage.showPlaylist();              //重新顯示歌單
        }
        //選擇第五項
        private void button7_Click(object sender, EventArgs e)
        {
            int temp = ((Page - 1) * 6) + 4;
            playlistTitle.Add(videoTitle[temp]);
            playlistName.Add(videoTitle[temp]); 
            playlistId.Add(videoId[temp]);
            listenPage.showPlaylist();              //重新顯示歌單
        }
        //選擇第六項
        private void button9_Click(object sender, EventArgs e)
        {
            int temp = ((Page - 1) * 6) + 5;
            playlistTitle.Add(videoTitle[temp]);
            playlistName.Add(videoTitle[temp]); 
            playlistId.Add(videoId[temp]);
            listenPage.showPlaylist();              //重新顯示歌單
        }



        //換查詢結果頁
        private void button11_Click_1(object sender, EventArgs e)
        {

            if ((Page  * 6) < ResultNumber)
            {
                Page++;
                ResultNumber = videoId.Count;
                showSearchReSult(Page, ResultNumber);
                button12.Visible = true;
            }
            if ((Page * 6) > ResultNumber)
            {
                button11.Visible = false;
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Page>1)
            {
                Page--;
                ResultNumber = videoId.Count;
                showSearchReSult(Page, ResultNumber);
                button11.Visible = true;
                if (Page == 1)
                {
                    button12.Visible = false;
                }
            }
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            //開啟登入form
            login loginInterface = new login(this);
            
            loginInterface.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click_1(object sender, EventArgs e)//開啟歌單
        {
            //秀出歌單
            listenPage = new listenlist();
            listenPage.setForm(this);
            listenPage.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            listenPage.Location = new System.Drawing.Point(1000, 300);

            listenPage.Show();
        }


        private void button15_Click(object sender, EventArgs e)//讀取資料庫
        {
            string cn = @"Data Source=(LocalDB)\v11.0;" +
                "AttachDbFilename=|DataDirectory|Database1.mdf;" +
                "Integrated Security=True";

            SqlConnection db = new SqlConnection(cn);//建立連接物件
            
            db.Open();   //使用Open方法開啟和資料庫的連接
            SqlCommand cmd = new SqlCommand("SELECT 個人歌單,歌單ID FROM 紀錄 WHERE (帳號='me') AND (歷史紀錄 IS NULL)", db);
            SqlDataReader dr = cmd.ExecuteReader();
            playlistTitle.Clear();
            playlistId.Clear();
            while (dr.Read())
            {
                playlistTitle.Add(dr[0].ToString());
                playlistId.Add(dr[1].ToString());
            }
            db.Close();   //使用Close方法關閉和資料庫的連接
        }

        private void button4_Click(object sender, EventArgs e)//History
        {
            string cn = @"Data Source=(LocalDB)\v11.0;" +
                "AttachDbFilename=|DataDirectory|Database1.mdf;" +
                "Integrated Security=True";

            SqlConnection db = new SqlConnection(cn);//建立連接物件

            db.Open();   //使用Open方法開啟和資料庫的連接
            SqlCommand cmd = new SqlCommand("SELECT 歷史紀錄,紀錄ID FROM 紀錄 WHERE (帳號='me') AND (個人歌單 IS NULL)", db);
            SqlDataReader dr = cmd.ExecuteReader();
            
            string data = "";   //宣告字串存放資料內容
            for (int i = 0; i < dr.FieldCount; i++)
            {
                data += dr.GetName(i) + "\t";
            }
            data += "\n";   //換行
            int j = 0;
            while (dr.Read())
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    if (i == 0)
                        data += historylistName[j++] + "\t";
                    else
                        data += dr[i].ToString() + "\t";
                }
                data += "\n";
            }
            MessageBox.Show(data, "紀錄");

            db.Close();   //使用Close方法關閉和資料庫的連接
        }

        

    }
}
