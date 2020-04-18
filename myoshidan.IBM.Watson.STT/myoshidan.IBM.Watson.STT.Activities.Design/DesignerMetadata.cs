using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using myoshidan.IBM.Watson.STT.Activities.Design.Designers;
using myoshidan.IBM.Watson.STT.Activities.Design.Properties;

namespace myoshidan.IBM.Watson.STT.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            #region Setup

            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            #endregion Setup


            builder.AddCustomAttributes(typeof(SpeechToTextScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(SpeechToTextScope), new DesignerAttribute(typeof(SpeechToTextScopeDesigner)));
            builder.AddCustomAttributes(typeof(SpeechToTextScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(RecognizeAudioFile), categoryAttribute);
            builder.AddCustomAttributes(typeof(RecognizeAudioFile), new DesignerAttribute(typeof(RecognizeAudioFileDesigner)));
            builder.AddCustomAttributes(typeof(RecognizeAudioFile), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), new DesignerAttribute(typeof(SpeechToTextStreamingScopeDesigner)));
            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(StartStreamingRecognize), categoryAttribute);
            builder.AddCustomAttributes(typeof(StartStreamingRecognize), new DesignerAttribute(typeof(StartStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(StartStreamingRecognize), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(StopStreamingRecognize), categoryAttribute);
            builder.AddCustomAttributes(typeof(StopStreamingRecognize), new DesignerAttribute(typeof(StopStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(StopStreamingRecognize), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ReadStreamingRecognize), categoryAttribute);
            builder.AddCustomAttributes(typeof(ReadStreamingRecognize), new DesignerAttribute(typeof(ReadStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(ReadStreamingRecognize), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
