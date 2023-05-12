using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RtfGameProject;

public class Game1 : Game
{
    private GameTexture[][] textureLayers;
    private int[] yPositions;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Bucket bucket;
    private GameTexture bubble;
    private Model gameModel;
    private SpriteFont scoreFont;
    private SpriteFont healthFont;

    private const int ShieldActivePeriodTime = 1000;

    private readonly int BucketRigthBorder;
    private readonly int BucketLeftBorder;

    private State currentState;
    private State nextState;
    private int shieldActivePeriodTimer;
    private int yPositionsIndex;
    private int collisionCounter;
    private int healthAmount;
    private bool isShieldActive;
    private int index;
    private bool isGameStarted = false;

    public Game1()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        graphics = new GraphicsDeviceManager(this);
        gameModel = new Model(Content);
        bucket = new Bucket(360, 400, 500, 65, 65, "bucket");
        bubble = new GameTexture(bucket.X, bucket.Y, 500, 80, 80, "bubble");

        yPositions = new int[] { -50, -150, -250, -350, -450, -550, -650, -750, -850, -950, -1050, -1150 };
        textureLayers = gameModel.GetTextureLayers(11, yPositions);

        BucketRigthBorder = graphics.PreferredBackBufferWidth - bucket.Width / 2 - 30;
        BucketLeftBorder = bucket.Width / 2 - 30;
        healthAmount = 3;
    }

    public void ChangeState(State state)
    {
        nextState = state;
    }

    protected override void Initialize() { base.Initialize(); }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        currentState = new MenuState(this, graphics.GraphicsDevice, Content);

        foreach (var layer in textureLayers)
        foreach (var texture in layer) gameModel.LoadContent(texture);
       
        gameModel.LoadContent(bucket);
        gameModel.LoadContent(bubble);

        scoreFont = Content.Load<SpriteFont>("Score");
        healthFont = Content.Load<SpriteFont>("Health");
    }

    protected override void Update(GameTime gameTime)
    {
        if (nextState != null)
        {
            currentState = nextState;
            nextState = null;
            isGameStarted = true;
        }

        currentState.Update(gameTime);
        currentState.PostUpdate(gameTime);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (isGameStarted)
        { 
            yPositionsIndex = 0;
            #region Bucket Move
            var keyBoardState = Keyboard.GetState();
            var delta = bucket.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyBoardState.IsKeyDown(Keys.Right))
                bucket.X += delta;
            if (keyBoardState.IsKeyDown(Keys.Left))
                bucket.X -= delta;

            if (bucket.X > BucketRigthBorder)
                bucket.X = BucketRigthBorder;
            else if (bucket.X < BucketLeftBorder)
                bucket.X = BucketLeftBorder;
            #endregion

            shieldActivePeriodTimer++;
            if (shieldActivePeriodTimer == ShieldActivePeriodTime)
            {
                shieldActivePeriodTimer = 0;
                isShieldActive = false;
            }

            foreach (var layer in textureLayers)
            {
                foreach (var texture in layer)
                {
                    gameModel.MoveTexture(gameTime, texture);
                    if (texture.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                    {
                        gameModel.InstantiteLayer(layer, yPositions[yPositionsIndex++]);
                        var newLayer = gameModel.GetObjectLayer(yPositions[index], 410);
                        layer[index].Texture = newLayer[index].Texture;
                        layer[index].Name = newLayer[index].Name;
                    }
                    index = 0;

                    if (gameModel.IsTouching(bucket, texture))
                    {
                        if (texture is Fruit) collisionCounter++;
                        if (texture is Shield) isShieldActive = true;
                        if (texture is Heal && healthAmount < 3) healthAmount++;
                        if (texture is Tool && !isShieldActive) healthAmount--;
                    }
                }
            }

            if (healthAmount == 0) Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();

        currentState.Draw(gameTime, spriteBatch);

        foreach (var layer in textureLayers)
        foreach (var texture in layer) gameModel.DrawTexture(spriteBatch, texture);

        gameModel.DrawTexture(spriteBatch, bucket);

        if (isShieldActive)
        {
            bubble.X = bucket.X - 5f;
            bubble.Y = bucket.Y;
            gameModel.DrawTexture(spriteBatch, bubble);
        }

        spriteBatch.DrawString(scoreFont, collisionCounter.ToString(), new Vector2(0, 0), Color.Black);
        spriteBatch.DrawString(healthFont, healthAmount.ToString(), new Vector2(0, 30), Color.Red);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}