using Newtonsoft.Json;

namespace FNFNewBot.Dto
{
    public class Note3
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("sLen")] public double SLen { get; set; }

        [JsonProperty("time")] public double Time { get; set; }

        [JsonProperty("type")] public int Type { get; set; }
    }

    public class StrumLine3
    {
        [JsonProperty("notes")] public List<Note3> Notes { get; set; }

        [JsonProperty("type")] public int Type { get; set; }
    }

    public class Song3
    {
        [JsonProperty("strumLines")] public List<StrumLine3> StrumLines { get; set; }
    }
}
