namespace FNFNewBot.Dto;

public class KeyType
{
    private static readonly KeyType Left = new(Keys.Left, "←", Color.Blue);
    private static readonly KeyType Down = new(Keys.Down, "↓", Color.Green);
    private static readonly KeyType Up = new(Keys.Up, "↑", Color.Brown);
    private static readonly KeyType Right = new(Keys.Right, "→", Color.Purple);
    private static readonly KeyType A = new(Keys.A, "a", Color.DarkViolet);
    private static readonly KeyType B = new(Keys.B, "b", Color.Black);
    private static readonly KeyType C = new(Keys.C, "c", Color.Crimson);
    private static readonly KeyType D = new(Keys.D, "d", Color.Firebrick);
    private static readonly KeyType E = new(Keys.E, "e", Color.DarkTurquoise);
    private static readonly KeyType F = new(Keys.F, "f", Color.DarkSlateGray);
    private static readonly KeyType G = new(Keys.G, "g", Color.DarkSeaGreen);
    private static readonly KeyType H = new(Keys.H, "h", Color.DarkGray);
    private static readonly KeyType I = new(Keys.I, "i", Color.DarkKhaki);
    private static readonly KeyType J = new(Keys.J, "j", Color.DarkGreen);
    private static readonly KeyType K = new(Keys.K, "k", Color.DarkMagenta);
    private static readonly KeyType L = new(Keys.L, "l", Color.DarkOliveGreen);
    private static readonly KeyType M = new(Keys.M, "m", Color.DarkOrchid);
    private static readonly KeyType N = new(Keys.N, "n", Color.DarkRed);
    private static readonly KeyType O = new(Keys.O, "o", Color.DarkSalmon);
    private static readonly KeyType P = new(Keys.P, "p", Color.DarkSlateBlue);
    private static readonly KeyType Q = new(Keys.Q, "q", Color.DarkSlateGray);
    private static readonly KeyType R = new(Keys.R, "r", Color.DarkTurquoise);
    private static readonly KeyType S = new(Keys.S, "s", Color.SaddleBrown);
    private static readonly KeyType T = new(Keys.T, "t", Color.DarkViolet);
    private static readonly KeyType U = new(Keys.U, "u", Color.DeepPink);
    private static readonly KeyType V = new(Keys.V, "v", Color.DeepSkyBlue);
    private static readonly KeyType W = new(Keys.W, "w", Color.DarkBlue);
    private static readonly KeyType X = new(Keys.X, "x", Color.DimGray);
    private static readonly KeyType Y = new(Keys.Y, "y", Color.DodgerBlue);
    private static readonly KeyType Z = new(Keys.Z, "z", Color.ForestGreen);
    private static readonly KeyType Space = new(Keys.Space, "space", Color.DarkOrange);

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
            "b" => B,
            "c" => C,
            "d" => D,
            "e" => E,
            "f" => F,
            "g" => G,
            "h" => H,
            "i" => I,
            "j" => J,
            "k" => K,
            "l" => L,
            "m" => M,
            "n" => N,
            "o" => O,
            "p" => P,
            "q" => Q,
            "r" => R,
            "s" => S,
            "t" => T,
            "u" => U,
            "v" => V,
            "w" => W,
            "x" => X,
            "y" => Y,
            "z" => Z,
            "space" => Space,
            _ => throw new ArgumentException($"Invalid key {name}"),
        };
    }
}
