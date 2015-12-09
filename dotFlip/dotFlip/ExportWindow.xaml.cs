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
using Microsoft.Win32;
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
            FramePicker.Maximum = Convert.ToByte(flipbook.PageCount);
            FramePicker.Minimum = 1;
            PathText.Text = System.AppDomain.CurrentDomain.BaseDirectory + "myProject";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbExportType.ItemsSource = Enum.GetValues(typeof (ExportType)).Cast<ExportType>();
            cmbExportType.SelectedItem = ExportType.Gif;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string path = PathText.Text;
            if(path.LastIndexOf(".") != -1) path = path.Substring(0, path.LastIndexOf("."));
            string pathWithOutEnd = (path.LastIndexOf("\\") != -1) ? path.Substring(0, path.LastIndexOf("\\")) : path;
            if (Directory.Exists(pathWithOutEnd))
            {
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
                    byte frameNum = FramePicker.Value ?? 0;
                    Bitmap bitmap = ConvertPageToBitmap(_flipbook.GetPageAt(frameNum - 1));
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
            else
            {
                MessageBox.Show("No such directory exists", "Unable to Export", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }        
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
            return bitmap;
        }

        private void SaveAsGif(IList<Bitmap> bitmaps, string path)
        {
            path += ".gif";
           AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            encoder.Start(path);
            encoder.SetRepeat(0);
            encoder.SetDelay(FrameDelayPicker.Value ?? 500);
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

        private void cmbExportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbExportType.SelectedItem.Equals(ExportType.Gif))
            {
                FramePicker.IsEnabled = false;
                FrameDelayPicker.IsEnabled = true;
            }
            else
            {
                FrameDelayPicker.IsEnabled = false;
                FramePicker.IsEnabled = true;
            }
        }

        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "Image File | *." + cmbExportType.SelectedItem.ToString().ToLower();
            saveFileDialog.DefaultExt = cmbExportType.SelectedItem.ToString().ToLower();
            if (saveFileDialog.ShowDialog() == true)
            {
                PathText.Text = saveFileDialog.FileName;
                
            }
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
