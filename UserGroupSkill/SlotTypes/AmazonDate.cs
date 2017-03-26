using System;
using System.Text.RegularExpressions;

namespace UserGroupSkill.SlotTypes
{
    class AmazonDate
    {
        public enum Indicator
        {
            Unknown,
            Now,
            Day,
            Week,
            Weekend,
            Month,
            Year,
            Decade,
            Season
        }

        public readonly DateTime Date;
        public readonly Indicator Type;

        public AmazonDate(string text)
        {
            if (text == "PRESENT_REF")
            {
                Type = Indicator.Now;
                Date = DateTime.Now;
                return;
            }

            var asMonth = new Regex("^([0-9]{4})-([0-9]{2})$");
            var match = asMonth.Match(text);
            if (match.Success)
            {
                Type = Indicator.Month;
                Date = new DateTime(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), 1);
                return;
            }

            var asDay = new Regex("^([0-9]{4})-([0-9]{2})-([0-9]{2})$");
            match = asDay.Match(text);
            if (match.Success)
            {
                Type = Indicator.Day;
                Date = new DateTime(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
                return;
            }

            /*
“this week”: 2015-W48
“next week”: 2015-W49
“this weekend”: 2015-W48-WE
“next year”: 2016
“this decade”: 201X
“next winter”: 2017-WI
    */
            Type = Indicator.Unknown;
            Date = DateTime.Now;
        }
    }
}
