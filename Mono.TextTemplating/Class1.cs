using System;
using System.Diagnostics;

public class Measure
{
    static Stopwatch sw = Stopwatch.StartNew();
    public static void Step(string messasge)
    {
        var e = sw.Elapsed;
        Console.WriteLine($"[ {e.TotalMilliseconds:0} ms ] " + messasge);
        sw.Restart();

    }
    public static void Step()
    {
        sw.Restart();
    }
}