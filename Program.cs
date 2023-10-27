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
        //bool run = true;

        var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
        string img = "ian.png";
        var path = root + @$"\images\{img}";

        Bitmap bmp = new Bitmap(path);

        List<long> time = new List<long>();

        for (int i = 0; i < 10; i++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ConsoleImage.DrawImage(bmp, 15, 20);
            sw.Stop();
            time.Add(sw.ElapsedMilliseconds);
        }

        long sum = 0; 
        foreach (var item in time)
        {
            sum += item;
            Console.WriteLine(item);
        }
        Console.WriteLine(sum);
        Console.WriteLine(sum / 10);

    }
}