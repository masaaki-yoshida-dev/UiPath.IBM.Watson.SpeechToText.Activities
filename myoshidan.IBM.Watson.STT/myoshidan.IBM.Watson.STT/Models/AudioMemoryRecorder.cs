using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myoshidan.IBM.Watson.STT.Models
{
    public class AudioMemoryRecorder : IDisposable
    {

        #region イベント
        public event EventHandler<AudioMemoryWaveInEventArgs> AudioMemoryWaveIn;
        #endregion

        #region フィールド

        private WasapiLoopbackCapture _WaveIn;
        private Stream _Stream;
        private WaveFileWriter _WaveFileWriter;
        private int lastPos;
        #endregion

        #region コンストラクタ

        public AudioMemoryRecorder(MMDevice device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            this._WaveIn = new WasapiLoopbackCapture(device);
            this._WaveIn.DataAvailable += this.WaveInOnDataAvailable;
            this._WaveIn.RecordingStopped += this.WaveInOnRecordingStopped;
            this._Stream = new MemoryStream();
            this._WaveFileWriter = new WaveFileWriter(this._Stream, new WaveFormat(16000, 2));
            this.lastPos = 0;
        }

        #endregion

        #region プロパティ

        public bool IsRecording
        {
            get;
            private set;
        }

        #endregion

        #region メソッド

        public void Start()
        {
            this.IsRecording = true;
            this._WaveIn.StartRecording();
        }

        public void Stop()
        {
            this.IsRecording = false;
            this._WaveIn.StopRecording();
        }

        #region オーバーライド
        #endregion

        #region イベントハンドラ

        private void WaveInOnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (this._Stream != null)
            {
                this._Stream.Close();
                this._Stream = null;
            }
            this.Dispose();
        }

        private void WaveInOnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded == 0) return;

            var convertedBytes = convert16(e.Buffer, e.BytesRecorded, _WaveIn.WaveFormat);
            this._WaveFileWriter.Write(convertedBytes, 0, convertedBytes.Length);
            var seekPos = (int)_Stream.Position;
            byte[] bytes = new byte[seekPos - lastPos];
            _Stream.Position = lastPos;
            _Stream.Read(bytes, 0, bytes.Length);
            _Stream.Position = seekPos;
            this.lastPos = seekPos;
            this.AudioMemoryWaveIn?.Invoke(this, new AudioMemoryWaveInEventArgs(bytes, bytes.Length));
        }

        #endregion

        #region ヘルパーメソッド
        private byte[] convert16(byte[] input, int length, WaveFormat format)
        {
            if (length == 0)
                return new byte[0];
            using (var memStream = new MemoryStream(input, 0, length))
            {
                using (var inputStream = new RawSourceWaveStream(memStream, format))
                {
                    var sampleStream = new WaveToSampleProvider(inputStream);
                    var resamplingProvider = new WdlResamplingSampleProvider(sampleStream, 16000);
                    var ieeeToPCM = new SampleToWaveProvider16(resamplingProvider);
                    //var sampleStreams = new StereoToMonoProvider16(ieeeToPCM);
                    //sampleStreams.RightVolume = 0.5f;
                    //sampleStreams.LeftVolume = 0.5f;
                    return readStream(ieeeToPCM, length);
                }
            }
        }

        private byte[] readStream(IWaveProvider waveStream, int length)
        {
            byte[] buffer = new byte[length];
            using (var stream = new MemoryStream())
            {
                int read;
                while ((read = waveStream.Read(buffer, 0, length)) > 0)
                {
                    stream.Write(buffer, 0, read);
                }
                return stream.ToArray();
            }
        }

        #endregion

        #endregion

        #region IDisposable メンバー

        public void Dispose()
        {
            this._WaveIn.DataAvailable -= this.WaveInOnDataAvailable;
            this._WaveIn.RecordingStopped -= this.WaveInOnRecordingStopped;
            this._WaveIn?.Dispose();
            this._Stream?.Dispose();
        }

        #endregion

    }
}
