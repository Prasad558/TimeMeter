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
    public partial class Form1 : Form
    {
        public Form1()
        {
            //textBox1.AutoSize = false;
            //textBox2.AutoSize = false;
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=103.212.121.67;Initial Catalog=ctasks;user id=ctaskadmin;password=P@ssword12*;Integrated Security=false");
        OpenFileDialog open = new OpenFileDialog();

       

        private void Login_Click(object sender, EventArgs e)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM TBL_TEAM_MEMBERS where TEAM_MEMBER_EMAIL_ADDRESS='"+ textBox1.Text + "' and TEAM_MEMBER_PASSWORD='"+textBox2.Text+"'", con))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        LoginInfo.userName = row["TEAM_MEMBER_FIRST_NAME"].ToString()+" "+ row["TEAM_MEMBER_SUR_NAME"].ToString();
                        LoginInfo.userID = row["TEAM_MEMBER_EMAIL_ADDRESS"].ToString();
                        LoginInfo.companyID = Convert.ToInt32(row["CUS_COMPANY_ID"].ToString() ?? "0");
                        LoginInfo.role = row["TEAM_MEMBER_ROLE"].ToString();
                    }

                    this.Cursor = Cursors.WaitCursor;
                    // Do something here  
                    this.Cursor = Cursors.Default;                    
                    User userForm = new User();
                    userForm.Show();
                    Hide();
                }
                else
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    MessageBox.Show("Invalid username or password!");
                }
            } 
        }
    }
}
