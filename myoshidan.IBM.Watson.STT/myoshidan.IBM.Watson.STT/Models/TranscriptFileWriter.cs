using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myoshidan.IBM.Watson.STT.Models
{
    public class TranscriptFileWriter
    {
        #region Properties

        /// <summary>
        /// Export Text FilePath
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Transcript List
        /// </summary>
        public List<string> Transcripts { get; set; }

        #endregion


        public TranscriptFileWriter(string filePath)
        {
            this.FilePath = filePath;
            this.Transcripts = new List<string>();
        }

        public void UpdateTranscriptFile()
        {
            var text = string.Join(Environment.NewLine, Transcripts);
            File.WriteAllText(this.FilePath, text);
        }
    }
}
