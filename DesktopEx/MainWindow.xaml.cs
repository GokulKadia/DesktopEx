using DesktopEx;
using Haley.WPF.Controls;
using Microsoft.Win32;
using Syncfusion.Lic.util.encoders;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static DesktopEx.MainWindow;



namespace DesktopEx
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetSysColors(int cElements, int[] lpaElements, uint[] lpaRgbValues);

        public const int COLOR_DESKTOP = 1;
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x01;
        public const int SPIF_SENDWININICHANGE = 0x02;
        
        public string backgroundColor = "";

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        //This method will set the background color of the desktop splited in Centered,Tiled and Streched
        //and also does change in registry of desktop.
        public static void Set(ExampleEnum exampleEnum)
        {
            
            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "C:\\Users\\Nitesh_Kumar\\OneDrive - Dell Technologies\\Desktop\\wallpaper\\desktop_Background_03.jpg.jpg");
            
            RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            
                       
                switch (exampleEnum)
                {
                    case ExampleEnum.Centered:
                        key.SetValue(@"WallpaperStyle", "1");
                        //key.SetValue(@"TileWallpaper", 1.ToString());
                        break;
                    case ExampleEnum.Tiled:
                        key.SetValue(@"WallpaperStyle", 1.ToString());
                        //key.SetValue(@"TileWallpaper", 0.ToString());
                        break;
                    case ExampleEnum.Stretched:
                        key.SetValue(@"WallpaperStyle", 2.ToString());
                        //key.SetValue(@"TileWallpaper", 0.ToString());
                        break;
                    default:
                        break;
                }
            
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, tempPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        //This method sets the wallpaper from a particular path and make changes in particular registry.
        private void ApplyWallpaper(object sender, RoutedEventArgs e)
        {     
            
            Set(ExampleEnum.Centered);           
        }

        //This method is called when we click apply color and the color gets applied.
        public void ApplyCol(object sender, RoutedEventArgs e)
        {
            // Save value in registry so that it will persist
            TestFunc();

        }

        //This method will return an R,G,B of a color i.e., called into backgroundColor 
        public string GenerateRgba()
        {
            System.Drawing.Color color = ColorTranslator.FromHtml(backgroundColor);//background color is the color which will be picked from color picker.
            int r = Convert.ToInt16(color.R);
            int g = Convert.ToInt16(color.G);
            int b = Convert.ToInt16(color.B);

            return string.Format("{0} {1} {2}", r, g,b);
        }

        //This method is called when the Browse button is clicked to get a particular
        //background picture of desktop. It will open a particular path where the picture is kept.
        public void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            if (result == true)
            {
                Browse_TextBox.Text = openFileDlg.FileName;
            }
            
        }

        public System.Drawing.Color color1 = new System.Drawing.Color();

        //This method is changing the color after taking a particular color from backgroundColor(Picked from color picker)
        //and make changes in the registry with the latest color selected and it will reflect the color in background of desktop.
        //immidiately without restarting.
        public void TestFunc()
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER,0,"",SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            int[] elements = { COLOR_DESKTOP };
            int[] colors = { System.Drawing.ColorTranslator.ToWin32(ColorTranslator.FromHtml(backgroundColor)) };
            uint[] uArr = new uint[colors.Length]; 
            for(int i=0; i<colors.Length; i++) 
            {
                uArr[i] = Convert.ToUInt32(colors[i]);
            }

            SetSysColors(elements.Length, elements, uArr);
            var Key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Colors", true);

            string tmp = (string)Key.GetValue("Background");

            string strinm = GenerateRgba();
            Key.SetValue(@"Background", strinm);
        }

        //setting the background color from color picker as string.
        private void colorPicker_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            backgroundColor = e.NewValue.ToString();
        }
        

    }
}

