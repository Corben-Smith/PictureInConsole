using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Channels;
using System.Xml;
using FastConsole;


namespace ImageToConsole;
public class ConsoleImage
{
    /*
     * The purpose of this method is to Draw an ascii image to the console if given a bitmap
     * 
     * 
     * These are for what is effectively resolution, Some images with high resolution will be too large to even fit into the console window, try 1-5 for smaller images and 10-20 for larger ones
     *      xSKip - The amount of horizontal lines it will skip before it reads the next pixel
     *      ySkip - the amount of vertical lines it will skip before it reads the next pixel
     *      
     *      xBuffer - Space between left edge of the console and image
     *      yBuffer - Space between top edge of the console and image
     *      
     *  title - titles the window - for FConsole - Could probably edit the FC class in future to not require this
     *  adjust - adjust color values to pop more - Doesn't really work yet, need to find good values
     *  
     *  
     * 
     */
    public static void DrawImage(Bitmap bmp, int xSkip = 1, int ySkip = 2, int xBuffer = 0, int yBuffer = 0, string title = "Console", bool adjust = false)
    {
        FConsole.Initialize("asdf");


        short yi = (short)yBuffer;
        short xi = (short)xBuffer;
    
        for (short y = 0; y < bmp.Height; y += (short)ySkip)
        {

            for (short x = 0; x < bmp.Width; x += (short)xSkip)
            {
                //this is the one that makes it run slow
                char dispLetter = SetCharacter(bmp, x, y, xSkip, ySkip);

                //this makes it run fast but i can't do luminance and borders
                //char dispLetter = '0';

                //makes a new pixel and adds that pixel to the buffer
                PixelValue pixel = new PixelValue(ClosestConsoleColor(bmp.GetPixel(x,y)), ConsoleColor.Black, dispLetter);
                FConsole.SetChar(xi, yi, pixel);

                xi++;
            }

            yi++;
            xi = (short)xBuffer;
        }
        
        //draws whole buffer
        FConsole.DrawBuffer();
    }

    /* 
     * The SetCharacter method’s job is to do two things, determine if the pixel its given is a border, and if not, call the CalculateLuminance method
     */


    static char SetCharacter(Bitmap bmp, short x, short y, int xSkip, int ySkip)
    {
        char ret = '0';

        //checks if on the edge of the picture, becasue if it is, we cannot get the surronding pixels
        if (x <= xSkip || y <= ySkip || x >= bmp.Width - xSkip || y >= bmp.Height - ySkip)
        {
            ret = CalculateLuminance(bmp.GetPixel(x, y));
            return ret;
        }

        //sets all of the colors around the pixel
        //I think this is the main source of my slowdown
        ConsoleColor top = ClosestConsoleColor(bmp.GetPixel(x, y - ySkip));
        ConsoleColor topRight = ClosestConsoleColor(bmp.GetPixel(x + xSkip, y - ySkip));
        ConsoleColor right = ClosestConsoleColor(bmp.GetPixel(x + xSkip, y));
        ConsoleColor bottomRight = ClosestConsoleColor(bmp.GetPixel(x + xSkip, y));
        ConsoleColor bottom = ClosestConsoleColor(bmp.GetPixel(x, y + xSkip));
        ConsoleColor bottomLeft = ClosestConsoleColor(bmp.GetPixel(x, y + ySkip));
        ConsoleColor left = ClosestConsoleColor(bmp.GetPixel(x - xSkip, y - ySkip));
        ConsoleColor topLeft = ClosestConsoleColor(bmp.GetPixel(x - xSkip, y - ySkip));


        //this is just a workaround i thought of but didn't work well
        /*
        if (top == ConsoleColor.Black)
        {
            if (top == ConsoleColor.Black && left == ConsoleColor.Black)
            {
                ret = '╱';
            }
            else if(top == ConsoleColor.Black && right == ConsoleColor.Black)
            {

            }
            else
            {
                ret = '─';
            }
        }
        else if (bottom == ConsoleColor.Black)
        {

            if (bottom == ConsoleColor.Black && left == ConsoleColor.Black)
            {
                ret = '╲';
            }
            else if (bottom == ConsoleColor.Black && right == ConsoleColor.Black)
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
        */


        //checks the colors around the pixel and determines what the character should be depending on that
        //I think i could make this more efficent, but i don't think it matters much
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
            else if (topLeft == ConsoleColor.Black)
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
            //if it is not a border, calculate like a non-border pixel
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
        // This is the source of all that is wrong with the world.
        // I do not know what is making this so slow

        var consoleColors = Enum.GetValues(typeof(ConsoleColor));
        foreach (ConsoleColor cc in consoleColors)
        {
            //Gets the name of the console color
            var n = Enum.GetName(typeof(ConsoleColor), cc);

            //Fixes some bug with the Console colors
            //assigns the color name to c
            var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n);


            //Calculates the distance from the 3 values and the console color
            //Euclidian distance
            var t = (c.R - rr) * (c.R - rr) + (c.G - gg) * (c.G - gg) + (c.B - bb) * (c.B - bb);


            //checks if the current console color is closer than a previous one, if so, set return to the new closest console color
            if (t == 0.0)
            {
                return cc;

            }
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

        //equation for luminance
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
