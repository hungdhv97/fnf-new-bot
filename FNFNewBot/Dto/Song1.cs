using Newtonsoft.Json;

namespace FNFNewBot.Dto;

public class SectionNotesDetail
{
    [JsonProperty("sectionNotes")] public List<List<double>> SectionNotes { get; set; }

    [JsonProperty("mustHitSection")] public bool MustHitSection { get; set; }
}

public class SongDetail
{
    [JsonProperty("notes")] public List<SectionNotesDetail> Notes { get; set; }

    [JsonProperty("song")] public string Name { get; set; }
}

public class Song1
{
    [JsonProperty("song")] public SongDetail Song { get; set; }
}