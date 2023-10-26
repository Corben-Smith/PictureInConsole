using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Channels;
using System.Xml;
using FastConsole;

class Program
{
    static void Main()
    {

        //Console.WriteLine("What image would you like to show?");
        //string path = Console.ReadLine();
        FConsole.Clear();


        while (true)
        {
            DrawImage("smurfcat.jpg" , 1, 2);
        }


    }


    static void DrawImage(string path, int xSkip = 1, int ySkip = 2, int xBuffer = 0, int yBuffer = 0, string title = "Console", bool adjust = false)
    {

        Bitmap bmp = new Bitmap(@$"C:\CSCI1250\Self-projects\img\ConsoleApp1\ConsoleApp1\{path}");

        FConsole.Initialize(title);


        string[] widthColor = new string[bmp.Width];

        short yi = (short)yBuffer;
        short xi = (short)xBuffer;



        for (short y = 0; y < bmp.Height; y += (short)ySkip)
        {


            for (short x = 0; x < bmp.Width; x += (short)xSkip)
            {
                PixelValue pixel = new PixelValue();
                char dispLetter = SetCharacter(bmp, x, y, xSkip, ySkip);


                pixel = new PixelValue(ClosestConsoleColor(bmp.GetPixel(x, y), adjust:adjust), ConsoleColor.Black, dispLetter);
                FConsole.SetChar(xi, yi, pixel);
                
                xi++;
            }

            yi++;
            xi = (short)xBuffer;
        }

        FConsole.DrawBuffer();
    }

    static char SetCharacter(Bitmap bmp, short x, short y, int xSkip, int ySkip)
    {
        char ret = '0';

        if(x <= xSkip || y <= ySkip || x >= bmp.Width - xSkip || y >= bmp.Height - ySkip)
        {
            ret = CalculateLuminance(bmp.GetPixel(x,y));
            return ret;
        }

        ConsoleColor top = ClosestConsoleColor(bmp.GetPixel(x, y - ySkip));
        ConsoleColor topRight = ClosestConsoleColor(bmp.GetPixel(x + xSkip, y - ySkip));
        ConsoleColor right = ClosestConsoleColor(bmp.GetPixel(x + xSkip, y));
        ConsoleColor bottomRight = ClosestConsoleColor(bmp.GetPixel(x + xSkip, y));
        ConsoleColor bottom = ClosestConsoleColor(bmp.GetPixel(x, y + xSkip));
        ConsoleColor bottomLeft = ClosestConsoleColor(bmp.GetPixel(x, y + ySkip));
        ConsoleColor left = ClosestConsoleColor(bmp.GetPixel(x - xSkip, y - ySkip));
        ConsoleColor topLeft = ClosestConsoleColor(bmp.GetPixel(x - xSkip, y - ySkip));



        if (top == ConsoleColor.Black)
        {
            if (topRight == ConsoleColor.Black && topLeft == ConsoleColor.Black)
            {
                ret = '─';
            }
            else if (topRight == ConsoleColor.Black)
            {
                ret = '╲';
            }
            else if(topLeft == ConsoleColor.Black)
            {
                ret = '╱';

            }
            else
            {
                ret = '─';
            }
        }
        else if (bottom == ConsoleColor.Black)
        {
            if (bottomRight == ConsoleColor.Black && bottomLeft == ConsoleColor.Black)
            {
                ret = '─';
            }
            else if (bottomRight == ConsoleColor.Black)
            {
                ret = '╱';
            }
            else if (bottomLeft == ConsoleColor.Black)
            {
                ret = '╲';

            }
            else
            {
                ret = '─';
            }
        }
        else if (right == ConsoleColor.Black)
        {
            ret = '▕';
        }
        else if (left == ConsoleColor.Black)
        {
            ret = '▏';
        }
        else
        {
            ret = CalculateLuminance(bmp.GetPixel(x, y));
        }


        return ret;
    }


    //Calculates the closest Console Color to the pixel's color
    //not mine
    static ConsoleColor ClosestConsoleColor(Color color, bool adjust = false)
    {
        ConsoleColor ret = 0;

        //Assigning the rgb values
        double rr = (byte)color.R;
        double gg = (byte)color.G;
        double bb = (byte)color.B + 10;
        double delta = double.MaxValue;

        //for each Console Color there is
        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
        {
            //Gets the name of the console color
            var n = Enum.GetName(typeof(ConsoleColor), cc);

            //Fixes some bug in the Console colors
            //assigns the color name to c
            var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix

            //Calculates the distance from the 3 values and the console color
            //Euclidian distance
            var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);

            //checks if the current console color is closer than a previous one, if so, set return to the new closest console color
            if (t == 0.0)
            {
                return cc;

            }
            if (t < delta)
            {
                if(adjust == true)
                {
                    if (cc == ConsoleColor.Black)
                    {
                        t += 3000;
                    }
                    if (cc == ConsoleColor.DarkBlue || cc == ConsoleColor.Blue)
                    {
                        t -= 8000;
                    }
                }

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

        //exation for luminance
        double y = 0.2126 * r + 0.7152 * g + 0.0722 * b;
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
            ret = ':';

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