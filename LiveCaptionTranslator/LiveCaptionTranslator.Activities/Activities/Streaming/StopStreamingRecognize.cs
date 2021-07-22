using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using LiveCaptionTranslator.Activities.Properties;
using LiveCaptionTranslator.Models;
using LiveCaptionTranslator.Models.IBMWatson;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace LiveCaptionTranslator.Activities
{
    [LocalizedDisplayName(nameof(Resources.StopStreamingRecognize_DisplayName))]
    [LocalizedDescription(nameof(Resources.StopStreamingRecognize_Description))]
    public class StopStreamingRecognize : ContinuableAsyncCodeActivity
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
        public InArgument<int> TimeoutMS { get; set; }

        #endregion


        #region Constructors

        public StopStreamingRecognize()
        {
            Constraints.Add(ActivityConstraints.HasParentType<StopStreamingRecognize, SpeechToTextStreamingScope>(string.Format(Resources.ValidationScope_Error, Resources.SpeechToTextStreamingScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SpeechToTextStreamingScope.ParentContainerPropertyTag);

            // Inputs
            var timeout = TimeoutMS.Get(context);

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SpeechToTextStreamingScope.ParentContainerPropertyTag);

            var service = objectContainer.Get<IBMWatsonSpeechToTextWebsocketService>();
            await service.CloseConnection();

            var recoder = objectContainer.Get<AudioMemoryRecorder>();
            recoder.Stop();
        }

        #endregion
    }
}

