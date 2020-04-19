using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using myoshidan.IBM.Watson.STT.Activities.Properties;
using myoshidan.IBM.Watson.STT.Models;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace myoshidan.IBM.Watson.STT.Activities
{
    [LocalizedDisplayName(nameof(Resources.ReadStreamingRecognize_DisplayName))]
    [LocalizedDescription(nameof(Resources.ReadStreamingRecognize_Description))]
    public class ReadStreamingRecognize : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.ReadStreamingRecognize_Transcript_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReadStreamingRecognize_Transcript_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Transcript { get; set; }

        #endregion


        #region Constructors

        public ReadStreamingRecognize()
        {
            Constraints.Add(ActivityConstraints.HasParentType<ReadStreamingRecognize, SpeechToTextStreamingScope>(string.Format(Resources.ValidationScope_Error, Resources.SpeechToTextStreamingScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

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
                Transcript.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SpeechToTextStreamingScope.ParentContainerPropertyTag);

            var service = objectContainer.Get<IBMWatsonSpeechToTextWebsocketService>();
            return await Task.FromResult(service.Transcipt);
        }

        #endregion
    }
}

