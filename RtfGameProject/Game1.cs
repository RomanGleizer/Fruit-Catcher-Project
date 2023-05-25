using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RtfGameProject;

public partial class Game1 : Game
{
    private const double ShieldActivePeriodTime = 500;

    private readonly int BucketRigthBorder;
    private readonly int BucketLeftBorder;

    private GameTexture[][] _textureLayers;
    private int[] _yPositions;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Bucket _bucket;
    private GameTexture _bubble;
    private GameTexture _tutorial;
    private GameTexture _win;
    private GameTexture _finalYuri;
    private Model _gameModel;
    private SpriteFont _font;
    private SpriteFont _tutorialFont;
    private State _currentState;
    private State _nextState;
    private int _shieldActivePeriodTimer;
    private int _yPositionsIndex;
    private int _collisionCounter;
    private int _healthAmount;
    private int _index;
    private double _shieldTime;
    private bool _isShieldActive;
    private bool _isGameStarted;
    private bool _isOpenTutorial;
    private bool _isGameEnd;
}