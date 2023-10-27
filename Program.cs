using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Threading.Channels;
using System.Xml;
using FastConsole;

namespace ImageToConsole;
public class Program
{
    public static void Main()
    {
        //getting the path to the image
        var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
        string img = "smurfcat.jpg";
        var path = root + @$"\images\{img}";

        //the image
        Bitmap bmp = new Bitmap(path);

        img = "stickman1.png";
        path = root + @$"\images\{img}";

        //the image
        Bitmap stickOneBmp = new Bitmap(path);

        img = "stickman2.png";
        path = root + @$"\images\{img}";

        //the image
        Bitmap stickTwoBmp = new Bitmap(path);



        //im sorry, morgan taught me about lists
        List<long> time = new List<long>();

        //running 10 times and adding the time it takes for it to complete to a list
        for (int i = 0; i < 10; i++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ConsoleImage.DrawImage(bmp, 1, 4);
            sw.Stop();
            time.Add(sw.ElapsedMilliseconds);
        }


        //This is for diagnostic information
        //It displays the runtimes, the total time, then the average time
        long sum = 0; 
        foreach (var item in time)
        {
            sum += item;
            Console.WriteLine(item);
        }
        Console.WriteLine(sum);
        Console.WriteLine(sum / 10);

        Console.ReadLine();

        //will display an image until you close the program

        /*
        while (true)
        {
            ConsoleImage.DrawImage(bmp, 1, 4);
        }
        */

        while (true)
        {
            ConsoleImage.DrawImage(stickOneBmp);
            ConsoleImage.DrawImage(stickTwoBmp);
        }
    }
}