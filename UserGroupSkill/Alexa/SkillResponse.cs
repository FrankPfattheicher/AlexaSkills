using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UserGroupSkill.Alexa
{
    public class SkillResponse
    {
        private readonly JObject _response;
        private const string Template = @"
{
  'version': '1.0',
  'sessionAttributes': { },
  'response': {
    'outputSpeech': {
      'type': 'SSML',
      'ssml': '',
      'text': ''
    },
    'shouldEndSession': true
  }
}";

        public SkillResponse()
        {
            _response = JsonConvert.DeserializeObject<JObject>(Template);
        }

        public string Json => _response.ToString();

        public string OutputSpeechSsml
        {
            get { return Property("response/outputSpeech/ssml"); }
            set
            {
                Property("response/outputSpeech/ssml", value);
                Property("response/outputSpeech/type", "SSML");
            }
        }
        public string OutputSpeechText
        {
            get { return Property("response/outputSpeech/text"); }
            set
            {
                Property("response/outputSpeech/text", value);
                Property("response/outputSpeech/type", "PlainText");
            }
        }

        private string Property(string path, string value = null)
        {
            var parts = path.Split(new [] { "/" }, StringSplitOptions.None);
            var prop = _response as JToken;
            foreach (var part in parts)
            {
                prop = prop[part];
                if (prop == null) return null;
            }
            if ((prop != null) && (value != null))
            {
                prop.Parent.Parent[parts.Last()] = value;
            }
            return prop?.Value<string>();
        }
    }
}
