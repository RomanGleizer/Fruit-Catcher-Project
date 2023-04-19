using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RtfGameProject;

public class Game1 : Game
{
    private SpriteFont font;
    private GameTexture[] firstLayerTextures;
    private GameTexture[] secondLayerTextures;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameTexture bucket;
    private Model gameModel;

    private const int TextureSpawnTime = 250;
    private const int FirstLayerPositionY = -50;
    private const int SecondLayerPositionY = -150;
    private const int TextureSpeed = 170;

    private readonly int BucketRigthBorder;
    private readonly int BucketLeftBorder;

    private double toolSpawnTimer;

    public Game1()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics = new GraphicsDeviceManager(this);
        gameModel = new Model(Content);
        bucket = new GameTexture(360, 400, 500, 65, 65, "bucket");

        BucketRigthBorder = _graphics.PreferredBackBufferWidth - bucket.Width / 2 - 30;
        BucketLeftBorder = bucket.Width / 2 - 30;
    }

    protected override void Initialize()
    {
        firstLayerTextures = gameModel.GetObjectLayer(FirstLayerPositionY, TextureSpeed);
        secondLayerTextures = gameModel.GetObjectLayer(SecondLayerPositionY, TextureSpeed);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach(var texture in firstLayerTextures) gameModel.LoadContent(texture);
        foreach (var texture in secondLayerTextures) gameModel.LoadContent(texture);
        gameModel.LoadContent(bucket);

        font = Content.Load<SpriteFont>("Score");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        #region Bucket Move
        var keyBoardState = Keyboard.GetState();
        var delta = bucket.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyBoardState.IsKeyDown(Keys.Right))
            bucket.PositionX += delta;
        if (keyBoardState.IsKeyDown(Keys.Left))
            bucket.PositionX -= delta;

        if (bucket.PositionX > BucketRigthBorder)
            bucket.PositionX = BucketRigthBorder;
        else if (bucket.PositionX < BucketLeftBorder)
            bucket.PositionX = BucketLeftBorder;
        #endregion 

        toolSpawnTimer++;
        if (toolSpawnTimer == TextureSpawnTime)
        {
            toolSpawnTimer = 0;

            firstLayerTextures = gameModel.GetObjectLayer(FirstLayerPositionY, TextureSpeed);
            secondLayerTextures = gameModel.GetObjectLayer(SecondLayerPositionY, TextureSpeed);

            gameModel.InstantiteFruit(firstLayerTextures, -50);
            gameModel.InstantiteFruit(secondLayerTextures, -150);
        }

        foreach (var textures in firstLayerTextures) gameModel.MoveTexture(gameTime, textures);
        foreach (var textures in secondLayerTextures) gameModel.MoveTexture(gameTime, textures);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        foreach (var textures in firstLayerTextures) gameModel.DrawTexture(_spriteBatch, textures);
        foreach (var textures in secondLayerTextures) gameModel.DrawTexture(_spriteBatch, textures);
        gameModel.DrawTexture(_spriteBatch, bucket);

        _spriteBatch.DrawString(font, gameTime.TotalGameTime.TotalSeconds.ToString(), new Vector2(0, 0), Color.Black);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}