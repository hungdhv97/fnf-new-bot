namespace FNFNewBot.Dto;

public class KeyType
{
    public static readonly KeyType Left = new(Keys.Left, "←", Color.Blue);
    public static readonly KeyType Down = new(Keys.Down, "↓", Color.Green);
    public static readonly KeyType Up = new(Keys.Up, "↑", Color.Brown);
    public static readonly KeyType Right = new(Keys.Right, "→", Color.Purple);
    public static readonly KeyType A = new(Keys.A, "a", Color.DarkViolet);
    public static readonly KeyType S = new(Keys.S, "s", Color.SaddleBrown);
    public static readonly KeyType D = new(Keys.D, "d", Color.DarkBlue);
    public static readonly KeyType F = new(Keys.F, "f", Color.Firebrick);
    public static readonly KeyType Space = new(Keys.Space, "space", Color.Firebrick);

    public byte Code { get; private set; }
    public string Name { get; private set; }
    public Color Color { get; private set; }

    private KeyType(Keys key, string name, Color color)
    {
        Code = (byte)key;
        Name = name;
        Color = color;
    }

    public static KeyType FromString(string name)
    {
        return name switch
        {
            "left" => Left,
            "down" => Down,
            "up" => Up,
            "right" => Right,
            "a" => A,
            "s" => S,
            "d" => D,
            "f" => F,
            "space" => Space,
            _ => throw new ArgumentException($"Invalid key {name}"),
        };
    }
}