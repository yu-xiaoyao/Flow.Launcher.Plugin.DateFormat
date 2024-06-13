using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;

namespace Flow.Launcher.Plugin.DateFormat;

public class Main_Test
{
    public static void Main()
    {
        
        
        var now = DateTime.UtcNow;
        DateTime unixStartUTC = new(1970, 1, 1, 0, 0, 0, 0);
        DateTime unixStart = TimeZoneInfo.ConvertTimeFromUtc(unixStartUTC, TimeZoneInfo.Local);

        Console.WriteLine(unixStart);
        Console.WriteLine(now);
        var ts = now - unixStart;

        Console.WriteLine(TimeZoneInfo.Local);
        

        
        
        Console.WriteLine(ts.TotalMilliseconds);

        // test_2();
        // test_query_today();
        // test_query_datetime();
    }

    static void test_2()
    {
        List<string> list = new List<string>();
        list.Add("ms");
        list.Add("second");
        list.Add("hour");
        list.Add("minute");

        list.RemoveAll(p => !p.StartsWith("m"));

        foreach (var se in list)
        {
            Console.WriteLine(se);
        }
    }

    static void test_1()
    {
        var template = "{0} {1} = {2} {3}";
        var f = string.Format(template, "12345", "年", 22, 1234565432);
        Console.WriteLine(f);

        DateTime UnixStart = new(1970, 1, 1, 0, 0, 0, 0);
        var ts = DateTime.MaxValue - UnixStart;
        Console.WriteLine("TotalMilliseconds:" + ts.TotalMilliseconds);
        Console.WriteLine("TotalSeconds:" + ts.TotalSeconds);
        Console.WriteLine("TotalMilliseconds:" + ts.TotalMilliseconds);

        var maxValue = DateTime.MaxValue;
        Console.WriteLine(maxValue.ToString("yyyy-MM-dd HH:mm:ss.fff"));
    }

    private static void test_query_today()
    {
        // TodayFormatter.GetDayTimes(1716493250979, "ms");
        var result = DateTimeFormatter.GetSecondUnitTimes(1, "day", null);
        foreach (var formatResult in result)
        {
            Console.WriteLine(formatResult);
        }
    }


    private static void test_query_datetime()
    {
        var dectectResult = DateTimeFormatter.FormatDateTime("2024-05-26 13:05:44");
        foreach (var formatResult in dectectResult)
        {
            Console.WriteLine(formatResult);
        }

        Console.WriteLine("________________________________");
        dectectResult = DateTimeFormatter.FormatDateTime("2024-05-26");
        foreach (var formatResult in dectectResult)
        {
            Console.WriteLine(formatResult);
        }

        Console.WriteLine("________________________________");
        dectectResult = DateTimeFormatter.FormatDateTime("13:05:44");
        foreach (var formatResult in dectectResult)
        {
            Console.WriteLine(formatResult);
        }

        Console.WriteLine("________________________________");
    }

    private static void test_format()
    {
        DateTimeFormatter.FormatTimeStampFormats(1716493250979);


        Print(DateTimeFormatter.FormatAndDectectDateTime("2024-05-26"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("20240526"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("2024/05/26"));

        Print(DateTimeFormatter.FormatAndDectectDateTime("13:05:44"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("130544"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("13/05/44"));

        Print(DateTimeFormatter.FormatAndDectectDateTime("2024-05-26 13:05:44"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("2024-05-26 13:05:44.123"));

        Print(DateTimeFormatter.FormatAndDectectDateTime("2024/05/26 13:05:44"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("2024/05/26 13:05:44.123"));

        Print(DateTimeFormatter.FormatAndDectectDateTime("2024/05/26 13/05/44"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("2024/05/26 13/05/44.123"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("2024/05/26 13/05/44/123"));

        Print(DateTimeFormatter.FormatAndDectectDateTime("20240526130544"));
        Print(DateTimeFormatter.FormatAndDectectDateTime("20240526130544123"));

        Print(DateTimeFormatter.FormatAndDectectDateTime("20240526 130544123"));
    }

    private static void Print(Tuple<DetectFormat, DateTime> result)
    {
        if (result == null)
        {
            Console.WriteLine("result is null");
        }
        else
        {
            Console.WriteLine($"{result.Item1} {result.Item2}");
        }
    }
}