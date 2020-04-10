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


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
