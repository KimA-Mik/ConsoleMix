namespace Image_Ascii
{
    internal class AsciiImage
    {
        private char[] Image { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public char DefaultChar {get; set;}

        public AsciiImage(int width, int height)
        {
            DefaultChar = ' ';
            Init(width, height);
        }

        public AsciiImage()
        {
            DefaultChar = ' ';
        }

        public void Init(int width, int height)
        {
            Width = width;
            Height = height;
            Image = new char[Width * Height];
            for (int i = 0; i < Image.Length; ++i)
                Image[i] = DefaultChar;
        }

        public char this[int x, int y]
        {
            get
            {
                var i = GetIndex(x, y);
                return (i >= 0) ? Image[i] : DefaultChar;
            }

            set
            {
                var i = GetIndex(x, y);
                if (i >= 0) Image[i] = value;
            }
        }

        int GetIndex(int x, int y)
        {
            return (x < 0 || x >= Width || y < 0 || y >= Height) ? -1 : x + y * Width;
        }
    }
}
