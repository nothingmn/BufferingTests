using System;
using System.Collections;
using Microsoft.SPOT;

namespace BufferingTests.Buffering
{
    public class BufferContext
    {
        public void DrawContext(int index)
        {
            var ctx = this[index];
            ctx.Flush();
        }
        public void Add(Bitmap buffer)
        {
            _backBuffers.Add(buffer); 
        }
        public Bitmap this[int index]
        {
            get { return (_backBuffers[index] as Bitmap); }
        }

        public int Count { get { return _backBuffers.Count; } }
        private ArrayList _backBuffers = new ArrayList();
        public int CurrentBuffer { get; set; }
    }
}
