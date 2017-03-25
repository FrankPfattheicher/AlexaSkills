using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace UserGroupSkill.Alexa
{
    public class SkillRequest
    {
        private readonly JObject _request;

        public SkillRequest(JObject request)
        {
            _request = request;
        }

        public string Json => _request.ToString();

        public string Intent => Property("request/intent/name")?.Value<string>();

        public Dictionary<string, string> Slots
        {
            get
            {
                var slots = new Dictionary<string, string>();

                try
                {
                    var requestSlots = Property("request/intent/slots")?.Children()
                        .Select(slot => slot.Children().Children().Cast<JProperty>());

                    Func<JToken, bool> hasValue = token =>
                    {
                        var props = token.Children().Children()
                            .Cast<JProperty>();
                        return props.Any(p => p.Name == "value");
                    };
                    if (requestSlots == null) return slots;

                    slots = requestSlots
                        .Where(slot => slot.Any(p => p.Name == "value"))
                        .ToDictionary(props => props.First(p => p.Name == "name").Value.ToString(),
                                      props => props.First(p => p.Name == "value").Value.ToString());
                }
                catch
                {
                    // ignore - return empty dictionary
                }

                return slots;
            }
        }

        private JToken Property(string path)
        {
            var parts = path.Split(new [] { "/" }, StringSplitOptions.None);
            var prop = _request as JToken;
            foreach (var part in parts)
            {
                prop = prop[part];
                if (prop == null) return null;
            }
            return prop;
        }
    }
}
