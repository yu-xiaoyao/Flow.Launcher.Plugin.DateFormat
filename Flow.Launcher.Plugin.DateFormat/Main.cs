using System;
using System.Collections.Generic;
using System.Linq;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.DateFormat
{
    public class DateFormat : IPlugin, IPluginI18n
    {
        public const string IconPath = @"Images\\date-format.png";

        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            var search = query.Search.Trim();
            if (search.Length == 0)
                return BuildEmptyContentResult();

            _context.API.LogInfo("DF", query.RawQuery);

            var firstSearch = query.FirstSearch.Trim();
            if (Util.IsLongNumber(firstSearch, out var timeLong))
            {
                var unitResultList = GetCompleteUnits(query.ActionKeyword, timeLong);
                List<Result> resultList;
                var secondSearch = query.SecondSearch.Trim();
                if (string.IsNullOrEmpty(secondSearch))
                {
                    if (timeLong < 86400)
                        return unitResultList;

                    // 格式化时间戳(MS)
                    resultList = BuildResultFromFormat(query.ActionKeyword,
                        DateTimeFormatter.FormatTimeStampFormats(timeLong));

                    unitResultList.RemoveAll(p => p.Title.Equals("ms", StringComparison.OrdinalIgnoreCase));
                    resultList.AddRange(unitResultList);
                }
                else
                {
                    // 显示一天内有多个时间
                    resultList = BuildResultFromFormat(query.ActionKeyword,
                        DateTimeFormatter.GetSecondUnitTimes(timeLong,
                            secondSearch,
                            key => _context.API.GetTranslation(key)));

                    var findEq = unitResultList.Any(result =>
                        secondSearch.Equals(result.Title, StringComparison.OrdinalIgnoreCase));
                    if (!findEq)
                    {
                        unitResultList.RemoveAll(p => !p.Title.StartsWith(secondSearch));
                        resultList.AddRange(unitResultList);
                    }
                }

                return resultList;
            }

            // 输入为时间格式.
            var formatResults = DateTimeFormatter.FormatDateTime(search);
            if (formatResults == null)
            {
                return new List<Result>();
            }

            return BuildResultFromFormat(query.ActionKeyword, formatResults);
        }


        private List<Result> BuildResultFromFormat(string actionKeyword, List<FormatResult> formatResults)
        {
            if (formatResults == null)
            {
                return new List<Result>();
            }

            return formatResults.Select(fr => new Result
            {
                IcoPath = IconPath,
                Title = fr.formatResult,
                CopyText = fr.formatResult,
                SubTitle = fr.transTitle ? _context.API.GetTranslation(fr.title) : fr.title,
                AutoCompleteText = $"{actionKeyword} {fr.formatResult}",
                Action = _ =>
                {
                    _context.API.CopyToClipboard(fr.formatResult, false, false);
                    return true;
                }
            }).ToList();
        }

        private List<Result> BuildEmptyContentResult()
        {
            var list = new List<Result>();

            var nowTimestamp = DateTimeFormatter.GetNowTimestamp();
            list.Add(new Result
            {
                IcoPath = IconPath,
                Title = $"{nowTimestamp}",
                SubTitle = _context.API.GetTranslation(I18nKey.CurrentTimestamp),
                CopyText = $"{nowTimestamp}",
                Action = _ =>
                {
                    _context.API.CopyToClipboard(nowTimestamp.ToString(), false, false);
                    return true;
                }
            });
            var nowSecond = nowTimestamp / 1000;
            list.Add(new Result
            {
                IcoPath = IconPath,
                Title = $"{nowSecond}",
                SubTitle = _context.API.GetTranslation(I18nKey.CurrentTimestampSecond),
                CopyText = $"{nowSecond}",
                Action = _ =>
                {
                    _context.API.CopyToClipboard(nowSecond.ToString(), false, false);
                    return true;
                }
            });
            return list;
        }

        private List<Result> GetCompleteUnits(string actionKeyword, long timeLong)
        {
            return new List<Result>
            {
                new()
                {
                    IcoPath = IconPath,
                    Title = "ms",
                    SubTitle = _context.API.GetTranslation(I18nKey.UnitMillSecondTitle),
                    AutoCompleteText = $"{actionKeyword} {timeLong} ms",
                    Score = -1,
                    Action = _ =>
                    {
                        _context.API.ChangeQuery($"{actionKeyword} {timeLong} ms", true);
                        return false;
                    }
                },
                new()
                {
                    IcoPath = IconPath,
                    Title = "second",
                    SubTitle = _context.API.GetTranslation(I18nKey.UnitSecondTitle),
                    AutoCompleteText = $"{actionKeyword} {timeLong} second",
                    Score = -1,
                    Action = _ =>
                    {
                        _context.API.ChangeQuery($"{actionKeyword} {timeLong} second", true);
                        return false;
                    }
                },
                new()
                {
                    IcoPath = IconPath,
                    Title = "minute",
                    SubTitle = _context.API.GetTranslation(I18nKey.UnitMinuteTitle),
                    AutoCompleteText = $"{actionKeyword} {timeLong} minute",
                    Score = -1,
                    Action = _ =>
                    {
                        _context.API.ChangeQuery($"{actionKeyword} {timeLong} minute", true);
                        return false;
                    }
                },
                new()
                {
                    IcoPath = IconPath,
                    Title = "hour",
                    SubTitle = _context.API.GetTranslation(I18nKey.UnitHourTitle),
                    AutoCompleteText = $"{actionKeyword} {timeLong} hour",
                    Score = -1,
                    Action = _ =>
                    {
                        _context.API.ChangeQuery($"{actionKeyword} {timeLong} hour", true);
                        return false;
                    }
                },
                new()
                {
                    IcoPath = IconPath,
                    Title = "day",
                    SubTitle = _context.API.GetTranslation(I18nKey.UnitDayTitle),
                    AutoCompleteText = $"{actionKeyword} {timeLong} day",
                    Score = -1,
                    Action = _ =>
                    {
                        _context.API.ChangeQuery($"{actionKeyword} {timeLong} day", true);
                        return false;
                    }
                }
            };
        }

        public string GetTranslatedPluginTitle()
        {
            return _context.API.GetTranslation("date_format_plugin_title");
        }

        public string GetTranslatedPluginDescription()
        {
            return _context.API.GetTranslation("date_format_plugin_desp");
        }
    }
}