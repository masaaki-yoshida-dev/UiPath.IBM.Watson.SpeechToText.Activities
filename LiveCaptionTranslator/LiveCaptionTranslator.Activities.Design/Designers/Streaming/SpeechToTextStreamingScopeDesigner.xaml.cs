using System;
using System.Collections.Generic;

namespace LiveCaptionTranslator.Activities.Design.Designers
{
    /// <summary>
    /// Interaction logic for SpeechToTextStreamingScopeDesigner.xaml
    /// </summary>
    public partial class SpeechToTextStreamingScopeDesigner
    {
        public SpeechToTextStreamingScopeDesigner()
        {
            InitializeComponent();
            audioModel.ItemsSource =
                new List<String> {
                    "ja-JP_BroadbandModel",
                    "en-US_BroadbandModel",
                    "ar-AR_BroadbandModel",
                    "de-DE_BroadbandModel",
                    "de-DE_NarrowbandModel",
                    "en-GB_BroadbandModel",
                    "en-GB_NarrowbandModel",
                    "en-US_NarrowbandModel",
                    "en-US_ShortForm_NarrowbandModel",
                    "es-AR_BroadbandModel",
                    "es-AR_NarrowbandModel",
                    "es-CL_BroadbandModel",
                    "es-CL_NarrowbandModel",
                    "es-CO_BroadbandModel",
                    "es-CO_NarrowbandModel",
                    "es-ES_BroadbandModel",
                    "es-ES_NarrowbandModel",
                    "es-MX_BroadbandModel",
                    "es-MX_NarrowbandModel",
                    "es-PE_BroadbandModel",
                    "es-PE_NarrowbandModel",
                    "fr-FR_BroadbandModel",
                    "fr-FR_NarrowbandModel",
                    "it-IT_BroadbandModel",
                    "it-IT_NarrowbandModel",
                    "ja-JP_NarrowbandModel",
                    "ko-KR_BroadbandModel",
                    "ko-KR_NarrowbandModel",
                    "nl-NL_BroadbandModel",
                    "nl-NL_NarrowbandModel",
                    "pt-BR_BroadbandModel",
                    "pt-BR_NarrowbandModel",
                    "zh-CN_BroadbandModel",
                    "zh-CN_NarrowbandModel" };
        }
    }
}
