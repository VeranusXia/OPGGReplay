using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace OPGGReplay
{
    public partial class Form1 : Form
    {
        private API api = new API();
        private Matches matches = new Matches();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            api.apiKey = textBox2.Text;
            matches = api.GetMatchListBySummonerName(name);
            dataGridView1.DataSource = matches.matches.Select(u => new
            {
                服务器 = u.platformId,
                英 = GetImageFromWeb(u.champion.ToString()),
                比赛 = u.matchId,
                分路 = u.lane,
                位置 = u.role
            }).ToList();
            DataGridViewImageColumn column = (DataGridViewImageColumn)dataGridView1.Columns[1];
            column.ImageLayout = DataGridViewImageCellLayout.Stretch;
            column.Width = 25;

        }
        public Image GetImageFromWeb(string cid)
        {
            string url = string.Format("http://img.db.178.com/lol/images/content/champion/icons/{0}.jpg", cid);
            string filepath = System.Environment.CurrentDirectory + "\\img\\" + cid + ".jpg";
            if (File.Exists(filepath))
            {
                return GetImage(filepath);
            }
            else
            {
                if (!Directory.Exists(System.Environment.CurrentDirectory + "\\img\\"))
                {
                    Directory.CreateDirectory(System.Environment.CurrentDirectory + "\\img\\");
                }
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(url, filepath);
                    return GetImage(filepath);
                }
            }

        }
        public System.Drawing.Image GetImage(string path)
        {
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);

            fs.Close();

            return result;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string matchid = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            api.DownloadBat(matchid);

            Process.Start(System.Environment.CurrentDirectory + "\\Replay\\" + matchid + ".bat");
        }

    }
}
