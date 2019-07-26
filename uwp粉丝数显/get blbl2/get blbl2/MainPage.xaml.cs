using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace get_blbl2
{
    public sealed partial class MainPage : Page
    {

        /// <summary>
        /// 在这里填入uid
        /// </summary>
        string uid = "14010836";  







        string sj = DateTime.Now.ToString();//时间
        int datafs = 0;
        int databf = 0;
        int datazl = 0;
        string dataup = "";
        int dataupls = 0;


        //刷新
        DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
        //数据对比
        DispatcherTimer timer2 = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
        //时间更新
        DispatcherTimer timer3 = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 500) };



        //刷新
        private async void Timer_Tick(object sender, object e)
        {
            Dataup();
            txtfs.Text = datafs.ToString();
            txtbf.Text = databf.ToString();
            txtzl.Text = datazl.ToString();
        }


        //数据分析
        private async void Timer2_Tick(object sender, object e)
        {
            int up = datafs - dataupls;


            if (up > 0)
            {

                dataup = "新增粉丝 继续加油啊！！！！";


            }
            else if (up < 0)
            {
                dataup = "都掉粉了还不去更新视频！！！";
            }

            dataupls = datafs;
            txtup.Text = dataup;

        }

        //时间更新
        private async void Timer3_Tick(object sender, object e)
        {
            sj = DateTime.Now.ToString();
            timsjk.Text = sj;
        }



        public MainPage()
        {
            this.InitializeComponent();
            //计时器绘制
            //10秒刷新数据
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新指针
            timer.Start();
            //1分钟分析数据
            timer2 = new DispatcherTimer();
            timer2.Interval = new TimeSpan(0, 1, 0);
            timer2.Tick += Timer2_Tick;//每秒触发这个事件，以刷新指针
            timer2.Start();
            //时间更新
            timer3 = new DispatcherTimer();
            timer3.Interval = new TimeSpan(0, 0, 1);
            timer3.Tick += Timer3_Tick;//每秒触发这个事件，以刷新指针
            timer3.Start();

        }











        private async void Dataup()
        {


            //get请求
            HttpClient client = new HttpClient();
            Uri uri = new Uri("https://api.bilibili.com/x/space/upstat?mid=" + uid);
            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string str = await response.Content.ReadAsStringAsync();
            //提取关键字
            int s = str.IndexOf("view");
            str = str.Substring(s + 6);

            string zhuanlan = "0";
            string buofangliang = "0";
            s = str.IndexOf("view");
            zhuanlan = str.Substring(s + 6);
            buofangliang = str.Replace(zhuanlan, "");


            //筛选数字
            zhuanlan = System.Text.RegularExpressions.Regex.Replace(zhuanlan, @"[^0-9]+", "");
            buofangliang = System.Text.RegularExpressions.Regex.Replace(buofangliang, @"[^0-9]+", "");
            //转换int
            int zl = Convert.ToInt32(zhuanlan);
            int bf = Convert.ToInt32(buofangliang);

            str = "";

            //get请求
            HttpClient client2 = new HttpClient();
            Uri uri2 = new Uri("http://api.bilibili.com/x/relation/stat?vmid=" + uid);
            HttpResponseMessage response2 = await client.GetAsync(uri2);
            response2.EnsureSuccessStatusCode();
            str = await response2.Content.ReadAsStringAsync();
            //提取关键字
            int fs = str.IndexOf("follower");
            str = str.Substring(fs + 10);
            //筛选数字
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");
            //转换int
            fs = Convert.ToInt32(str);

            datafs = fs;
            databf = bf;
            datazl = zl;




        }

        //加载事件

        

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            Dataup();
            dataupls = datafs;
        }
    }
}
