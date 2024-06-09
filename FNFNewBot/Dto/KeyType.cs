namespace FNFNewBot.Dto;

public class KeyType
{
    public static readonly KeyType Left = new(Keys.Left, "←", Color.Blue);
    public static readonly KeyType Down = new(Keys.Down, "↓", Color.Green);
    public static readonly KeyType Up = new(Keys.Up, "↑", Color.Brown);
    public static readonly KeyType Right = new(Keys.Right, "→", Color.Purple);
    public static readonly KeyType A = new(Keys.A, "a", Color.DarkViolet);
    public static readonly KeyType B = new(Keys.B, "b", Color.Black);
    public static readonly KeyType C = new(Keys.C, "c", Color.Crimson);
    public static readonly KeyType D = new(Keys.D, "d", Color.Firebrick);
    public static readonly KeyType E = new(Keys.E, "e", Color.DarkTurquoise);
    public static readonly KeyType F = new(Keys.F, "f", Color.DarkSlateGray);
    public static readonly KeyType G = new(Keys.G, "g", Color.DarkSeaGreen);
    public static readonly KeyType H = new(Keys.H, "h", Color.DarkGray);
    public static readonly KeyType I = new(Keys.I, "i", Color.DarkKhaki);
    public static readonly KeyType J = new(Keys.J, "j", Color.DarkGreen);
    public static readonly KeyType K = new(Keys.K, "k", Color.DarkMagenta);
    public static readonly KeyType L = new(Keys.L, "l", Color.DarkOliveGreen);
    public static readonly KeyType M = new(Keys.M, "m", Color.DarkOrchid);
    public static readonly KeyType N = new(Keys.N, "n", Color.DarkRed);
    public static readonly KeyType O = new(Keys.O, "o", Color.DarkSalmon);
    public static readonly KeyType P = new(Keys.P, "p", Color.DarkSlateBlue);
    public static readonly KeyType Q = new(Keys.Q, "q", Color.DarkSlateGray);
    public static readonly KeyType R = new(Keys.R, "r", Color.DarkTurquoise);
    public static readonly KeyType S = new(Keys.S, "s", Color.SaddleBrown);
    public static readonly KeyType T = new(Keys.T, "t", Color.DarkViolet);
    public static readonly KeyType U = new(Keys.U, "u", Color.DeepPink);
    public static readonly KeyType V = new(Keys.V, "v", Color.DeepSkyBlue);
    public static readonly KeyType W = new(Keys.W, "w", Color.DarkBlue);
    public static readonly KeyType X = new(Keys.X, "x", Color.DimGray);
    public static readonly KeyType Y = new(Keys.Y, "y", Color.DodgerBlue);
    public static readonly KeyType Z = new(Keys.Z, "z", Color.ForestGreen);
    public static readonly KeyType Space = new(Keys.Space, "space", Color.DarkOrange);

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
