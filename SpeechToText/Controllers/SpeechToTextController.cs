using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using SpeechToText.Provider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SpeechToText.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/yesno")]
        [HttpGet]
        public async Task<int> YesNoSpeechToText(string PathToFile, string Language)
        {
            RequestProvder rp = new RequestProvder("YesNo");
            return await rp.RecognizeTheYesNo(PathToFile,Language);
        }

        [Route("api/City")]
        [HttpGet]
        
        public async Task<int> CitySpeechToText(string PathToFile, string Language)
        {
            RequestProvder rp = new RequestProvder("City");
            return await rp.RecognizeTheCity(PathToFile, Language);
        }
    }
}
