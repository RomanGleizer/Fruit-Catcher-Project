using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RtfGameProject;

public class GameFont
{
    private SpriteFont spriteFont;

    public string Name { get; }
    public Color Color { get; }
    public Vector2 Position { get; }
    public object Data { get; }

    public SpriteFont SpriteFont 
    { 
        get => spriteFont;
        set => spriteFont = value;
    }

    public GameFont(string name, SpriteFont spriteFont, object data, Vector2 position, Color color)
    {
        SpriteFont = spriteFont;
        Name = name;
        Data = data;
        Position = position;
        Color = color;
    }
}
