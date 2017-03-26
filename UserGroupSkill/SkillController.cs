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
        [Route("skill/usergroup")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] JObject json)
        {
            var alexaRequest = new SkillRequest(json);
            Trace.TraceInformation(alexaRequest.Json);

            var skill = new Skill.UserGroupSkill();
            skill.HandleRequest(alexaRequest);
            var skillResponse = skill.Response;
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
