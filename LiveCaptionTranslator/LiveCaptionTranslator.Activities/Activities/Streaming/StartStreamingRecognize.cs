using System;
using System.Activities;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCaptionTranslator.Activities.Properties;
using LiveCaptionTranslator.Models;
using LiveCaptionTranslator.Models.IBMWatson;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace LiveCaptionTranslator.Activities
{
    [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_DisplayName))]
    [LocalizedDescription(nameof(Resources.StartStreamingRecognize_Description))]
    public class StartStreamingRecognize : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_LiveCaption_DisplayName))]
        [LocalizedDescription(nameof(Resources.StartStreamingRecognize_LiveCaption_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<bool> LiveCaption { get; set; } = true;

        [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_ExportFilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.StartStreamingRecognize_ExportFilePath_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<string> ExportFilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_LiveTranslator_DisplayName))]
        [LocalizedDescription(nameof(Resources.StartStreamingRecognize_LiveTranslator_Description))]
        [LocalizedCategory(nameof(Resources.Translator_Category))]
        public bool LiveTranslator { get; set; }

        [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_TranslatorAPIKey_DisplayName))]
        [LocalizedDescription(nameof(Resources.StartStreamingRecognize_TranslatorAPIKey_Description))]
        [LocalizedCategory(nameof(Resources.Translator_Category))]
        public InArgument<string> TranslatorAPIKey { get; set; }

        [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_TranslatorURL_DisplayName))]
        [LocalizedDescription(nameof(Resources.StartStreamingRecognize_TranslatorURL_Description))]
        [LocalizedCategory(nameof(Resources.Translator_Category))]
        public InArgument<string> TranslatorURL { get; set; }

        [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_TranslatorModelId_DisplayName))]
        [LocalizedDescription(nameof(Resources.StartStreamingRecognize_TranslatorModelId_Description))]
        [LocalizedCategory(nameof(Resources.Translator_Category))]
        public InArgument<string> TranslatorModelId { get; set; }

        #endregion


        #region Constructors

        public StartStreamingRecognize()
        {
            Constraints.Add(ActivityConstraints.HasParentType<StartStreamingRecognize, SpeechToTextStreamingScope>(string.Format(Resources.ValidationScope_Error, Resources.SpeechToTextStreamingScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (LiveTranslator)
            {
                if (TranslatorAPIKey == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TranslatorAPIKey)));
                if (TranslatorURL == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TranslatorURL)));
                if (TranslatorModelId == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TranslatorModelId)));
            }

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SpeechToTextStreamingScope.ParentContainerPropertyTag);

            // Inputs
            var filePath = ExportFilePath.Get(context);
            var transApikey = TranslatorAPIKey.Get(context);
            var transUrl = TranslatorURL.Get(context);
            var modelID = TranslatorModelId.Get(context);

            var livecaption = LiveCaption.Get(context);
            var livetranslator = LiveTranslator;

            if (!string.IsNullOrEmpty(filePath))
            {
                var writer = new TranscriptFileWriter(filePath);
                objectContainer.Add(writer);
            }

            if (LiveTranslator && !string.IsNullOrEmpty(transApikey) && !string.IsNullOrEmpty(transUrl) && !string.IsNullOrEmpty(modelID))
            {
                var transService = new IBMWatsonLanguageTranslatorService(transApikey, transUrl);
                transService.ModelId = modelID;
                objectContainer.Add(transService);
            }

            if (livecaption)
            {
                CreateLiveCaptionForm(objectContainer);
            }

            var service = objectContainer.Get<IBMWatsonSpeechToTextWebsocketService>();
            await service.StartConnection();

            var recoder = objectContainer.Get<AudioMemoryRecorder>();
            recoder.Start();

            service.HandleCallback();

            // Outputs
            return (ctx) => {
            };
        }
        
        #endregion

        #region private Methods

        private void CreateLiveCaptionForm(IObjectContainer objectContainer)
        {
            Task.Run(() =>
            {
                var f = new Form();
                f.BackColor = Color.Black;
                f.AutoSize = false;
                f.Opacity = 0.8;
                f.FormBorderStyle = FormBorderStyle.None;
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(0, (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.9));
                f.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.1));
                f.TopMost = true;
                f.ShowInTaskbar = false;

                var label = new Label();
                label.BackColor = Color.Transparent;
                label.BorderStyle = BorderStyle.None;
                label.ForeColor = Color.White;
                label.Font = new Font("MS UI Gothic", 16);
                label.AutoSize = false;
                label.Dock = DockStyle.Fill;
                label.Text = "";
                f.Controls.Add(label);
                objectContainer.Add(label);
                objectContainer.Add(f);

                Application.EnableVisualStyles();
                Application.Run(f);
            });
        }
        #endregion
    }
}

