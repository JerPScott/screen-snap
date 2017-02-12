using NUnit.Framework;
using ScreenGrab;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Test
{
    [TestFixture]
    public class ScreenSnapTest
    {
        [TestCase(0,0,100,100)]
        [TestCase(1,1,1,1)]
        public void ConstructGrabber_WithDimensions(int x, int y, int width, int height)
        {
            // arrange, act, assert
            Assert.DoesNotThrow(
                () => { var grabber = new Grabber(x, y, width, height); },
                "Exception thrown when creating grabber");
        }

        [Test]
        public void ConstructGrabber_FullScreen()
        {
            // arrange, act, assert
            Assert.DoesNotThrow(
                () => { var grabber = new Grabber(); },
                "Exception thrown when creating grabber");
        }

        [TestCase(0,0,100,100)]
        [TestCase(1,1,1,1)]
        public void ScreenShot_WithDimensions(int x, int y, int width, int height)
        {
            // arrange
            var grabber = new Grabber(x, y, width, height);

            // act
            var bmp = grabber.ScreenShot();

            // assert
            Assert.That(
                bmp, Is.Not.Null,
                "Bitmap was not created");

            Assert.That(
                bmp.Width, Is.EqualTo(width),
                "The Bitmap had an incorrect width.");

            Assert.That(
                bmp.Height, Is.EqualTo(height),
                "The Bitmap had an incorrect height.");
        }

        [Test]
        public void ScreenShot_FullScreen()
        {
            // arrange
            var grabber = new Grabber();

            // act
            var bmp = grabber.ScreenShot();

            // assert
            Assert.That(
                bmp, Is.Not.Null,
                "Bitmap was not created");

            Assert.That(
                bmp.Width, Is.EqualTo(Screen.PrimaryScreen.WorkingArea.Width),
                "The Bitmap had an incorrect width.");

            Assert.That(
                bmp.Height, Is.EqualTo(Screen.PrimaryScreen.WorkingArea.Height),
                "The Bitmap had an incorrect height.");
        }

        [Test]
        public void ScreenShot_Multiple()
        {
            // arrange
            const int count = 100;
            Bitmap bmp;
            var sw = new Stopwatch();
            var grabber = new Grabber();

            // act
            sw.Start();
            for (int i = 0; i < count; i++)
            {
                bmp = grabber.ScreenShot();
            }
            sw.Stop();

            // assert
            Assert.That(
                sw.Elapsed.TotalMilliseconds / count, Is.LessThan(20),
                "The screen shots took longer than required.");
        }
    }
}
