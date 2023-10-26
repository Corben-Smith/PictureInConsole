using System;
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
        string img = "smurfcat.jpg";
        var path = root + @$"\images\{img}";

        FConsole.Initialize("asdf");

        while (true)
        {
            ConsoleImage.DrawImage(img, 1,3);
            ConsoleImage.DrawImage(img, 1, 4);

        }

    }
}