using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Gif.Components;
using Image = System.Drawing.Image;
using Size = System.Windows.Size;

namespace dotFlip
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        private Flipbook _flipbook;

        public ExportWindow(Flipbook flipbook)
        {
            InitializeComponent();
            _flipbook = flipbook;
            btnCancel.Click += (sender, e) => this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbExportType.ItemsSource = Enum.GetValues(typeof (ExportType)).Cast<ExportType>();
            cmbExportType.SelectedItem = ExportType.Gif;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string path = "D:\\dotFlipSaves\\test";

            ExportType type = (ExportType) cmbExportType.SelectionBoxItem;

            if (type == ExportType.Gif)
            {
                IList<Bitmap> bitmaps = new List<Bitmap>();
                for (int ii = 0; ii < _flipbook.PageCount; ii++)
                {
                    bitmaps.Add(ConvertPageToBitmap(_flipbook.GetPageAt(ii)));
                }
                SaveAsGif(bitmaps, path);
            }
            else
            {
                //int frameNum = 0;
                Bitmap bitmap = ConvertPageToBitmap(_flipbook.CurrentPage);
                switch (type) 
                {
                    case ExportType.Bmp:
                        SaveAsBmp(bitmap, path);
                        break;
                    case ExportType.Jpg:
                        SaveAsJpg(bitmap, path);
                        break;
                    case ExportType.Png:
                        SaveAsPng(bitmap, path);
                        break;
                }

            }


            this.Close();
        }

        private Bitmap ConvertPageToBitmap(Page p)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int) p.ActualWidth, (int) p.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(p);

            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);

            Bitmap bitmap = new Bitmap(stream);
//            bitmap.Save("D:\\dotflipsaves\\test.bmp");
            return bitmap;
        }

        private void SaveAsGif(IList<Bitmap> bitmaps, string path)
        {
            path += ".gif";

           AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            encoder.Start(path);
            encoder.SetRepeat(0);
            encoder.SetDelay(500);
            for (int ii = 0; ii < _flipbook.PageCount; ii++)
            {
                encoder.AddFrame(bitmaps[ii]);
            }
            encoder.Finish();
        }

        private void SaveAsPng(Bitmap bitmap, string path)
        {
            path += ".png";
            bitmap.Save(path, ImageFormat.Png);
        }

        private void SaveAsJpg(Bitmap bitmap, string path)
        {
            path += ".jpg";
            bitmap.Save(path, ImageFormat.Jpeg);
        }

        private void SaveAsBmp(Bitmap bitmap, string path)
        {
            path += ".bmp";
            bitmap.Save(path, ImageFormat.Bmp);   
        }
    }

    public enum ExportType
    {
        Gif,
        Jpg,
        Png,
        Bmp
    }
}
