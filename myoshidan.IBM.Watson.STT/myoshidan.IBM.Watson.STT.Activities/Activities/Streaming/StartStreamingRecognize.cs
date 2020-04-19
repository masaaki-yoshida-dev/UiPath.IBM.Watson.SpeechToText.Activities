using System;
using System.Activities;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myoshidan.IBM.Watson.STT.Activities.Properties;
using myoshidan.IBM.Watson.STT.Models;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace myoshidan.IBM.Watson.STT.Activities
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

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedCategory(nameof(Resources.Options_Category))]
        [LocalizedDisplayName(nameof(Resources.StartStreamingRecognize_LiveCaption_DisplayName))]
        [LocalizedDescription(nameof(Resources.StartStreamingRecognize_LiveCaption_Description))]
        public bool LiveCaption { get; set; }

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
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(SpeechToTextStreamingScope.ParentContainerPropertyTag);

            var service = objectContainer.Get<IBMWatsonSpeechToTextWebsocketService>();
            await service.StartConnection();

            var recoder = objectContainer.Get<AudioMemoryRecorder>();
            recoder.Start();

            if (LiveCaption)
            {
                CreateLiveCaptionForm(objectContainer);
            }

            await Task.Delay(1000);
            service.HandleCallback();
           
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
                label.Font = new Font("MS UI Gothic", 20);
                label.AutoSize = false;
                label.Dock = DockStyle.Fill;
                label.Text = "";
                f.Controls.Add(label);
                objectContainer.Add(label);

                Application.EnableVisualStyles();
                Application.Run(f);
            });
        }
        #endregion
    }
}

