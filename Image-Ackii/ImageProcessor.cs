using System.Drawing;

namespace Image_Ascii
{
    internal class ImageProcessor
    {
        Bitmap _bitmap;
        //const string Palette = " .'`^\",:;Il!i><~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwepdbkhao*#MW&8%B@$";
        //const string Palette = "@%#*++-:. ";
        const string Palette = "$@B%8&WM#*oahkbdpewmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
        //int gs = 0.2162 * R + 0.7152 * GC + 0.0722 * B;
        int SectorWidth = 5;
        int SectorHeight = 10;
        public ImageProcessor(string imgPath)
        {
            _bitmap = new Bitmap(imgPath);
        }

        public AsciiImage ProcessToAscii(int mode, double adjust = 255.0 / 2)
        {
            AsciiImage ascii = new AsciiImage();
            using (var image = new ImageWrapper(_bitmap, true))
            {
                if (image.Width < 160)
                    SectorWidth = 1;
                else
                    SectorWidth = image.Width / 160;
                SectorHeight = SectorWidth * 2;

                ascii.Init(image.Width / SectorWidth, image.Height / SectorHeight);
                for (int y = 0; y < image.Height; y += SectorHeight)
                    for (int x = 0; x < image.Width; x += SectorWidth)
                    {
                        var col = GetSectorColor(image, x, y, SectorWidth, SectorHeight);
                        char newVal = (char)0;
                        switch (mode)
                        {
                            case 1:
                                newVal = GrayscaleModeI(col);
                                break;
                            case 2:
                                newVal = BlackWhiteMode(col, adjust);
                                break;
                        }

                        ascii[x / SectorWidth, y / SectorHeight] = newVal;
                    }
            }
            return ascii;
        }

        char GrayscaleModeI(Color col)
        {
            double grayscale = 0.2150 * col.R + 0.7140 * col.G + 0.0710 * col.B;
            //grayscale = ((double)col.R + (double)col.G + (double)col.B) / 3.0;
            if (grayscale > 254.5) grayscale = 255;
            if (grayscale < 0) grayscale = 0;
            int ind = (int)(grayscale / 255.0 * (Palette.Length - 1));

            return Palette[ind];
        }

        char BlackWhiteMode(Color col, double adjust)
        {
            double grayscale = 0.2150 * col.R + 0.7140 * col.G + 0.0710 * col.B;
            //grayscale = ((double)col.R + (double)col.G + (double)col.B) / 3.0;
            if (grayscale > 254.5) grayscale = 254.5;
            if (grayscale < 0) grayscale = 0;

            return (grayscale < (adjust)) ? Palette[0] : Palette[Palette.Length - 1];
        }

        Color GetSectorColor(ImageWrapper src, int x, int y, int curSectorWidth, int curSectorHeight)
        {
            var col = src[x, y];
            double r = col.R;
            double g = col.G;
            double b = col.B;
            double a = col.A;
            for (int curY = 0; curY < curSectorHeight; ++curY)
                for (int curX = 0; curX < curSectorWidth; ++curX)
                {
                    var color = src[x + curX, y + curY];
                    r = (color.R + r) / 2.0;
                    g = (color.G + g) / 2.0;
                    b = (color.B + b) / 2.0;
                    a = (color.A + b) / 2.0;
                }
            if (a == 0)
                return Color.FromArgb(255, 255, 255);
            
            return Color.FromArgb((int)r, (int)g, (int)b);
        }
    }
}
