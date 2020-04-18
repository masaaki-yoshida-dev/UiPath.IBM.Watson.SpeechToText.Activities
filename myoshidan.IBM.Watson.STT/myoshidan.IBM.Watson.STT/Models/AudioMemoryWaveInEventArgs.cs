using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myoshidan.IBM.Watson.STT.Models
{
    public class AudioMemoryWaveInEventArgs : EventArgs
    {
        public AudioMemoryWaveInEventArgs(byte[] buffer, int bytes)
        {
            this.Buffer = buffer;
            this.BytesRecorded = bytes;
        }

        public byte[] Buffer { get; }

        public int BytesRecorded { get; }
    }
}
