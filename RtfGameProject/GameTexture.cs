using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RtfGameProject;

public class GameTexture
{
    private float x;
    private float y;
    private Texture2D texture;

    public float Speed { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Name { get; set; }
    public bool IsTouchedBucket { get; }
    public Vector2 Velocity { get; }

    public Texture2D Texture
    {
        get => texture;
        set => texture = value;
    }

    public float X
    {
        get => x;
        set => x = value;
    }

    public float Y
    {
        get => y;
        set => y = value;
    }

    public Rectangle Rectangle
    {
        get
        {
            return new Rectangle((int)x, (int)y, Width, Height);
        }
    }

    public GameTexture(float x, float y, float speed, int width, int height, string name)
    {
        X = x;
        Y = y;
        Speed = speed;
        Width = width;
        Height = height;
        Name = name;
    }
}