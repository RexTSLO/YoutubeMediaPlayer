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
    public partial class login : Form
    {
        //帳號:me
        //密碼:1234
        
        mainform Mainform;

        public login(mainform m)
        {
            InitializeComponent();
            Mainform = m;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "me" && textBox2.Text == "1234")
            {
                MessageBox.Show("登入成功");
                Mainform.acc = "me";
                Mainform.psw = "1234";
                Mainform.label10.Text = "me";
                Mainform.label10.Visible = true;
                Close();
            }
        }

    }
}
