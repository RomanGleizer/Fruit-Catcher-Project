using Microsoft.Xna.Framework.Graphics;

namespace RtfGameProject;

public class GameTexture
{
    private float positionX;
    private float positionY;
    private Texture2D texture;

    public Texture2D Texture
    {
        get => texture;
        set => texture = value;
    }

    public float PositionX
    {
        get => positionX;
        set => positionX = value;
    }

    public float PositionY
    {
        get => positionY;
        set => positionY = value;
    }

    public float Speed { get; }
    public int Width { get; }
    public int Height { get; }
    public string Name { get; }

    public GameTexture(float positionX, float positionY, float speed, int width, int height, string name)
    {
        PositionX = positionX;
        PositionY = positionY;
        Speed = speed;
        Width = width;
        Height = height;
        Name = name;
    }
}