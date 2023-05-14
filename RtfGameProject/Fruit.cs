using Microsoft.Xna.Framework.Graphics;

namespace RtfGameProject;

public class Fruit : GameTexture
{
    public Fruit(float positionX, float positionY, float speed, int width, int height, string name)
    : base(positionX, positionY, speed, width, height, name)
    {
    }
}