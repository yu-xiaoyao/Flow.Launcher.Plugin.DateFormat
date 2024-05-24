using System;
using System.Collections.Generic;
using System.Globalization;

namespace Flow.Launcher.Plugin.DateFormat;

public class DateTimeFormatter
{
    public static string[] DateFormats =
    {
        "yyyy-MM-dd",
        "yyyy/MM/dd",
        "yyyyMMdd"
    };

    public static string[] TimeFormats =
    {
        "HH:mm:ss",
        "HHmmss",
        "HH/mm/ss"
    };

    public static string[] DateTimeFormats =
    {
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss.fff",


        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss.fffZ",

        "yyyy/MM/dd HH:mm:ss",
        "yyyy/MM/dd HH:mm:ss.fff",

        "yyyy/MM/ddTHH:mm:ssZ",
        "yyyy/MM/ddTHH:mm:ss.fffZ",

        "yyyy/MM/dd HH/mm/ss",
        "yyyy/MM/dd HH/mm/ss.fff",
        "yyyy/MM/dd HH/mm/ss/fff",

        "yyyyMMddHHmmss",
        "yyyyMMdd HHmmss",
        "yyyyMMddHHmmssfff",
        "yyyyMMdd HHmmssfff",
    };

    private static readonly DateTime UnixStart = new(1970, 1, 1, 0, 0, 0, 0);


    public static Tuple<DetectFormat, DateTime> FormatAndDectectDateTime(string input)
    {
        var result = DateTime.TryParseExact(input, DateTimeFormats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var dateTime);
        if (result)
        {
            return new Tuple<DetectFormat, DateTime>(DetectFormat.DateTime, dateTime);
        }

        result = DateTime.TryParseExact(input, DateFormats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var date);
        if (result)
        {
            return new Tuple<DetectFormat, DateTime>(DetectFormat.Date, date);
        }

        result = DateTime.TryParseExact(input, TimeFormats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var time);
        if (result)
        {
            return new Tuple<DetectFormat, DateTime>(DetectFormat.Time, time);
        }

        return null;
    }


    public static List<FormatResult> FormatDateTime(string input)
    {
        var result = DateTime.TryParseExact(input, DateTimeFormats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var dateTime);
        if (result)
        {
            return GetDateTimeFormatResults(dateTime);
        }

        result = DateTime.TryParseExact(input, DateFormats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var date);
        if (result)
        {
            return GetDateFormatResults(date);
        }

        result = DateTime.TryParseExact(input, TimeFormats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var time);
        if (result)
        {
            return GetTimeFormatResults(time);
        }

        return null;
    }


    /// <summary>
    /// 输入为 日期 Date
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static List<FormatResult> GetDateFormatResults(DateTime dateTime)
    {
        var ts = dateTime - UnixStart;
        return new List<FormatResult>()
        {
            new(I18nKey.FormatDateTitle, dateTime.ToString("yyyy-MM-dd")),
            new(I18nKey.TimestampTitle, $"{ts.TotalMilliseconds}"),
            new(I18nKey.TimestampSecondTitle, $"{ts.TotalSeconds}"),
            new(I18nKey.TimestampDayTitle, $"{ts.TotalDays}")
        };
    }

