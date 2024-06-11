using Newtonsoft.Json;

namespace FNFNewBot.Dto;

public class Note2
{
    [JsonProperty("d")] public int Direction { get; set; }

    [JsonProperty("t")] public double Time { get; set; }

    [JsonProperty("l")] public double? Length { get; set; }

    [JsonProperty("k")] public string? Key { get; set; }
}

public class Difficulty2
{
    [JsonProperty("easy")] public List<Note2> Easy { get; set; } = new();

    [JsonProperty("hard")] public List<Note2> Hard { get; set; } = new();

    [JsonProperty("normal")] public List<Note2> Normal { get; set; } = new();

    [JsonProperty("erect")] public List<Note2> Erect { get; set; } = new();
    [JsonProperty("nightmare")] public List<Note2> Nightmare { get; set; } = new();
}

public class Song2
{
    [JsonProperty("notes")] public Difficulty2 Notes { get; set; } = new();
}