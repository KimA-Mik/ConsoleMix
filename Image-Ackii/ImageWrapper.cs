using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Image_Ascii
{
    public class ImageWrapper : IDisposable, IEnumerable<Point>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Color DefaultColor { get; set; }

        private byte[] _data;
        private byte[] _outData;
        private int _stride;
        private BitmapData _bmpData;
        private Bitmap _bitmap;

        public ImageWrapper(Bitmap bmp, bool copySourceToOutput = false)
        {
            Width = bmp.Width;
            Height = bmp.Height;
            _bitmap = bmp;

            _bmpData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _stride = _bmpData.Stride;

            _data = new byte[_stride * Height];
            System.Runtime.InteropServices.Marshal.Copy(_bmpData.Scan0, _data, 0, _data.Length);

            _outData = copySourceToOutput ? (byte[])_data.Clone() : new byte[_stride * Height];
        }

        public Color this[int x, int y]
        {
            get
            {
                var i = GetIndex(x, y);
                return i < 0 ? DefaultColor : Color.FromArgb(_data[i + 3], _data[i + 2], _data[i + 1], _data[i]);
            }

            set
            {
                var i = GetIndex(x, y);
                if (i >= 0)
                {
                    _outData[i] = value.B;
                    _outData[i + 1] = value.G;
                    _outData[i + 2] = value.R;
                    _outData[i + 3] = value.A;
                }
            }
        }

        public Color this[Point p]
        {
            get { return this[p.X, p.Y]; }
            set { this[p.X, p.Y] = value; }
        }

        public void SetPixel(Point p, double r, double g, double b)
        {
            if (r < 0) r = 0;
            if (r > 255) r = 255;
            if (g < 0) g = 0;
            if (g > 255) g = 255;
            if (b < 0) b = 0;
            if (b > 255) b = 255;

            this[p.X, p.Y] = Color.FromArgb((int)r, (int)g, (int)b);
        }

        public void SwapBuffers()
        {
            (_data, _outData) = (_outData, _data);
        }

        int GetIndex(int x, int y)
        {
            return (x < 0 || x >= Width || y < 0 || y >= Height) ? -1 : x * 4 + y * _stride;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                    yield return new Point(x, y);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Dispose()
        {
            System.Runtime.InteropServices.Marshal.Copy(_outData, 0, _bmpData.Scan0, _outData.Length);
            _bitmap.UnlockBits(_bmpData);
        }
    }
}
