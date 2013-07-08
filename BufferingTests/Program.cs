using System;
using BufferingTests.Buffering;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;

namespace BufferingTests
{
    public class Program
    {
        private static Font font = Resources.GetFont(Resources.FontResources.NinaB);
        private static BufferContext _buffer;
        private static ManualResetEvent evt;
        private static Bitmap _screen;
        private static System.Random rnd = new Random();

        public static void Main()
        {
            evt = new ManualResetEvent(false);
            _buffer = new BufferContext();
            _screen = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);
            ThreadUtil.SafeQueueWorkItem(new ThreadStart(SetupZeroBuffer));
            ThreadUtil.SafeQueueWorkItem(new ThreadStart(SetupOtherBuffers));

            evt.WaitOne();
            ThreadUtil.SafeQueueWorkItem(new ThreadStart(RenderFast));
            Thread.Sleep(Timeout.Infinite);
        }


        private static void RenderFast()
        {
            while (true)
            {
                _screen.Clear();
                for (int i = 0; i < _buffer.Count; i++)
                {
                    var ctx = _buffer[i];
                    _screen.DrawImage(rnd.Next(110) + 10, rnd.Next(110) + 10, ctx, 0, 0, ctx.Width, ctx.Height);
                    _screen.Flush();
                    Thread.Sleep(1);
                }
            }
        }


        private static int x, y;

        public static void SetupZeroBuffer()
        {
            //add it to our buffer context
            var bmp = new Bitmap(10, 10);
            bmp.DrawLine(Color.White, 1, 0, 0, 1, 1);
            _buffer.Add(bmp);

            //render it right away
            _screen.Clear();
            _screen.DrawImage(0, 0, bmp, 0, 0, bmp.Width, bmp.Height);
            _screen.Flush();
        }

        public static void SetupOtherBuffers()
        {
            int lineLength = 10;
            for (int x = 1; x <= lineLength; x++)
            {
                int x0 = x;
                int y0 = x;
                int x1 = x0+x;
                int y1 = y0+x;
                //create the buffered image
                var bmp = new Bitmap(lineLength, lineLength);
                bmp.DrawLine(Color.White, x, x0, y0, x1, y1);
                //add it to our list of buffered images
                _buffer.Add(bmp);
            }

            evt.Set();
        }


    }
}
