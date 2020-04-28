using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Activities.Statements;
using System.ComponentModel;
using myoshidan.IBM.Watson.STT.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using myoshidan.IBM.Watson.STT.Models;
using NAudio.CoreAudioApi;
using System.Linq;
using myoshidan.IBM.Watson.STT.Enums;
using UiPath.Shared.Activities.Utilities;
using myoshidan.IBM.Watson.STT.Helper;
using System.Windows.Forms;

namespace myoshidan.IBM.Watson.STT.Activities
{
    /// <summary>
    /// SpeechToTextStreamingScope
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.SpeechToTextStreamingScope_DisplayName))]
    [LocalizedDescription(nameof(Resources.SpeechToTextStreamingScope_Description))]
    public class SpeechToTextStreamingScope : ContinuableAsyncNativeActivity
    {
        #region Properties

        [Browsable(false)]
        public ActivityAction<IObjectContainer​> Body { get; set; }

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.SpeechToTextStreamingScope_Region_DisplayName))]
        [LocalizedDescription(nameof(Resources.SpeechToTextStreamingScope_Region_Description))]
        [LocalizedCategory(nameof(Resources.Authentication_Category))]
        [TypeConverter(typeof(EnumNameConverter<Region>))]
        public Region Region { get; set; }

        [LocalizedDisplayName(nameof(Resources.SpeechToTextStreamingScope_APIKey_DisplayName))]
        [LocalizedDescription(nameof(Resources.SpeechToTextStreamingScope_APIKey_Description))]
        [LocalizedCategory(nameof(Resources.Authentication_Category))]
        public InArgument<string> APIKey { get; set; }

        [LocalizedDisplayName(nameof(Resources.SpeechToTextStreamingScope_Model_DisplayName))]
        [LocalizedDescription(nameof(Resources.SpeechToTextStreamingScope_Model_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        [TypeConverter(typeof(EnumNameConverter<AudioModel>))]
        public AudioModel Model { get; set; }

        // A tag used to identify the scope in the activity context
        internal static string ParentContainerPropertyTag => "ScopeActivity";

        // Object Container: Add strongly-typed objects here and they will be available in the scope's child activities.
        private readonly IObjectContainer _objectContainer;

        #endregion


        #region Constructors

        public SpeechToTextStreamingScope(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;

            Body = new ActivityAction<IObjectContainer>
            {
                Argument = new DelegateInArgument<IObjectContainer> (ParentContainerPropertyTag),
                Handler = new Sequence { DisplayName = Resources.Do }
            };
        }

        public SpeechToTextStreamingScope() : this(new ObjectContainer())
        {

        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (APIKey == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(APIKey)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<NativeActivityContext>> ExecuteAsync(NativeActivityContext  context, CancellationToken cancellationToken)
        {
            // Inputs
            var apikey = APIKey.Get(context);
                        
            var token = await IBMWatsonGetAccessToken.GetAccessToken(apikey);
            var region = RegionConvertHelper.GetRegionUrl(this.Region);
            var model = AudioModelConvertHelper.GetAudioModelName(this.Model);
            var service = new IBMWatsonSpeechToTextWebsocketService(region, token, model);

            var collection = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            MMDevice device = collection.FirstOrDefault();
            if (collection.Count() >= 2)
            {
                device = MMDeviceSelectDialog.ShowDialog("Select Audio Device",collection);
            }
            var recoder = new AudioMemoryRecorder(device);
            recoder.AudioMemoryWaveIn += Recoder_AudioMemoryWaveIn;

            _objectContainer.Add(service);
            _objectContainer.Add(recoder);

            return (ctx) => {
                // Schedule child activities
                if (Body != null)
				    ctx.ScheduleAction<IObjectContainer>(Body, _objectContainer, OnCompleted, OnFaulted);

                // Outputs
            };
        }

        private void Recoder_AudioMemoryWaveIn(object sender, AudioMemoryWaveInEventArgs e)
        {
            var service = _objectContainer.Get<IBMWatsonSpeechToTextWebsocketService>();
            service.SendAudioToWatson(e.Buffer).Wait();

            if (_objectContainer.Contains<Form>() && _objectContainer.Get<Form>().TopMost)
            {
                _objectContainer.Get<Form>().TopMost = true; ;
            }

            var transcriptText = service.Transcipt;
            if (string.IsNullOrEmpty(service.Transcipt)) return;

            if (_objectContainer.Contains<IBMWatsonLanguageTranslatorService>())
            {
                var translator = _objectContainer.Get<IBMWatsonLanguageTranslatorService>();
                if((DateTime.Now - translator.LastUpdate).TotalSeconds > 2)
                {
                    transcriptText = translator.Translate(transcriptText);
                }
                else
                {
                    return;
                }
            }

            
            if (_objectContainer.Contains<TranscriptFileWriter>())
            {
                var writer = _objectContainer.Get<TranscriptFileWriter>();
                if (service.ResultIndex != writer.Transcripts.Count -1)
                {
                    writer.Transcripts.Add(transcriptText);
                }
                else
                {
                    writer.Transcripts[writer.Transcripts.Count-1] = transcriptText;
                }
                writer.UpdateTranscriptFile();
            }

            if (!string.IsNullOrEmpty(transcriptText))
            {
                var displayText = transcriptText;
                if(displayText.Length > 250)
                {
                    displayText = displayText.Substring(displayText.Length - 250, 250);
                }
                var label = _objectContainer.Get<Label>();
                label.Invoke((MethodInvoker)(() => label.Text = displayText));
            }
        }

        #endregion


        #region Events

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            faultContext.CancelChildren();
            Cleanup();
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            Cleanup();
        }

        #endregion


        #region Helpers
        
        private void Cleanup()
        {
            var disposableObjects = _objectContainer.Where(o => o is IDisposable);
            foreach (var obj in disposableObjects)
            {
                if (obj is IDisposable dispObject)
                    if(obj is Control)
                    {
                        (obj as Control).Invoke((MethodInvoker)(() => dispObject.Dispose()));
                    }
                    else
                    {
                        dispObject.Dispose();
                    }
            }
            _objectContainer.Clear();
        }

        #endregion
    }
}

