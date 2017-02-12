using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenGrab
{
    public class Grabber : IDisposable
    {
        private readonly Graphics g;
        private readonly Bitmap bmp;
        private readonly Point pt;

        /// <summary>
        /// Construct a Grabber to get the whole screen.
        /// </summary>
        public Grabber()
            : this(
                0,0,
                Screen.PrimaryScreen.WorkingArea.Width,
                Screen.PrimaryScreen.WorkingArea.Height)
        {
        }

        /// <summary>
        /// Construct the grabber to get a portion of the screen.
        /// </summary>
        /// <param name="x">The starting x coord.</param>
        /// <param name="y">The starting y coord.</param>
        /// <param name="width">The width to grab.</param>
        /// <param name="height">The height to grab.</param>
        public Grabber(int x, int y, int width, int height)
        {
            pt = new Point(x, y);

            bmp = new Bitmap(width, height);
            g = Graphics.FromImage(bmp);
        }

        public Bitmap ScreenShot()
        {
            g.CopyFromScreen(pt, new Point(), bmp.Size);
            return bmp;
        }

        public void Dispose()
        {
            bmp.Dispose();
            g.Dispose();
        }
    }
}
