namespace FNFNewBot.Dto;

public class KeyType
{
    private static readonly KeyType Left = new(Keys.Left, "←", Color.Blue);
    private static readonly KeyType Down = new(Keys.Down, "↓", Color.Green);
    private static readonly KeyType Up = new(Keys.Up, "↑", Color.Brown);
    private static readonly KeyType Right = new(Keys.Right, "→", Color.Purple);

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
            _ => throw new ArgumentException("Invalid note type", nameof(name)),
        };
    }
}