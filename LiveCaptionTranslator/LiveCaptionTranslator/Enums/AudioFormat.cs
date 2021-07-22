using System.ComponentModel;

namespace LiveCaptionTranslator.Enums
{
    public enum AudioFormat
    {
        [Description("")]
        none,
        [Description("audio/alaw")]
        audio_alaw,
        [Description("audio/basic")]
        audio_basic,
        [Description("audio/flac")]
        audio_flac,
        [Description("audio/g729")]
        audio_g729,
        [Description("audio/l16")]
        audio_l16,
        [Description("audio/mp3")]
        audio_mp3,
        [Description("audio/mpeg")]
        audio_mpeg,
        [Description("audio/mulaw")]
        audio_mulaw,
        [Description("audio/ogg")]
        audio_ogg,
        [Description("audio/ogg;codecs=opus")]
        audio_ogg_codecs_opus,
        [Description("audio/ogg;codecs=vorbis")]
        audio_ogg_codecs_vorbis,
        [Description("audio/wav")]
        audio_wav,
        [Description("audio/webm")]
        audio_webm,
        [Description("audio/webm;codecs=opus")]
        audio_webm_codecs_opus,
        [Description("audio/webm;codecs=vorbis")]
        audio_webm_codecs_vorbis
    }
}
