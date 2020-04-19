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

namespace myoshidan.IBM.Watson.STT.Activities
{
    /// <summary>
    /// SpeechToTextScope
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.SpeechToTextFileScope_DisplayName))]
    [LocalizedDescription(nameof(Resources.SpeechToTextFileScope_Description))]
    public class SpeechToTextFileScope : ContinuableAsyncNativeActivity
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

        [LocalizedDisplayName(nameof(Resources.SpeechToTextFileScope_APIKey_DisplayName))]
        [LocalizedDescription(nameof(Resources.SpeechToTextFileScope_APIKey_Description))]
        [LocalizedCategory(nameof(Resources.Authentication_Category))]
        public InArgument<string> APIKey { get; set; }

        [LocalizedDisplayName(nameof(Resources.SpeechToTextFileScope_URL_DisplayName))]
        [LocalizedDescription(nameof(Resources.SpeechToTextFileScope_URL_Description))]
        [LocalizedCategory(nameof(Resources.Authentication_Category))]
        public InArgument<string> URL { get; set; }

        // A tag used to identify the scope in the activity context
        internal static string ParentContainerPropertyTag => "ScopeActivity";

        // Object Container: Add strongly-typed objects here and they will be available in the scope's child activities.
        private readonly IObjectContainer _objectContainer;

        #endregion


        #region Constructors

        /// <summary>
        /// Speech To Text Scope
        /// </summary>
        /// <param name="objectContainer"></param>
        public SpeechToTextFileScope(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;

            Body = new ActivityAction<IObjectContainer>
            {
                Argument = new DelegateInArgument<IObjectContainer> (ParentContainerPropertyTag),
                Handler = new Sequence { DisplayName = Resources.Do }
            };
        }
        /// <summary>
        /// SpeechToTextScope
        /// </summary>
        public SpeechToTextFileScope() : this(new ObjectContainer())
        {

        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (APIKey == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(APIKey)));
            if (URL == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(URL)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<NativeActivityContext>> ExecuteAsync(NativeActivityContext  context, CancellationToken cancellationToken)
        {
            // Inputs
            var apikey = APIKey.Get(context);
            var url = URL.Get(context);

            await Task.Run(() =>
            {
                var service = new IBMWatsonSpeechToTextService(apikey, url);
                _objectContainer.Add(service);
            });

            return (ctx) => {
                // Schedule child activities
                if (Body != null)
				    ctx.ScheduleAction<IObjectContainer>(Body, _objectContainer, OnCompleted, OnFaulted);
            };
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
                    dispObject.Dispose();
            }
            _objectContainer.Clear();
        }

        #endregion
    }
}

