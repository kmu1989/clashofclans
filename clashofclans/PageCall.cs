using System;
using System.IO;
using System.Net;
using System.Text;

namespace clashofclans
{
    public class PageCall
    {
        public void send(string subject, string content, string table)
        {                    
            String postData = String.Format("subject={0}&content={1}&table={2}", subject, content, table);

            string url = "http://cocfriend.com/bbs/write_update_video.php";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            request.Timeout = 30 * 1000;

            //POST할 데이타를 Request Stream에 쓴다
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length; // 바이트수 지정

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bytes, 0, bytes.Length);
            }

            // Response 처리
            
            string responseText = string.Empty;
            using (WebResponse resp = request.GetResponse())
            {
                Stream respStream = resp.GetResponseStream();
                using (StreamReader sr = new StreamReader(respStream))
                {
                    responseText = sr.ReadToEnd();
                }               
            }
            
        }
    }
}
