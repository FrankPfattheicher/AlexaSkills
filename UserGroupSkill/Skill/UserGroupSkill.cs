using System;
using System.Diagnostics;
using System.Linq;
using UserGroupSkill.Alexa;
using UserGroupSkill.SlotTypes;

namespace UserGroupSkill.Skill
{
    public class UserGroupSkill
    {
        public SkillResponse Response = new SkillResponse
        {
            OutputSpeechSsml = "<speak><p>Oh!</p><p>Es tut mir Leid, DAAS weiß ich leider nicht.</p></speak>"
        };

        public void HandleRequest(SkillRequest request)
        {
            var eventResponse = new[] { "Treffen", "das" };
            var rqEvent = request.Slots
                .FirstOrDefault(slot => string.Compare(slot.Key, "Event", StringComparison.InvariantCultureIgnoreCase) == 0)
                .Value.ToLower();
            if (rqEvent.Contains("in"))
            {
                eventResponse = new[] { "Termin", "der" };
            }
            else if (rqEvent.Contains("event"))
            {
                eventResponse = new[] { "Event", "das" };
            }
            else if (rqEvent.Contains("meet") || rqEvent.Contains("ieder"))
            {
                eventResponse = new[] { "Meet-up", "das" };
            }
            Trace.TraceInformation($"Event({rqEvent}) => {eventResponse[1]} {eventResponse[0]}");

            switch (request.Intent)
            {
                case "UG_Current":
                    HandleCurrent(request, eventResponse);
                    break;
                case "UG_Month":
                    HandleMonth(request, eventResponse);
                    break;
                case "UG_Date":
                    HandleSpecificDate(request, eventResponse);
                    break;
            }
        }


        private void HandleCurrent(SkillRequest request, string[] eventResponse)
        {
            Response.OutputSpeechSsml =
                $"<speak>{eventResponse[1]} nächste {eventResponse[0]} findet am 18. April statt. Das Thema ist U.W.P.</speak>";
        }

        private void HandleMonth(SkillRequest request, string[] eventResponse)
        {
            var data = request.Slots;
            if (data.ContainsKey("Month"))
            {
                var date = new AmazonDate(data["Month"]).Date;
                if (date < DateTime.Now)
                {
                    Response.OutputSpeechSsml = $"<speak>Im {date:MMMM} fand kein {eventResponse[0]} statt.</speak>";
                }
                else if (date.Month != DateTime.Now.Month)
                {
                    Response.OutputSpeechSsml = $"<speak>Im {date:MMMM} findet kein {eventResponse[0]} mehr statt.</speak>";
                }
                else
                {
                    Response.OutputSpeechSsml = $"<speak>Diesen Monat findet kein {eventResponse[0]} mehr statt.</speak>";
                }
            }
        }

        private void HandleSpecificDate(SkillRequest request, string[] eventResponse)
        {
            var data = request.Slots;
            if (data.ContainsKey("Date"))
            {
                var date = new AmazonDate(data["Date"]).Date;
                Response.OutputSpeechSsml =
                    $"<speak>Am {date} findet leider kein {eventResponse[0]} statt.</speak>";
            }
        }

    }
}