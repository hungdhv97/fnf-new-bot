using Newtonsoft.Json;

namespace FNFNewBot.DTO
{
    public class Note
    {
        [JsonProperty("d")] public int Direction { get; set; }

        [JsonProperty("t")] public double Time { get; set; }

        [JsonProperty("l")] public double? Length { get; set; }

        [JsonProperty("k")] public string? Key { get; set; }
    }

    public class Difficulty
    {
        [JsonProperty("easy")] public List<Note> Easy { get; set; } = new();

        [JsonProperty("hard")] public List<Note> Hard { get; set; } = new();

        [JsonProperty("normal")] public List<Note> Normal { get; set; } = new();

        [JsonProperty("erect")] public List<Note> Erect { get; set; } = new();
    }

    public class Song
    {
        [JsonProperty("notes")] public Difficulty Notes { get; set; } = new();
    }
}