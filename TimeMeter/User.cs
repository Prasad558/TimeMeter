using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                string folderPath = Path.GetDirectoryName("http://102.130.114.194:9023/Screenshots");
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
                    folderPath = folderPath + "\\Screenshots\\";
                    //string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    //string tt = Path.GetDirectoryName(Application.StartupPath).Replace(@"bin\debug\", string.Empty);

                    //string gg = tt.Replace(@"bin\\debug\\", "Screenshots");
                    //path = path.Remove(path.Length - 10, 10) + "Images\\";

                    //bitmap.Save(path + filename, ImageFormat.Jpeg); //Change Content to any folder name you desire



                    bitmap.Save(saveDirectory + filename, ImageFormat.Jpeg);

                    StringBuilder sb = new StringBuilder();

                    //NameValueCollection nvc = new NameValueCollection();
                    //nvc.Add("id", "TTR");
                    //nvc.Add("btn-submit-photo", "Upload");
                    //HttpUploadFile("http://102.130.114.194:9023/Screenshots/",
                    //     @"C:\TimeMeter\"+ filename, "file", "image/jpeg", nvc);
                    //UploadFileToFTP(filename);
                    // Get the hostname
                    //string myHost = System.Net.Dns.GetHostName();

                    string HostName = System.Net.Dns.GetHostName();
                    string IP_ADDRESS = "";
                    IPAddress[] ipaddress = Dns.GetHostAddresses(HostName);
                    foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        IP_ADDRESS = ip4.ToString();
                    }

    //                DateTime dt = DateTime.Now;
    //                string todayDate = dt.ToString("yyyy-MM-dd HH:mm:ss");
    //                string query = string.Format("insert into TBL_SCREENSHOTS_LOG" + "(COMPANY_ID,USER_ID,IMAGE_NAME,IP_ADDRESS,DEVICE_NAME,SCREENSHOT_DATE)"
    //+ "values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", LoginInfo.companyID, LoginInfo.userID, filename, IP_ADDRESS, HostName, todayDate);
    //                bool flagResult = false;
    //                SqlCommand com = new SqlCommand(query, con);
    //                try
    //                {
    //                    con.Open();
    //                    if (com.ExecuteNonQuery() == 1)
    //                        flagResult = true;
    //                    //MessageBox.Show("Screenshot captured successfully!");
    //                    con.Close();
    //                }
    //                catch (Exception es)
    //                {
    //                    MessageBox.Show(es.Message);
    //                }
    //                con.Close();
                }
            }            
        }

        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            //log.Debug(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoginInfo.userID = "";
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
        private void UploadFileToFTP(string fileName)
        {

            try
            {
                FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create("ftp://www.ctasks.co.za/"+ fileName);

                ftpReq.UseBinary = true;
                //ftpReq.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                
                ftpReq.Credentials = new NetworkCredential("jqzdtaca", "P@ssword12*");
                ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
                //jqzdtaca@103.212.121.67
                //P@ssword12*
                byte[] b = File.ReadAllBytes(@"C:\TimeMeter\" + fileName);
                ftpReq.ContentLength = b.Length;
                using (Stream s = ftpReq.GetRequestStream())
                {
                    s.Write(b, 0, b.Length);
                }

                FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();

                if (ftpResp != null)
                {
                    if (ftpResp.StatusDescription.StartsWith("226"))
                    {
                        Console.WriteLine("File Uploaded.");
                    }
                }
            }
            catch (WebException e)
            {
                String status = ((FtpWebResponse)e.Response).StatusDescription;
            }

            
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
