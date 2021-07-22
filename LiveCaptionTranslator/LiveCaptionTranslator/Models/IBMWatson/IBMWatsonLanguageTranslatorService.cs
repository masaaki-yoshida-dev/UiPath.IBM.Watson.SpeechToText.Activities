using IBM.Cloud.SDK.Core.Authentication;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.LanguageTranslator.v3;
using LiveCaptionTranslator.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveCaptionTranslator.Models.IBMWatson
{
    public class IBMWatsonLanguageTranslatorService
    {
        #region Properties
        /// <summary>
        /// APIKey
        /// </summary>
        public string APIKey { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// ModelId
        /// </summary>
        public string ModelId { get; set; }
        public DateTime LastUpdate { get; private set; }
        #endregion

        #region Private Fields
        private Authenticator _authenticator;
        private LanguageTranslatorService _languageTranslatorService;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apikey"></param>
        /// <param name="uri"></param>
        public IBMWatsonLanguageTranslatorService(string apikey, string uri)
        {
            APIKey = apikey;
            URL = uri;
            _authenticator = new IamAuthenticator(apikey: this.APIKey);
            _languageTranslatorService = new LanguageTranslatorService("2018-05-01", _authenticator);
            _languageTranslatorService.SetServiceUrl(this.URL);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Translate
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Translate(string text)
        {
            var result = _languageTranslatorService.Translate(new List<string>() { text }, ModelId);
            var responseObj = JsonConvert.DeserializeObject<LanguageTranslatorResponse>(result.Response);
            LastUpdate = DateTime.Now;
            return responseObj.translations.FirstOrDefault().translation;
        }
        #endregion


    }
}
