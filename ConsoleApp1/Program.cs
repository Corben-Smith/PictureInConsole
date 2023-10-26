using System;
using System.Drawing;
using System.Threading.Channels;
using System.Xml;
using FastConsole;

class Program
{
    static void Main()
    {
        Console.ReadLine();

        while (true)
        {
            DrawImage(@"stickman1.png");
            DrawImage(@"stickman2.png");
        }
    }


    static void DrawImage(string path)
    {

        Bitmap bmp = new Bitmap(@$"C:\CSCI1250\Self-projects\img\ConsoleApp1\ConsoleApp1\{path}");

        FConsole.Initialize("Move man");




        string[] widthColor = new string[bmp.Width];

        short yi = 0;
        short xi = 0;

        for (short y = 0; y < bmp.Height; y += 2)
        {


            for (short x = 0; x < bmp.Width; x += 1)
            {

                Console.ForegroundColor = ClosestConsoleColor(bmp.GetPixel(x, y));

                PixelValue pixel = new PixelValue(ClosestConsoleColor(bmp.GetPixel(x, y)), ConsoleColor.Black, CalculateLuminance(bmp.GetPixel(x, y)));

                FConsole.SetChar(xi, yi, pixel);
                xi++;
            }

            yi++;
            xi = 0;
        }

        FConsole.DrawBuffer();
    }




    //Calculates the closest Console Color to the pixel's color
    //not mine
    static ConsoleColor ClosestConsoleColor(Color color)
    {
        byte r = (byte)color.R;
        byte g = (byte)color.G;
        byte b = (byte)color.B;

        ConsoleColor ret = 0;
        double rr = r, gg = g, bb = b, delta = double.MaxValue;

        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
        {
            var n = Enum.GetName(typeof(ConsoleColor), cc);
            var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
            var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
            if (t == 0.0)
                return cc;
            if (t < delta)
            {
                delta = t;
                ret = cc;
            }
        }
        return ret;
    }



    //Calculates the luminace of the given pixel
    static char CalculateLuminance(Color color)
    {
        byte r = (byte)color.R;
        byte g = (byte)color.G;
        byte b = (byte)color.B;


        double y = 0.299 * r + 0.587 * g + 0.114 * b;
        char ret = '.';

        if (y >= 95)
        {
            ret = '0';
        }
        else if (y >= 90)
        {
            ret = '{';

        }
        else if (y >= 70)
        {
            ret = '\\';

        }
        else if (y >= 60)
        {
            ret = '+';

        }
        else if (y >= 50)
        {
            ret = '~';

        }
        else if (y >= 40)
        {
            ret = '\"';

        }
        else if (y >= 30)
        {
            ret = '`';
        }
        else if (y >= 20)
        {
            ret = '\'';
        }
        else if (y >= 10)
        {
            ret = ',';
        }
        else
        {
            ret = '.';
        }

        return ret;
    }

}