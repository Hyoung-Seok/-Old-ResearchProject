using System.Collections.Generic;
using System.Diagnostics;

public static class PrefCheck
{
    public static List<(int, int)> CheckedTiles { get; private set; } = new List<(int, int)>();
    public static int LoopCount { get; private set; }
    public static float ElapsedTime { get; private set; }
    
    private static Stopwatch _stopwatch = new Stopwatch();

    public static void StartPrefCheck()
    {
        _stopwatch.Start();
        CheckedTiles.Clear();
        LoopCount = 0;
        ElapsedTime = 0;
    }

    public static void EndPrefCheck()
    {
        _stopwatch.Stop();
        ElapsedTime = _stopwatch.ElapsedMilliseconds / 1000f;
    }

    public static void AddCheckTile((int, int) pos)
    {
        CheckedTiles.Add(pos);
        LoopCount++;
    }
}
