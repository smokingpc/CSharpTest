using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;

namespace PrintTest
{
    //[列印的基本觀念解說]
    //把印表機當繪圖裝置，紙張當成螢幕。如果要在紙張上面畫圈畫方塊劃線，
    //印法就跟控制繪圖元件在螢幕上畫圖是一樣的
    //但要最後印到紙上，就要把弄好的影像資料(image or memory stream) 送去給印表機裝置印。

    //這份範例主要的概念是 "不用data wizard綁死的資料庫連線，可以自由使用DataTable或List"
    //但report本身版面還是要借助wizard，這東西是個很大的XML定義檔，自己寫太過繁瑣
    //做法大致上步驟是：
    //1.利用 report wizard 弄個local report，產生過程中也會叫你用DataWizard產生
    //  dataset與datasource，就給他用wizard做下去沒關係(後面不使用就行)，
    //  注意這邊選擇的column名稱不要亂寫，要寫正確的....
    //2.列印時產生個 LocalReport 物件，在report.DataSources.Add()指定 ReportDataSource 物件
    //  給它，指定資料來源，這邊資料來源可以是DataTable也可以是IEnumerable等，共五六種container可選
    //  千萬要注意，指定ReportDataSource時，source name這邊指定的名字 "一定" 要跟step1
    //  用DataWizard產生的data source name一樣，這樣才能騙過LocalReport.....
    public partial class frmMain
    {
        //每一頁都有自己的binary stream....
        private List<Stream> PageStreams = new List<Stream>();
        private int CurrentPage = -1;

        private void PrintReportDirectly(string printer)
        {
            PrintDocument doc = PrintDocument();

            //印表機是根據 Installed Name 來指定的....
            //(控制台->裝置與印表機  清單內看到的名稱)
            doc.PrinterSettings = new PrinterSettings()
            {
                PrinterName = printer,
            };

            //真正送列印命令並且把列印資料送出
            doc.Print();
        }

        private void PrintWithPreview(string printer)
        {
            PrintDocument doc = PrintDocument();
            //print preview control只要把畫好的document塞給它就會自己顯示，不需refresh
            printPreviewControl1.Document = doc;

            //印表機是根據 Installed Name 來指定的....
            //(控制台->裝置與印表機  清單內看到的名稱)
            doc.PrinterSettings = new PrinterSettings()
            {
                PrinterName = printer,
            };

            //真正送列印命令並且把列印資料送出
            doc.Print();
        }
        private void PreivewReport()
        {
            //其實ReportViewer只是先幫你把 LocalReport 與 RemoteReport 物件建好，
            //這LocalReport物件還是跟原本一樣操作方法

            LocalReport report = reportViewer1.LocalReport;
            //如果要用datatable也可以，欄位大小寫也要注意......
            //DataTable table = new DataTable();
            BindingList<CMyData> datalist = new BindingList<CMyData>();
            datalist.Add(
                        new CMyData()
                        {
                            SN = 1,
                            USERNAME = "User1",
                            POSTCODE = 456,
                            TEL = "02-28825252",
                        });
            datalist.Add(
                        new CMyData()
                        {
                            SN = 2,
                            USERNAME = "User2",
                            POSTCODE = 123,
                            TEL = "0800-092-000",
                        });
            //EmbeddedResource 的 report file 要寫成 <%NameSpace>.<%RDLC Filename>
            report.ReportEmbeddedResource = "PrintTest.MyReportXYZ.rdlc";

            //在rdlc裡面指定的列印dataset，這邊的名稱要跟rdlc裡面的dataset指定
            //的名稱一樣不然會exception
            report.DataSources.Add(
                new ReportDataSource("DataSetABC", datalist));
            
            //資料塞好以後叫ReportViewer做一次 refresh report 就會自己畫出來了
            reportViewer1.RefreshReport();
        }
        private PrintDocument PrintDocument()
        {
            //列印基本上就是做出Image，並將它吐到printer就可以列印了
            //Report裡面要指定個Group的欄位(通常用PK當group條件)，不然只會印一行
            //注意：這邊比較麻煩的地方是DataSource內指定的欄位有大小寫的區分
            //     例如下面這範例把 CMyData::USERNAME 改成CMyData::UserName 就會印不出這個欄位(其他欄位仍然正常印出)
            //     如果用DataTable，也有一樣的限制....

            //如果要用datatable也可以，欄位大小寫也要注意......
            //DataTable table = new DataTable();
            BindingList<CMyData> datalist = new BindingList<CMyData>();
            datalist.Add(
                        new CMyData()
                        {
                            SN=1,
                            USERNAME="User1",
                            POSTCODE=456,
                            TEL="02-28825252",
                        });
            datalist.Add(
                        new CMyData()
                        {
                            SN = 2,
                            USERNAME = "User2",
                            POSTCODE = 123,
                            TEL = "0800-092-000",
                        });

            LocalReport report = new LocalReport();
            //EmbeddedResource 的 report file 要寫成 <%NameSpace>.<%RDLC Filename>
            report.ReportEmbeddedResource = "PrintTest.MyReportXYZ.rdlc";

            //在rdlc裡面指定的列印dataset，這邊的名稱要跟rdlc裡面的dataset指定
            //的名稱一樣不然會exception
            report.DataSources.Add(
                new ReportDataSource("DataSetABC", datalist));
            //指定
            string devinfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>21cm</PageWidth>
                <PageHeight>29.7cm</PageHeight>
                <MarginTop>1cm</MarginTop>
                <MarginLeft>1cm</MarginLeft>
                <MarginRight>1cm</MarginRight>
                <MarginBottom>1cm</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            report.Render("Image", devinfo, BuildStream, out warnings);
            //report.ListRenderingExtensions

            //LocalReport在Render時會動到Stream的 CurrentPosition，所以要先歸位
            //等一下讀取才不會讀錯位置
            foreach (var item in PageStreams)
                item.Position = 0;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            CurrentPage = 0;
            
            return printDoc;
        }
        //每印一頁就會觸發一次？
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(PageStreams[CurrentPage]);

            //調整列印範圍
            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            //把剛剛render好的Image印上去printer
            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            CurrentPage++;
            ev.HasMorePages = (CurrentPage < PageStreams.Count);
        }

        //Report.Render時用的，每多一頁就會呼叫一次
        private Stream BuildStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            PageStreams.Add(stream);

            //這邊new MemoryStream()後回傳給LocalReport，讓它可以把影像畫在這個stream裡
            return stream;
        }
    }

    public class CMyData
    {
        public Int32 SN { get; set; }
        public string USERNAME { get; set; }
        public string TEL { get; set; }
        public Int16 POSTCODE { get; set; }
    }
}
