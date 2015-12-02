using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace dotFlip
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {

        private bool _closeStoryBoardCompleted = false;
        private Storyboard _closeStoryboard;
        private bool _result;

        public Notification(String title, String message, Point lowerRightPoint)
        {
            InitializeComponent();

            Title.Inlines.Add(new Run() {Text = title, FontWeight = FontWeights.Bold});
            Message.Text = message + "\n";
            Closing += Notification_OnClosing;
            _closeStoryboard = (Storyboard) FindResource("OnClosingStoryboard");
            _closeStoryboard.Completed += CloseStoryboardOnCompleted;
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var corner = lowerRightPoint;
                this.Left = corner.X - this.ActualWidth - (SystemParameters.ResizeFrameVerticalBorderWidth * 4);
                this.Top = lowerRightPoint.Y - this.ActualHeight - (SystemParameters.WindowCaptionHeight + SystemParameters.ResizeFrameHorizontalBorderHeight * 4);
            }));
        }

        private void CloseStoryboardOnCompleted(object sender, EventArgs eventArgs)
        {
            _closeStoryBoardCompleted = true;
            this.Close();
        }

        private void BtnYes_OnClick(object sender, RoutedEventArgs e)
        {
            _result = true;
            this.Close();
        }

        private void BtnNo_OnClick(object sender, RoutedEventArgs e)
        {
            _result = false;
            this.Close();
        }


        private void Notification_OnClosing(object sender, CancelEventArgs e)
        {
            if (!_closeStoryBoardCompleted)
            {
                _closeStoryboard.Begin(this);
                e.Cancel = true;
            }
            DialogResult = _result;
        }
    }
}
