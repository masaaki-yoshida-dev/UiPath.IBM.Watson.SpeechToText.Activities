using LiveCaptionTranslator.Enums;
using System;
using System.Collections.Generic;

namespace LiveCaptionTranslator.Helper
{
    public static class AudioModelConvertHelper
    {
        public static string GetAudioModelName(AudioModel audiomodel)
        {
            var audioStr = "";
            switch (audiomodel)
            {
                case AudioModel.none:
                    break;
                case AudioModel.ar_AR_BroadbandModel:
                    audioStr = "ar-AR_BroadbandModel";
                    break;
                case AudioModel.de_DE_BroadbandModel:
                    audioStr = "de-DE_BroadbandModel";
                    break;
                case AudioModel.de_DE_NarrowbandModel:
                    audioStr = "de-DE_NarrowbandModel";
                    break;
                case AudioModel.en_GB_BroadbandModel:
                    audioStr = "en-GB_BroadbandModel";
                    break;
                case AudioModel.en_GB_NarrowbandModel:
                    audioStr = "en-GB_NarrowbandModel";
                    break;
                case AudioModel.en_US_BroadbandModel:
                    audioStr = "en-US_BroadbandModel";
                    break;
                case AudioModel.en_US_NarrowbandModel:
                    audioStr = "en-US_NarrowbandModel";
                    break;
                case AudioModel.en_US_ShortForm_NarrowbandModel:
                    audioStr = "en-US_ShortForm_NarrowbandModel";
                    break;
                case AudioModel.es_AR_BroadbandModel:
                    audioStr = "es-AR_BroadbandModel";
                    break;
                case AudioModel.es_AR_NarrowbandModel:
                    audioStr = "es-AR_NarrowbandModel";
                    break;
                case AudioModel.es_CL_BroadbandModel:
                    audioStr = "es-CL_BroadbandModel";
                    break;
                case AudioModel.es_CL_NarrowbandModel:
                    audioStr = "es-CL_NarrowbandModel";
                    break;
                case AudioModel.es_CO_BroadbandModel:
                    audioStr = "es-CO_BroadbandModel";
                    break;
                case AudioModel.es_CO_NarrowbandModel:
                    audioStr = "es-CO_NarrowbandModel";
                    break;
                case AudioModel.es_ES_BroadbandModel:
                    audioStr = "es-ES_BroadbandModel";
                    break;
                case AudioModel.es_ES_NarrowbandModel:
                    audioStr = "es-ES_NarrowbandModel";
                    break;
                case AudioModel.es_MX_BroadbandModel:
                    audioStr = "es-MX_BroadbandModel";
                    break;
                case AudioModel.es_MX_NarrowbandModel:
                    audioStr = "es-MX_NarrowbandModel";
                    break;
                case AudioModel.es_PE_BroadbandModel:
                    audioStr = "es-PE_BroadbandModel";
                    break;
                case AudioModel.es_PE_NarrowbandModel:
                    audioStr = "es-PE_NarrowbandModel";
                    break;
                case AudioModel.fr_FR_BroadbandModel:
                    audioStr = "fr-FR_BroadbandModel";
                    break;
                case AudioModel.fr_FR_NarrowbandModel:
                    audioStr = "fr-FR_NarrowbandModel";
                    break;
                case AudioModel.it_IT_BroadbandModel:
                    audioStr = "it-IT_BroadbandModel";
                    break;
                case AudioModel.it_IT_NarrowbandModel:
                    audioStr = "it-IT_NarrowbandModel";
                    break;
                case AudioModel.ja_JP_BroadbandModel:
                    audioStr = "ja-JP_BroadbandModel";
                    break;
                case AudioModel.ja_JP_NarrowbandModel:
                    audioStr = "ja-JP_NarrowbandModel";
                    break;
                case AudioModel.ko_KR_BroadbandModel:
                    audioStr = "ko-KR_BroadbandModel";
                    break;
                case AudioModel.ko_KR_NarrowbandModel:
                    audioStr = "ko-KR_NarrowbandModel";
                    break;
                case AudioModel.nl_NL_BroadbandModel:
                    audioStr = "nl-NL_BroadbandModel";
                    break;
                case AudioModel.nl_NL_NarrowbandModel:
                    audioStr = "nl-NL_NarrowbandModel";
                    break;
                case AudioModel.pt_BR_BroadbandModel:
                    audioStr = "pt-BR_BroadbandModel";
                    break;
                case AudioModel.pt_BR_NarrowbandModel:
                    audioStr = "pt-BR_NarrowbandModel";
                    break;
                case AudioModel.zh_CN_BroadbandModel:
                    audioStr = "zh-CN_BroadbandModel";
                    break;
                case AudioModel.zh_CN_NarrowbandModel:
                    audioStr = "zh-CN_NarrowbandModel";
                    break;
                default:
                    break;
            }
            return audioStr;
        }

        public static bool isExistAudioModel(string audioModel)
        {
            var modelList = new List<String> {
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

            return modelList.Contains(audioModel);
        }
    }
}
