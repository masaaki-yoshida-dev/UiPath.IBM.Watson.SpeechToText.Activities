using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBM.Watson.SpeechToText.v1.Model;
using myoshidan.IBM.Watson.STT.Activities.Properties;
using myoshidan.IBM.Watson.STT.Enums;
using myoshidan.IBM.Watson.STT.Models;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace myoshidan.IBM.Watson.STT.Activities
{
    [LocalizedDisplayName(nameof(Resources.RecognizeAudioFile_DisplayName))]
    [LocalizedDescription(nameof(Resources.RecognizeAudioFile_Description))]
    public class RecognizeAudioFile : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.RecognizeAudioFile_FilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.RecognizeAudioFile_FilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.RecognizeAudioFile_Transcript_DisplayName))]
        [LocalizedDescription(nameof(Resources.RecognizeAudioFile_Transcript_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Transcript { get; set; }

        [LocalizedDisplayName(nameof(Resources.RecognizeAudioFile_AudioFormats_DisplayName))]
        [LocalizedDescription(nameof(Resources.RecognizeAudioFile_AudioFormats_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        [TypeConverter(typeof(EnumNameConverter<AudioFormat>))]
        public AudioFormat AudioFormats { get; set; }

        [LocalizedDisplayName(nameof(Resources.RecognizeAudioFile_Model_DisplayName))]
        [LocalizedDescription(nameof(Resources.RecognizeAudioFile_Model_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        [TypeConverter(typeof(EnumNameConverter<AudioModel>))]
        public AudioModel Model { get; set; }

        #endregion


        #region Constructors

        public RecognizeAudioFile()
        {
            Constraints.Add(ActivityConstraints.HasParentType<RecognizeAudioFile, SpeechToTextFileScope>(string.Format(Resources.ValidationScope_Error, Resources.SpeechToTextFileScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (FilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilePath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                Transcript.Set(ctx, task.Result.FirstOrDefault().Alternatives.FirstOrDefault().Transcript);
            };
        }

        private async Task<List<SpeechRecognitionResult>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SpeechToTextFileScope.ParentContainerPropertyTag);

            // Inputs
            var filepath = FilePath.Get(context);
            var service = objectContainer.Get<IBMWatsonSpeechToTextService>();

            //Outputs
            return await Task.FromResult(service.RecognizeAudioFile(filepath));
        }

        #endregion
    }
}

