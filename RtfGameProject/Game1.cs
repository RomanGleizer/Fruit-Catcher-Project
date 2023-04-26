using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RtfGameProject;

public class Game1 : Game
{
    private SpriteFont scoreFont;
    private SpriteFont healthFont;
    private GameTexture[][] textureLayers;
    private int[] yPositions;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Bucket bucket;
    private Model gameModel;

    private const int TextureSpawnTime = 150;

    private readonly int BucketRigthBorder;
    private readonly int BucketLeftBorder;

    private double toolSpawnTimer;
    private int yPositionsIndex;
    private int collisionCounter;
    private int healthAmount;

    public Game1()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics = new GraphicsDeviceManager(this);
        gameModel = new Model(Content);
        bucket = new Bucket(360, 400, 500, 65, 65, "bucket");

        BucketRigthBorder = _graphics.PreferredBackBufferWidth - bucket.Width / 2 - 30;
        BucketLeftBorder = bucket.Width / 2 - 30;
    }

    protected override void Initialize()
    {
        yPositions = new int[] { -50, -150, -250, -350, -450 };
        textureLayers = gameModel.GetTextureLayers(5, yPositions);
        healthAmount = 3;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var layer in textureLayers)
        foreach (var texture in layer) gameModel.LoadContent(texture);
       
        gameModel.LoadContent(bucket);
        scoreFont = Content.Load<SpriteFont>("Score");
        healthFont = Content.Load<SpriteFont>("Health");
    }

    protected override void Update(GameTime gameTime)
    {
        yPositionsIndex = 0;

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
            textureLayers = gameModel.GetTextureLayers(5, yPositions);

            foreach (var layer in textureLayers)
                gameModel.InstantiteLayer(layer, yPositions[yPositionsIndex++]);
        }

        foreach (var layer in textureLayers)
            foreach (var texture in layer)
            {
                gameModel.MoveTexture(gameTime, texture);

                if (gameModel.IsTouching(bucket, texture) && texture is Fruit)
                    collisionCounter++;
                else if (gameModel.IsTouching(bucket, texture) && texture is not Fruit)
                    healthAmount--;
            }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        foreach (var layer in textureLayers)
        foreach (var texture in layer) gameModel.DrawTexture(_spriteBatch, texture);

        gameModel.DrawTexture(_spriteBatch, bucket);

        _spriteBatch.DrawString(scoreFont, collisionCounter.ToString(), new Vector2(0, 0), Color.Black);
        _spriteBatch.DrawString(healthFont, healthAmount.ToString(), new Vector2(0, 100), Color.Red);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}