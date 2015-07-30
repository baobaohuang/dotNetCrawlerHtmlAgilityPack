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
            string str;
            str = Console.ReadLine();

            if (string.IsNullOrEmpty(str))
            {
                List<string> StockID = new List<string>();
                StockID.Add("2498");
                StockID.Add("9904");
                StockID.ForEach(getdata);                
            }
            else
            {
                getdata(str);
            }

            Console.WriteLine("Completed.");
            Console.ReadLine();

        }

        static void getdata(string stock)
        {
            // 下載 Yahoo 奇摩股市資料 (範例為 2317 鴻海) 
            WebClient client = new WebClient();
            MemoryStream ms = new MemoryStream(client.DownloadData(
        "http://tw.stock.yahoo.com/q/q?s="+stock));

            // 使用預設編碼讀入 HTML 
            HtmlDocument doc = new HtmlDocument();
            doc.Load(ms, Encoding.Default);

            // 裝載第一層查詢結果 
            HtmlDocument docStockContext = new HtmlDocument();

            docStockContext.LoadHtml(doc.DocumentNode.SelectSingleNode(
        "/html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1]").InnerHtml);

            // 取得個股標頭 
            HtmlNodeCollection nodeHeaders =
         docStockContext.DocumentNode.SelectNodes("./tr[1]/th");
            // 取得個股數值 
            string[] values = docStockContext.DocumentNode.SelectSingleNode(
        "./tr[2]").InnerText.Trim().Split('\n');
            int i = 0;

            // 輸出資料 
            foreach (HtmlNode nodeHeader in nodeHeaders)
            {
                Console.WriteLine("Header: {0}, Value: {1}",
        nodeHeader.InnerText, values[i].Trim());
                i++;
            }

            doc = null;
            docStockContext = null;
            client = null;
            ms.Close();            

        }
    }
}
