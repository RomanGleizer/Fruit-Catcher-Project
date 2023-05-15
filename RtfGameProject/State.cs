using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RtfGameProject;

public abstract class State
{
    protected ContentManager _content;
    protected GraphicsDevice _graphicsDevice;
    protected Game1 _game;
    protected SpriteBatch _batch;
    protected Model _model;

    public bool IsPossibleOpenTutorial;
    public SpriteFont TutorialText;

    public GameTexture BlackBackground { get; set; }

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    public abstract void DrawOne(GameTime gameTime, SpriteBatch spriteBatch, int index);
    public abstract void PostUpdate(GameTime gameTime);
    public abstract void Update(GameTime gameTime);

    public State(
        Game1 game, 
        GraphicsDevice graphicsDevice, 
        ContentManager content, 
        Model model, 
        SpriteBatch batch, 
        bool isOpenTutorial,
        SpriteFont tutorialText)
    {
        _game = game;
        _graphicsDevice = graphicsDevice;
        _content = content;
        _model = model;
        _batch = batch;
        IsPossibleOpenTutorial = isOpenTutorial;
        TutorialText = tutorialText;
    }
}