
namespace myoshidan.IBM.Watson.STT.Models.DTO
{
    public class StreamingRecognizeResponse
    {
        public Result[] results { get; set; }
        public int result_index { get; set; }
    }

    public class Result
    {
        public Alternative[] alternatives { get; set; }
        public bool final { get; set; }
    }

    public class Alternative
    {
        public string transcript { get; set; }
    }
}
