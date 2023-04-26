namespace RtfGameProject;

public class Shield : GameTexture
{
    private bool _isActive;
    public bool IsActive { get => _isActive; set => _isActive = value; }

    public Shield(float positionX, float positionY, float speed, int width, int height, string name)
        : base(positionX, positionY, speed, width, height, name)
    {
    }
}
