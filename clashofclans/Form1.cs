using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace clashofclans
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        async void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCcu-5qYc71UDjpY0JxOh0fGpfXdenZsw4",
                ApplicationName = "My YouTube Search"

            });

            SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
            listRequest.Q = "클래시 오브 클랜";
            listRequest.MaxResults = 50;
            
            SearchListResponse searchResponse = listRequest.Execute();
                        
            int count = 0;
            //dataGridView1.Rows.Add(49);
            foreach (SearchResult searchResult in searchResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        //videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[count].Cells[0].Value = searchResult.Snippet.Title;
                        dataGridView1.Rows[count].Cells[1].Value = searchResult.Id.VideoId;
                        
                        count++;
                        break;
                }
            }
        }
        
        Thread ListThread;
        private void button1_Click(object sender, EventArgs e)
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
                pc.send(subject, content);
                Thread.Sleep(1000);
                dataGridView1.Rows[i].Cells[2].Value = "업로드 완료";
            }
            ListThread.Abort();
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
    }
}
