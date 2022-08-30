using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Image_Ascii
{
    internal class ImageToAsciiProgram
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool isBlack = true;
            var key = ConsoleKey.Enter;
            bool inMenu = true;
            int selectedMode = 0;
            while (inMenu)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Select procession mode");
                Console.WriteLine("1   - Grayscale");
                Console.WriteLine("2   - Black & white (you can adjust brightness limit with A-D keys (Left/Right arrows))");
                Console.WriteLine("R   - Change color scheme");
                Console.WriteLine("Esc - close");
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        selectedMode = 1;
                        inMenu = false;
                        break;
                    case ConsoleKey.D2:
                        selectedMode = 2;
                        inMenu = false;
                        break;
                    case ConsoleKey.R:
                        ChangeColors(ref isBlack);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }

            string imagePath = String.Empty;
            using (var fileDialog = new OpenFileDialog())
            {

                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fileDialog.Filter = "Изображения (*.png; *.jpg; *.jpeg; *.bmp)|*.png; *.jpg; *.jpeg; *.bmp|Все файлы (*.*)|*.*";
                fileDialog.Title = "Изображение для преобразования";

                while (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    Console.Clear();
                    Console.WriteLine("Select image file");
                    Console.WriteLine("Esc - exit");
                    Console.WriteLine("Press any key to continue...");
                    imagePath = fileDialog.FileName;
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape)
                        return;
                }
            }

            ChangeColors(ref isBlack);
            StringBuilder sb = new StringBuilder();
            try
            {
            ImageProcessor imageProcessor = new ImageProcessor(imagePath);
                double adjustment = 255 / 2.0;
                var img = imageProcessor.ProcessToAscii(selectedMode, adjustment);
                DrawImage(img, sb);
                bool isRunnig = true;
                while (isRunnig)
                {
                    key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            adjustment -= 5;
                            if (adjustment < 0) adjustment = 0;
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            adjustment += 5;
                            if (adjustment > 255) adjustment = 255;
                            break;
                        case ConsoleKey.R:
                            ChangeColors(ref isBlack);
                            break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Enter:
                            isRunnig = false;
                            WriteFile(sb.ToString(), GetAnotherExtention(imagePath, ".txt"));
                            break;
                    }
                    img = imageProcessor.ProcessToAscii(selectedMode, adjustment);
                    DrawImage(img, sb);
                }
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ConstructImage(StringBuilder target, AsciiImage source)
        {
            target.Clear();
            target.EnsureCapacity(source.Width * source.Height);
            for (int y = 0; y < source.Height; ++y)
            {
                for (int x = 0; x < source.Width; ++x)
                {
                    target.Append(source[x, y]);
                }
                target.Append('\n');
            }
        }

        static void DrawImage(AsciiImage img, StringBuilder sb)
        {
            if (img.Width > 0 && img.Height > 0)
            {
                Console.SetCursorPosition(0, 0);
                ConstructImage(sb, img);
                Console.WriteLine(sb.ToString());
            }
        }

        static void ChangeColors(ref bool isBlack)
        {
            if (isBlack)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            isBlack = !isBlack;
            Console.Clear();
        }

        static void WriteFile(string img, string outFile)
        {
            using (var sw = new StreamWriter(outFile, false))
            {
                sw.WriteLine(img);
            }
        }

        static string GetAnotherExtention(string file, string newExt)
        {
            var fi = new FileInfo(file);
            var ext = fi.Extension;
            return file.Replace(ext, newExt);
        }
    }
}

