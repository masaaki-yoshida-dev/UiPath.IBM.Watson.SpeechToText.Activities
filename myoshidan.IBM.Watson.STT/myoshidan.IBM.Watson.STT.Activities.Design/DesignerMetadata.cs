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



            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
