using System;

namespace LiveCaptionTranslator.Models
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
