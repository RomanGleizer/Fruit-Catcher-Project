using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RtfGameProject;

public partial class Game1 : Game
{
    private const int ShieldActivePeriodTime = 700;

    private readonly int BucketRigthBorder;
    private readonly int BucketLeftBorder;

    private GameTexture[][] _textureLayers;
    private int[] _yPositions;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Bucket _bucket;
    private GameTexture _bubble;
    private GameTexture _blackBlackground;
    private Model _gameModel;
    private SpriteFont _scoreFont;
    private SpriteFont _healthFont;
    private SpriteFont _tutorialText;
    private State _currentState;
    private State _nextState;
    private int _shieldActivePeriodTimer;
    private int _yPositionsIndex;
    private int _collisionCounter;
    private int _healthAmount;
    private int _index;
    private bool _isShieldActive;
    private bool _isGameStarted;
    private bool _isOpenTutorial;
    private string _tutorialDescription;
}