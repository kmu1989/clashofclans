using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;


namespace clashofclans
{
    public partial class Form1 : Form
    {        

        public Form1()
        {
            InitializeComponent();
            dataGridView2.Rows.Add("클래시오브클랜", "video");
            dataGridView2.Rows.Add("클래시오브클랜 완파", "total_damage");
            dataGridView2.Rows.Add("클래시오브클랜 클랜전", "clan_video");
            dataGridView2.Rows.Add("클래시오브클랜 11홀", "11_hall_board");
            dataGridView2.Rows.Add("클래시오브클랜 10홀", "10_hall_board");
            dataGridView2.Rows.Add("클래시오브클랜 9홀", "9_hall_board");
            dataGridView2.Rows.Add("클래시오브클랜 8홀", "8_hall_board");
        }

        string NextPageToken = null;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            youtubeSerche();
        }
             
        private void button1_Click(object sender, EventArgs e)
        {
            youtubeRegister();
        }
        
        //데이터 그리드 셀번호 매기기
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            txtSearch.Text = dataGridView2.Rows[0].Cells[0].Value.ToString();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {            
            txtSearch.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            lb_table.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        //다음
        private void button2_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();

            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCcu-5qYc71UDjpY0JxOh0fGpfXdenZsw4",
                ApplicationName = "My YouTube Search"

            });

            SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
            
            listRequest.Q = txtSearch.Text;
            listRequest.MaxResults = 50;
            listRequest.PageToken = NextPageToken;
            SearchListResponse searchResponse = listRequest.Execute();
            NextPageToken = searchResponse.NextPageToken;
            int count = 0;
            
            //dataGridView1.Rows.Add(49);
            foreach (SearchResult searchResult in searchResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        //videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        //dataGridView1.Rows.Add();
                        //dataGridView1.Rows[count].Cells[0].Value = searchResult.Snippet.Title;
                        //dataGridView1.Rows[count].Cells[1].Value = searchResult.Id.VideoId;
                        dataGridView1.Rows.Add(searchResult.Snippet.Title, searchResult.Id.VideoId);
                        count++;
                        break;
                }
            }
        }
               
        private void btn_auto_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        
        public void youtubeSerche()
        {
            dataGridView1.Rows.Clear();

            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCcu-5qYc71UDjpY0JxOh0fGpfXdenZsw4",
                ApplicationName = "My YouTube Search"

            });

            SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
            listRequest.Q = txtSearch.Text;
            listRequest.MaxResults = 50;

            SearchListResponse searchResponse = listRequest.Execute();
            NextPageToken = searchResponse.NextPageToken;
            int count = 0;
            foreach (SearchResult searchResult in searchResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        //videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        //dataGridView1.Rows.Add();
                        //dataGridView1.Rows[count].Cells[0].Value = searchResult.Snippet.Title;
                        //dataGridView1.Rows[count].Cells[1].Value = searchResult.Id.VideoId;

                        dataGridView1.Rows.Add(searchResult.Snippet.Title, searchResult.Id.VideoId, "");

                        count++;
                        break;
                }
            }
        }

        Thread ListThread;
        public void youtubeRegister()
        {            
            CheckForIllegalCrossThreadCalls = false;
            ListThread = new Thread(new ThreadStart(listsend));
            ListThread.IsBackground = true; //프로그램 종료시 쓰레드 종료
            ListThread.Start();
        }

        PageCall pc = new PageCall();
        public void listsend()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string subject = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string content = dataGridView1.Rows[i].Cells[1].Value.ToString();

                content = "{동영상: http://youtube.com/watch?v=" + content + "}";
                pc.send(subject, content, lb_table.Text);
                Thread.Sleep(1000);
                dataGridView1.Rows[i].Cells[2].Value = "업로드 완료";

            }
            ListThread.Abort();
        }

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                Delay(3000);
                txtSearch.Text = dataGridView2.Rows[i].Cells[0].Value.ToString();
                lb_table.Text = dataGridView2.Rows[i].Cells[1].Value.ToString();

                youtubeSerche();
                Delay(3000);

                youtubeRegister();
                Delay(50000);

                dataGridView2.Rows[i].Cells[2].Value = "업로드 완료";
            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                dataGridView2.Rows[i].Cells[2].Value = "";

        }
    }
}
