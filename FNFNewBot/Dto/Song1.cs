using Newtonsoft.Json;

namespace FNFNewBot.Dto;

public class SectionNotes1Detail
{
    [JsonProperty("sectionNotes")] public List<List<double>> SectionNotes { get; set; }

    [JsonProperty("mustHitSection")] public bool MustHitSection { get; set; }
}

public class Song1Detail
{
    [JsonProperty("notes")] public List<SectionNotes1Detail> Notes { get; set; }

    [JsonProperty("song")] public string Name { get; set; }
}

public class Song1
{
    [JsonProperty("song")] public Song1Detail Song { get; set; }
}