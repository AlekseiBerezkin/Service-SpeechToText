using Google.Cloud.Speech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SpeechToText.Provider
{
    public class RequestProvder
    {
        const int MAX_TEXT_LENGTH = 50;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        string typeRequest { get; set; }
        public RequestProvder(string type)
        {
            typeRequest = type;
        }
        public async Task<int> RecognizeTheCity(string PathToFile, string Language)
        {
            return await Recognize(PathToFile, Language, Properties.Resources.SelectCodeFromCityDictionary);
        }

        public async Task<int> RecognizeTheYesNo(string PathToFile, string Language)
        {
            return await Recognize(PathToFile, Language, Properties.Resources.SelectCodeFromYesNoDictionary);
        }

        private async Task<int> Recognize (string PathToFile, string Language, string sql)
        {
            try
            {
                logger.Info($"Request: " + "File:" + PathToFile + ".Language:" + Language);

                var speech = SpeechClient.Create();
                var response = speech.Recognize(new RecognitionConfig()
                {
                    LanguageCode = Language
                }, RecognitionAudio.FromFile(PathToFile));
                var sttResults = response.Results.SelectMany(x => x.Alternatives).Select(x => x.Transcript).ToArray();

                int iRes = -1;

                if (sttResults != null && sttResults.Length > 0)
                {

                    string text = sttResults[0];
                    if (text.Length > MAX_TEXT_LENGTH)
                        text = text.Substring(0, MAX_TEXT_LENGTH);

                    using (SQLProvider provider = new SQLProvider())
                    {
                        iRes = await provider.ExecuteScalarAsync(sql, text);

                        if (iRes == -1)
                            await provider.ExecuteNonScalarAsync(PathToFile, text, typeRequest);

                    }
                }
                logger.Info($"Result: {iRes} dictionary '{typeRequest}'");
                return iRes;
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }
    }
}