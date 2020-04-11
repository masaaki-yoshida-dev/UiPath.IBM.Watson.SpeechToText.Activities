using IBM.Cloud.SDK.Core.Authentication;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.SpeechToText.v1;
using IBM.Watson.SpeechToText.v1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myoshidan.IBM.Watson.STT.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class IBMWatsonSpeechToTextService
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
        #endregion

        #region Private Fields
        private Authenticator _authenticator;
        private SpeechToTextService _speechToTextService;
        #endregion

        #region Constractor
        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="apikey"></param>
        /// <param name="uri"></param>
        public IBMWatsonSpeechToTextService(string apikey, string uri)
        {
            APIKey = apikey;
            URL = uri;
            _authenticator = new IamAuthenticator(apikey: this.APIKey);
            _speechToTextService = new SpeechToTextService(_authenticator);
            _speechToTextService.SetServiceUrl(this.URL);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Recognize Audio File
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<SpeechRecognitionResult> RecognizeAudioFile(string filePath)
        {
            if(_speechToTextService == null)
            {
                throw new NullReferenceException("SpeechToTextService is not Initialized");
            }

            FileStream fs = new FileStream(filePath, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            var result = _speechToTextService.Recognize(data);
            return result.Result.Results;
        }
        #endregion
    }
}
