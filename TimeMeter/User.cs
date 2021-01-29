using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeMeter
{
    public partial class User : Form
    {
        public User()
        {            
            InitializeComponent();
        }
        int i = 0;
        SqlConnection con = new SqlConnection(@"Data Source=103.212.121.67;Initial Catalog=ctasks;user id=ctaskadmin;password=P@ssword12*;Integrated Security=false");

    
        public void OnTimerEvent(object source, EventArgs e)
        {
            if(!string.IsNullOrEmpty(LoginInfo.userID))
            {
                string saveDirectory = @"C:\TimeMeter\";
                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }

                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }
                    string filename = DateTime.Now.ToString("dd-MM-yyyy");
                    filename = filename + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".jpg";

                    //string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    //string tt = Path.GetDirectoryName(Application.StartupPath).Replace(@"bin\debug\", string.Empty);

                    //string gg = tt.Replace(@"bin\\debug\\", "Screenshots");
                    //path = path.Remove(path.Length - 10, 10) + "Images\\";

                    //bitmap.Save(path + filename, ImageFormat.Jpeg); //Change Content to any folder name you desire
                    bitmap.Save(saveDirectory + filename, ImageFormat.Jpeg);

                    StringBuilder sb = new StringBuilder();
                    // Get the hostname
                    //string myHost = System.Net.Dns.GetHostName();

                    string HostName = System.Net.Dns.GetHostName();
                    string IP_ADDRESS = "";
                    IPAddress[] ipaddress = Dns.GetHostAddresses(HostName);
                    foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        IP_ADDRESS = ip4.ToString();
                    }

                    DateTime dt = DateTime.Now;
                    string todayDate = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    string query = string.Format("insert into TBL_SCREENSHOTS_LOG" + "(COMPANY_ID,USER_ID,IMAGE_NAME,IP_ADDRESS,DEVICE_NAME,SCREENSHOT_DATE)"
    + "values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", LoginInfo.companyID, LoginInfo.userID, filename, IP_ADDRESS, HostName, todayDate);
                    bool flagResult = false;
                    SqlCommand com = new SqlCommand(query, con);
                    try
                    {
                        con.Open();
                        if (com.ExecuteNonQuery() == 1)
                            flagResult = true;
                        //MessageBox.Show("Screenshot captured successfully!");
                        con.Close();
                    }
                    catch (Exception es)
                    {
                        MessageBox.Show(es.Message);
                    }
                    con.Close();
                }
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            Form1 us = new Form1();
            us.Show();
            Hide();
            
        }

        private void User_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome  " + LoginInfo.userName;

            timer1.Interval = 60000;
            timer1.Enabled = true;
            timer1.Tick += new System.EventHandler(OnTimerEvent);

            timer2.Interval = 1000;
            timer2.Start();
        }
        

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            i = i + 1;
            TimeSpan time = TimeSpan.FromSeconds(i);
            string str = time.ToString(@"hh\:mm\:ss");
            label2.Text = DateTime.Now.ToString(str);

        }
    }
}
