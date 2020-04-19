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

            var categoryFileAttribute = new CategoryAttribute($"{Resources.CategoryFile}");
            var categoryStreamingAttribute = new CategoryAttribute($"{Resources.CategoryStreaming}");

            #endregion Setup

            #region FileGroup
            builder.AddCustomAttributes(typeof(SpeechToTextFileScope), categoryFileAttribute);
            builder.AddCustomAttributes(typeof(SpeechToTextFileScope), new DesignerAttribute(typeof(SpeechToTextFileScopeDesigner)));
            builder.AddCustomAttributes(typeof(SpeechToTextFileScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(RecognizeAudioFile), categoryFileAttribute);
            builder.AddCustomAttributes(typeof(RecognizeAudioFile), new DesignerAttribute(typeof(RecognizeAudioFileDesigner)));
            builder.AddCustomAttributes(typeof(RecognizeAudioFile), new HelpKeywordAttribute(""));
            #endregion

            #region Streaming
            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), categoryStreamingAttribute);
            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), new DesignerAttribute(typeof(SpeechToTextStreamingScopeDesigner)));
            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(StartStreamingRecognize), categoryStreamingAttribute);
            builder.AddCustomAttributes(typeof(StartStreamingRecognize), new DesignerAttribute(typeof(StartStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(StartStreamingRecognize), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(StopStreamingRecognize), categoryStreamingAttribute);
            builder.AddCustomAttributes(typeof(StopStreamingRecognize), new DesignerAttribute(typeof(StopStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(StopStreamingRecognize), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ReadStreamingRecognize), categoryStreamingAttribute);
            builder.AddCustomAttributes(typeof(ReadStreamingRecognize), new DesignerAttribute(typeof(ReadStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(ReadStreamingRecognize), new HelpKeywordAttribute(""));
            #endregion

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
