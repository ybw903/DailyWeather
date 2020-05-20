using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPWeather;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static UWPWeather.OpenWeatherMapProxy;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace daily_weather
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string min_temp;
        string max_temp;
        int lat = 68, lon = 106;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void CityCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string city=e.AddedItems[0].ToString();
            switch (city)
            {
                case "서울":
                    lat = 60; lon = 127;
                    break;
                case "광주":
                    lat = 58; lon = 74;
                    break;
                case "대전":
                    lat = 67; lon = 100;
                    break;
                case "대구":
                    lat = 89; lon = 90;
                    break;
                case "청주":
                    lat = 68; lon = 106;
                    break;
                case "부산":
                    lat = 98; lon = 76;
                    break;
                case "제주":
                    lat = 52; lon = 38;
                    break;
            }
        }
        private async void Button_click(object sender, RoutedEventArgs e)
        {
            
            RootObject myWeather = await OpenWeatherMapProxy.GetWeather(lat, lon);
            string tmp = DateTime.Now.ToString("HH");
            int cmp = 99;
            string precipitation="";
            string sky="";
            string cur_temp="";
            foreach (Item it in myWeather.response.body.items.item)
            {
                if (it.category == "TMN")
                {
                    Min_temp.Text = "최저기온 : "+ it.fcstValue+ "℃";
                }
                else if(it.category=="TMX")
                {
                    Max_temp.Text = "최고기온 : "+ it.fcstValue+ "℃";
                }
                int t = Math.Abs(Int32.Parse(tmp) - Int32.Parse(it.fcstTime));
                if (t <= cmp)
                {
                    cmp = t;
                    if (it.category == "POP")
                    {
                        precipitation = it.fcstValue;
                    }
                    else if (it.category == "SKY")
                    {
                        sky = it.fcstValue;
                    }
                    else if (it.category == "T3H")
                    {
                        cur_temp = it.fcstValue;
                    }
                }
            }
            if (sky == "1")
            {
                Sky.Text = "맑음";
                img.Source = new BitmapImage(new Uri("ms-appx:///Assets/sunny.png"));
            }
            else if (sky == "3"||sky=="4")
            {
                if (Int32.Parse(precipitation) < 80)
                {
                    Sky.Text = "구름";
                    img.Source = new BitmapImage(new Uri("ms-appx:///Assets/clouds.png"));
                }
                else
                {
                    Sky.Text = "구름";
                    img.Source = new BitmapImage(new Uri("ms-appx:///Assets/rain.png"));
                }
            }
            Precipitation.Text = "강수확률 : "+precipitation+"%";
            Cur_temp.Text = cur_temp+ "℃";
        }


    }
}
