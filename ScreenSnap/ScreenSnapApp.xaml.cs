using System;
using MahApps.Metro.Controls;
using ScreenGrab;
using System.IO;
using System.Drawing;
using System.ComponentModel;

namespace ScreenSnap
{
    /// <summary>
    /// Interaction logic for ScreenSnapApp.xaml
    /// </summary>
    public partial class ScreenSnapApp : MetroWindow
    {
        BackgroundWorker _worker;

        public ScreenSnapApp()
        {
            InitializeComponent();
            InitializeValues();
            InitializeWorker();
        }

        #region Worker Methods

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() => SnapButton.IsEnabled = false);

            Grabber grabber;
            if (Dispatcher.Invoke(() => IsFullScreen.IsChecked.Value))
            {
                grabber = new Grabber();
            }
            else
            {
                grabber = new Grabber(
                    (int) Dispatcher.Invoke(() => XPosition.Value),
                    (int) Dispatcher.Invoke(() => YPosition.Value),
                    (int) Dispatcher.Invoke(() => SnapWidth.Value),
                    (int) Dispatcher.Invoke(() => SnapHeight.Value));
            }

            Bitmap bmp = null;
            var count = (int) Dispatcher.Invoke(() => Count.Value);
            var imageName = Dispatcher.Invoke(() => ImageName.Text);
            Directory.CreateDirectory("ScreenShots");
            for (int i = 0; i < count; i++)
            {
                _worker.ReportProgress((i + 1) * 100 / count);
                var fileName = Path.GetFileNameWithoutExtension(imageName) + "-" + i + Path.GetExtension(imageName);
                bmp = grabber.ScreenShot();
                bmp.Save(Path.Combine("ScreenShots", fileName));
            }
            _worker.ReportProgress(0);
            if (bmp != null) bmp.Dispose();
            if (grabber != null) grabber.Dispose();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SnapProgressBar.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            SnapButton.IsEnabled = true;
        }

        #endregion Worker Methods

        #region Event Handlers

        private void IsFullScreen_Checked(object sender, EventArgs args)
        {
            EnableElements(false);
        }

        private void IsFullScreen_Unchecked(object sender, EventArgs args)
        {
            EnableElements(true);
        }

        private void SnapButton_Click(object sender, EventArgs args)
        {
            _worker.RunWorkerAsync();
        }

        #endregion Events Handlers

        private void EnableElements(bool isEnabled)
        {
            XPosition.IsEnabled = isEnabled;
            YPosition.IsEnabled = isEnabled;
            SnapWidth.IsEnabled = isEnabled;
            SnapHeight.IsEnabled = isEnabled;
        }

        private void InitializeWorker()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerComplete;
        }

        private void InitializeValues()
        {
            IsFullScreen.IsChecked = true;
            Count.Value = 1;
            XPosition.Value = 0;
            YPosition.Value = 0;
            SnapWidth.Value = 100;
            SnapHeight.Value = 100;

            EnableElements(false);
        }
    }
}