    /// <summary>
    /// 输入为 时间 Time
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static List<FormatResult> GetTimeFormatResults(DateTime dateTime)
    {
        //  var startDay = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        return new List<FormatResult>()
        {
            new(I18nKey.FormatTimeTitle, dateTime.ToString("HH:mm:ss")),
            new(I18nKey.FormatTimeMillsTitle, dateTime.ToString("HH:mm:ss.fff")),
            new(I18nKey.FormatTimeTitle, dateTime.ToString("yyyy-MM-dd HH:mm:ss")),
            new(I18nKey.FormatISODateMillsTitle, dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
        };
    }

    /// <summary>
    /// 输入为  DateTime
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static List<FormatResult> GetDateTimeFormatResults(DateTime dateTime)
    {
        var ts = dateTime - UnixStart;
        return new List<FormatResult>()
        {
            new(I18nKey.TimestampTitle, $"{ts.TotalMilliseconds}"),
            new(I18nKey.FormatDateTimeTitle, dateTime.ToString("yyyy-MM-dd HH:mm:ss")),
            new(I18nKey.FormatDateTimeMillsTitle, dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")),
            new(I18nKey.FormatISODateMillsTitle, dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")),
            new(I18nKey.FormatDateTitle, dateTime.ToString("yyyy-MM-dd")),
            new(I18nKey.FormatDateTitle, dateTime.ToString("HH:mm:ss.fff")),
            new(I18nKey.FormatDateTitle, dateTime.ToString("HH:mm:ss")),
            new(I18nKey.TimestampSecondTitle, $"{ts.TotalSeconds}"),
        };
    }


    /// <summary>
    /// 格式化时间戳(毫秒)
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static List<FormatResult> FormatTimeStampFormats(long timestamp)
    {
        // 这里有范围..

        if (timestamp > 253402300800000)
        {
            return null;
        }

        var dateTime = UnixStart.AddMilliseconds(timestamp);
        return new List<FormatResult>
        {
            new(I18nKey.FormatDateTimeTitle, dateTime.ToString("yyyy-MM-dd HH:mm:ss")),
            new(I18nKey.FormatDateTimeMillsTitle, dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")),
            new(I18nKey.FormatISODateMillsTitle, dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")),
            new(I18nKey.FormatISODateTitle, dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")),
            new(I18nKey.FormatDateTitle, dateTime.ToString("yyyy-MM-dd")),
            new(I18nKey.FormatTimeMillsTitle, dateTime.ToString("HH:mm:ss.fff")),
            new(I18nKey.FormatTimeTitle, dateTime.ToString("HH:mm:ss"))
        };
    }

    public static List<FormatResult> GetSecondUnitTimes(long time, string inputUnit, Func<string, string> transFunc)
    {
        var template = "{0} {1} = {2} {3}";
        if ("d".Equals(inputUnit, StringComparison.OrdinalIgnoreCase) ||
            "day".Equals(inputUnit, StringComparison.OrdinalIgnoreCase) ||
            "days".Equals(inputUnit, StringComparison.OrdinalIgnoreCase))
        {
            var hour = time * 24;
            var minute = hour * 60;
            var seconds = minute * 60;
            var mills = seconds * 1000;
            var currentUnitName = transFunc.Invoke(I18nKey.UnitDayTitle);

            return new List<FormatResult>
            {
                new(string.Format(template, time, currentUnitName, seconds, transFunc.Invoke(I18nKey.UnitSecondTitle)),
                    $"{seconds}", false),
                new(string.Format(template, time, currentUnitName, minute, transFunc.Invoke(I18nKey.TodayMinuteTitle)),
                    $"{minute}", false),
                new(
                    string.Format(template, time, currentUnitName, mills,
                        transFunc.Invoke(I18nKey.TodayMillsSecondTitle)), $"{mills}", false),
                new(string.Format(template, time, currentUnitName, hour, transFunc.Invoke(I18nKey.TodayHourTitle)),
                    $"{hour}", false),
            };
        }

        if ("h".Equals(inputUnit, StringComparison.OrdinalIgnoreCase) ||
            "hour".Equals(inputUnit, StringComparison.OrdinalIgnoreCase) ||
            "hours".Equals(inputUnit, StringComparison.OrdinalIgnoreCase))
        {
            var minute = time * 60;
            var seconds = minute * 60;
            var mills = seconds * 1000;

            var currentUnitName = transFunc.Invoke(I18nKey.UnitHourTitle);
            return new List<FormatResult>
            {
                new(string.Format(template, time, currentUnitName, seconds, transFunc.Invoke(I18nKey.UnitSecondTitle)),
                    $"{seconds}", false),
                new(string.Format(template, time, currentUnitName, minute, transFunc.Invoke(I18nKey.TodayMinuteTitle)),
                    $"{minute}", false),
                new(
                    string.Format(template, time, currentUnitName, mills,
                        transFunc.Invoke(I18nKey.TodayMillsSecondTitle)), $"{mills}", false),
            };
        }

        if ("m".Equals(inputUnit) ||
            "minute".Equals(inputUnit, StringComparison.OrdinalIgnoreCase) ||
            "minutes".Equals(inputUnit, StringComparison.OrdinalIgnoreCase))
        {
            var seconds = time * 60;
            var mills = seconds * 1000;

            var currentUnitName = transFunc.Invoke(I18nKey.UnitMinuteTitle);
            return new List<FormatResult>
            {
                new(string.Format(template, time, currentUnitName, seconds, transFunc.Invoke(I18nKey.UnitSecondTitle)),
                    $"{seconds}", false),
                new(
                    string.Format(template, time, currentUnitName, mills,
                        transFunc.Invoke(I18nKey.TodayMillsSecondTitle)), $"{mills}", false),
            };
        }

        if ("s".Equals(inputUnit) ||
            "second".Equals(inputUnit, StringComparison.OrdinalIgnoreCase) ||
            "seconds".Equals(inputUnit, StringComparison.OrdinalIgnoreCase))
        {
            return FormatTimeStampFormats(time * 1000);
        }

        if ("ms".Equals(inputUnit, StringComparison.OrdinalIgnoreCase) ||
            "mills".Equals(inputUnit, StringComparison.OrdinalIgnoreCase))
        {
            return FormatTimeStampFormats(time);
        }

        return new List<FormatResult>();
    }

    public static long GetNowTimestamp()
    {
        TimeSpan ts = DateTime.Now - UnixStart;
        return Convert.ToInt64(ts.TotalMilliseconds);
    }
}