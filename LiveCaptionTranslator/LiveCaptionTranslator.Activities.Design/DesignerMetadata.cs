using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using LiveCaptionTranslator.Activities.Design.Designers;
using LiveCaptionTranslator.Activities.Design.Properties;

namespace LiveCaptionTranslator.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), new DesignerAttribute(typeof(SpeechToTextStreamingScopeDesigner)));
            builder.AddCustomAttributes(typeof(SpeechToTextStreamingScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(StartStreamingRecognize), categoryAttribute);
            builder.AddCustomAttributes(typeof(StartStreamingRecognize), new DesignerAttribute(typeof(StartStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(StartStreamingRecognize), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(StopStreamingRecognize), categoryAttribute);
            builder.AddCustomAttributes(typeof(StopStreamingRecognize), new DesignerAttribute(typeof(StopStreamingRecognizeDesigner)));
            builder.AddCustomAttributes(typeof(StopStreamingRecognize), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
