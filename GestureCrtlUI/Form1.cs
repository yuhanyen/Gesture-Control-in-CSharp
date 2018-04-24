using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GestureCrtlUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string dbHost = "192.168.50.225";
            string dbUser = "david";
            string dbPass = "hellohello";
            string dbName = "kinect_225";
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName + ";charset=utf8;";
            string ResultStr;

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        ResultStr = "ErrCode = 0";
                        break; //"連線到資料庫."
                    case 1045:
                        ResultStr = "ErrCode = 1";
                        break; //"使用者帳號或密碼錯誤,請再試一次.";
                }
            }

            string SQL = "SELECT * FROM kinect_1_img  ORDER BY id DESC LIMIT 1";
            try
            {
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader myData = cmd.ExecuteReader();

                if (!myData.HasRows)
                    ResultStr = "null"; // 如果沒有資料,顯示沒有資料的訊息
                else
                {
                    ResultStr = myData.Read().ToString(); // 讀取資料並且顯示出來

                    DateTime dt1 = Convert.ToDateTime(myData.GetString("time"));
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;

                    if (ts.TotalSeconds < 5000)
                    {
                        string url = "http://192.168.50.225/Kinet_Face_Img/Kinect_1/" + myData.GetString("img_name");
                        pictureBox1.ImageLocation = url;
                        if (Convert.ToInt32(myData.GetString("face_number")) > 0)
                        {
                            pictureBox2.Image = Properties.Resources.O;
                        }
                        else
                        {
                            pictureBox2.Image = Properties.Resources.X;
                        }
                    }
                }
                myData.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                ResultStr = "ErrCode = 2 :" + ex.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
