using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace crawler2
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 2300; i <= 2317; i++)
            {
                getDate.getData(Convert.ToString(i));
                System.Threading.Thread.Sleep(5000);
            }
            Console.WriteLine("Completed.");              
        }
    }
    
    public class getDate
    {
        public static void getData(string args)
        {

            // 下載 Yahoo 奇摩股市資料 (範圍代號只取股價 ) 
            WebClient client = new WebClient();
            MemoryStream ms = new MemoryStream(client.DownloadData(
        "http://tw.stock.yahoo.com/q/q?s=" + args));

            // 使用預設編碼讀入 HTML 
            HtmlDocument doc = new HtmlDocument();
            doc.Load(ms, Encoding.Default);

            // 裝載第一層查詢結果 
            HtmlDocument docStockContext = new HtmlDocument();

            try
            {
                string sDocStockContext = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1]").InnerHtml;

                docStockContext.LoadHtml(sDocStockContext);

                // 取得個股標頭 
                HtmlNodeCollection nodeHeaders =
             docStockContext.DocumentNode.SelectNodes("./tr[1]/th");
                // 取得個股數值
                HtmlNode nodeName =
            docStockContext.DocumentNode.SelectNodes("./tr[2]/td/a").ElementAt(0);
                string[] values = docStockContext.DocumentNode.SelectSingleNode(
            "./tr[2]").InnerText.Trim().Split('\n');
                //    int i = 0;
                //    //輸出資料 
                //    foreach (HtmlNode nodeHeader in nodeHeaders)
                //    {
                //        Console.WriteLine("Header: {0}, Value: {1}",
                //nodeHeader.InnerText, values[i].Trim());
                //        i++;
                //    }

                string Stock = nodeHeaders.ElementAt(0).InnerText + " ";
                Stock = Stock + nodeName.InnerText + " ";
                //Stock = Stock + nodeValue.ElementAt(0).SelectNodes("./a[1]").ElementAt(0).InnerText + " ";
                Stock = Stock + nodeHeaders.ElementAt(7).InnerText + " ";
                Stock = Stock + values[7].Trim();
                //Stock = Stock + nodeValue.ElementAt(7).InnerText ; 

                Console.WriteLine(Stock);
                doc = null;
                docStockContext = null;
                client = null;
                ms.Close();

            }
            catch { }

        }

    }    
}
