namespace IOweYou.Helper;

public class EnvHelper
{
    public static void LoadFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Env file not found at " + path);

        foreach (var l in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(l))
                continue;
            
            if(l.StartsWith("#"))
                continue;

            var parts = l.Split('=', 2);
            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim();
            var value = parts[1].Trim();

            Environment.SetEnvironmentVariable(key, value);
        }
    }
}