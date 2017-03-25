using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;
using UserGroupSkill.Alexa;

namespace UserGroupSkill
{
    public class SkillController : ApiController
    {
        [Route("alexa")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] JObject json)
        {
            var alexaRequest = new SkillRequest(json);
            var skillResponse = new SkillResponse();

            Trace.TraceInformation(alexaRequest.Json);

            if (alexaRequest.Intent == "UserGroupIntent")
            {
                var data = alexaRequest.Slots;

                if (data.ContainsKey("Monat"))
                {
                    var fields = new Regex("([0-9]{4})-([0-9]{2})").Match(data["Monat"]);
                    if (fields.Success)
                    {
                        var date = new DateTime(int.Parse(fields.Groups[1].Value), int.Parse(fields.Groups[2].Value), 1);
                        skillResponse.OutputSpeechSsml = $"<speak>Im {date:MMMM} findet kein Treffen mehr statt.</speak>";
                    }
                    else
                    {
                        skillResponse.OutputSpeechSsml = "<speak>Diesen Monat findet kein Treffen mehr statt.</speak>";
                    }
                }
                else if(data.Count > 0)
                {
                    skillResponse.OutputSpeechSsml = $"<speak>Du fragst nach einem {data.Keys.First()}. Das weiß ich leider nicht.</speak>";
                }
                else
                {
                    skillResponse.OutputSpeechSsml = "<speak>Der nächste Termin findet am 18. April statt. Das Thema ist U.W.P.</speak>";
                }
            }
            else
            {
                skillResponse.OutputSpeechSsml = "<speak>Hallo?</speak>";
            }

            Trace.TraceInformation(skillResponse.Json);

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(skillResponse.Json, Encoding.UTF8, "application/json")
            };

            var result = new ResponseMessageResult(httpResponse);
            return await Task.FromResult(result);
        }
    }
}
