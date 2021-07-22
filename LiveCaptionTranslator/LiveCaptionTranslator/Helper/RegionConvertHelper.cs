using LiveCaptionTranslator.Enums;

namespace LiveCaptionTranslator.Helper
{
    public static class RegionConvertHelper
    {
        public static string GetRegionUrl(Region region)
        {
            var regionStr = "";
            switch (region)
            {
                case Region.Dallas:
                    regionStr = "stream.watsonplatform.net";
                    break;
                case Region.WashingtonDC:
                    regionStr = "gateway-wdc.watsonplatform.net";
                    break;
                case Region.Frankfurt:
                    regionStr = "stream-fra.watsonplatform.net";
                    break;
                case Region.Sydney:
                    regionStr = "gateway-syd.watsonplatform.net";
                    break;
                case Region.Tokyo:
                    regionStr = "gateway-tok.watsonplatform.net";
                    break;
                case Region.London:
                    regionStr = "gateway-lon.watsonplatform.net";
                    break;
                default:
                    regionStr = "stream.watsonplatform.net";
                    break;
            }

            return regionStr;
        }
    }
}
