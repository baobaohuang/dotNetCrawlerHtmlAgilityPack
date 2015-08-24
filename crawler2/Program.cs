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
            else if (str=="50")
            {
                getTaiwan50();
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

        static void getTaiwan50()
        {
            // 下載 Yahoo 奇摩股市資料 (範例為 2317 鴻海) 
            WebClient client = new WebClient();
            MemoryStream ms = new MemoryStream(client.DownloadData(
        "https://tw.stock.yahoo.com/s/list2.php?c=%A5x%C6W50"));

            // 使用預設編碼讀入 HTML 
            HtmlDocument doc = new HtmlDocument();
            doc.Load(ms, Encoding.Default);

            // 裝載第一層查詢結果 
            HtmlDocument docStockContext = new HtmlDocument();

            docStockContext.LoadHtml(doc.DocumentNode.SelectSingleNode(
"/html[1]/body[1]/center[1]/table[2]/tr[2]/td[1]/table[1]/tr[1]/td[1]/table[1]").InnerHtml);

            // 取得個股標頭 
            HtmlNodeCollection nodeHeaders =
         docStockContext.DocumentNode.SelectNodes("./tr[1]/th");

            // 取得個股數值
            //stock[] stocks = new stock[50];
            List<stock> stocks = new List<stock>();
            int i = 0;
            for (int stock = 2; stock < 52; stock++)
            {
                string nodeNumber = "./tr[" + stock + "]";
           
                string[] values = docStockContext.DocumentNode.SelectSingleNode(nodeNumber).InnerText.Trim().Split('\n');
                
                // 輸出資料
                decimal a =  Convert.ToDecimal(values[10].Trim());
                decimal b =  Convert.ToDecimal(values[11].Trim());
                decimal c =  Convert.ToDecimal(values[10].Trim()) - Convert.ToDecimal(values[11].Trim());
                stock x = new stock { Name = values[0].Trim(), HighPrice = a, LowPrice = b, PriceDrop = c, Percent = decimal.Round(c * 100 / a, 1) };
                stocks.Add ( x);                
            }

            
            //Array.Sort(stocks, delegate(stock x, stock y) { return x.Percent.CompareTo(y.Percent); });
            IEnumerable<stock> query = stocks.OrderByDescending( x =>x.Percent);
            ////IEnumerable<stock> query = from e in stocks orderby e.Percent descending select e;
            ////var query = from e in stocks orderby e.Percent select e;
            ////var query = stocks.OrderBy(x => x.Percent);
            foreach (stock output in query)
            {
                Console.Write(output.Name);
                Console.Write(output.PriceDrop);
                Console.Write(" " + output.Percent);
                Console.Write("\n");
            }

            doc = null;
            docStockContext = null;
            client = null;
            ms.Close();   
        }
    }
    
    class stock
    {
        public string Name { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal PriceDrop { get; set; }
        public decimal Percent { get; set; }
    }
}
