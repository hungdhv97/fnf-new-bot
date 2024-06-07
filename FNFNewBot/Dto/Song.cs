using Newtonsoft.Json;

namespace FNFNewBot.Dto
{
    public class Note
    {
        [JsonProperty("d")] public int Direction { get; set; }

        [JsonProperty("t")] public double Time { get; set; }

        [JsonProperty("l")] public double? Length { get; set; }

        [JsonProperty("k")] public string? Key { get; set; }

        public byte GetKeyCode()
        {
            return Direction switch
            {
                0 => (byte)Keys.Left,
                1 => (byte)Keys.Up,
                2 => (byte)Keys.Right,
                3 => (byte)Keys.Down,
                _ => 0
            };
        }

        public string GetKeyName()
        {
            return Direction switch
            {
                0 => "←",
                1 => "↑",
                2 => "→",
                3 => "↓",
                _ => string.Empty
            };
        }

        public Color GetColor()
        {
            return Direction switch
            {
                0 => Color.Brown,
                1 => Color.Green,
                2 => Color.Blue,
                3 => Color.Purple,
                _ => Color.Black
            };
        }
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