using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Youtube_Media_Player
{
    public partial class listenlist : Form
    {

        mainform Mainform;
        public listenlist()
        {
            InitializeComponent();

        }

        private void listenlist_Load(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            //印出歌單
            showPlaylist();
            timer1.Enabled = true;
        }
        int count=0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            textBox1.Text = "";

            showPlaylist();
            count++;
             * */
            //textBox1.Text += "\r\n" + count;

        }


        public void showPlaylist()
        {
            /*textBox1.Text = "";
            for (int i = Mainform.currentPlay; i < Mainform.playlistTitle.Count; i++)
            {
                if (Mainform.playlistTitle[i].Length>30)
                    textBox1.Text += Mainform.playlistTitle[i].Substring(0,30) + "....\r\n";
                else 
                    textBox1.Text += Mainform.playlistTitle[i] + "\r\n";
            }*/

            textBox1.Text = "";
            for (int i = Mainform.currentPlay; i < Mainform.playlistTitle.Count; i++)
            {
                if (Mainform.playlistName[i].Length > 30)
                    textBox1.Text += Mainform.playlistName[i].Substring(0, 30) + "....\r\n";
                else
                    textBox1.Text += Mainform.playlistName[i] + "\r\n";
            }

        }


        public void setForm(mainform f)
        {
            Mainform = f;

        }
    }
}
