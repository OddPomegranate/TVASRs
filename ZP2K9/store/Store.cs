using System;
using System.IO;
using IMAK3Z0MB1EGAEM;
using IMAK3Z0MB1EGAEM.menu;

namespace ZP2K9.store;

public class Store
{
    public const int STORE_SCORES = 0;
    public string mapPath = "map";

    private static string SaveDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeViking");

    private static string ScoresPath => Path.Combine(SaveDirectory, "scores.zdx");

    public void Update() { } // no device-selection state machine needed on PC

    public void Write()
    {
        try
        {
            Directory.CreateDirectory(SaveDirectory);
            using var writer = new BinaryWriter(File.Open(ScoresPath, FileMode.Create, FileAccess.Write));
            try { HighScores.Write(writer); } catch { }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Read()
    {
        if (!File.Exists(ScoresPath))
        {
            HighScores.Init();
            return;
        }
        try
        {
            using var reader = new BinaryReader(File.Open(ScoresPath, FileMode.Open, FileAccess.Read));
            try { HighScores.Read(reader); } catch { }
        }
        catch { }
    }
}