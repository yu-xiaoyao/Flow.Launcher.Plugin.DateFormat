namespace Flow.Launcher.Plugin.DateFormat;

public class Util
{
    public static bool IsLongNumber(string text, out long result)
    {
        return long.TryParse(text, out result);
    }
}