using Newtonsoft.Json;

namespace FNFNewBot.DTO
{
    public class ScrollSpeed
    {
        [JsonProperty("easy")]
        public double Easy { get; set; }

        [JsonProperty("hard")]
        public double Hard { get; set; }

        [JsonProperty("normal")]
        public double Normal { get; set; }
    }

    public class EventValue
    {
        [JsonProperty("char")]
        public int? Char { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("ease")]
        public string Ease { get; set; } = null!;

        [JsonProperty("zoom")]
        public double? Zoom { get; set; }

        [JsonProperty("x")]
        public int? X { get; set; }

        [JsonProperty("y")]
        public int? Y { get; set; }
    }

    public class Event
    {
        [JsonProperty("t")]
        public double Time { get; set; }

        [JsonProperty("e")]
        public string EventType { get; set; } = null!;

        [JsonProperty("v")]
        public EventValue Value { get; set; } = null!;
    }

    public class Note
    {
        [JsonProperty("d")]
        public int Direction { get; set; }

        [JsonProperty("t")]
        public double Time { get; set; }

        [JsonProperty("l")]
        public double? Length { get; set; }

        [JsonProperty("k")]
        public string? Key { get; set; }
    }

    public class Difficulty
    {
        [JsonProperty("easy")]
        public List<Note> Easy { get; set; } = new();

        [JsonProperty("hard")]
        public List<Note> Hard { get; set; } = new();

        [JsonProperty("normal")]
        public List<Note> Normal { get; set; } = new();
    }

    public class Song
    {
        [JsonProperty("version")]
        public string Version { get; set; } = null!;

        [JsonProperty("scrollSpeed")]
        public ScrollSpeed ScrollSpeed { get; set; } = null!;

        [JsonProperty("events")]
        public List<Event> Events { get; set; } = new();

        [JsonProperty("notes")]
        public Difficulty Notes { get; set; } = new();
    }
}
