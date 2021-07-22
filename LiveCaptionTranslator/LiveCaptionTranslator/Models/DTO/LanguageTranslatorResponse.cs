namespace LiveCaptionTranslator.Models.DTO
{
    public class LanguageTranslatorResponse
    {
        public Translation[] translations { get; set; }
        public int word_count { get; set; }
        public int character_count { get; set; }
    }

    public class Translation
    {
        public string translation { get; set; }
    }
}
