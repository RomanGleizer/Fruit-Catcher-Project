using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RtfGameProject;

public partial class Game1
{
    public Game1()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics = new GraphicsDeviceManager(this);
        _gameModel = new Model(Content);
        _bucket = new Bucket(360, 400, 500, 65, 65, "bucket");
        _bubble = new GameTexture(_bucket.X, _bucket.Y, 500, 150, 150, "bubble");

        _tutorialText = new GameTexture(0, 0, 0, 0, 0, "Tutorial");

        _blackBlackground = new GameTexture
            (
                0, 0, 0, 
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, 
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, 
                "Black"
            );

        _yPositions = new int[] { -50, -150, -250, -350, -450, -550, -650, -750, -850, -950, -1050, -1150 };
        _textureLayers = _gameModel.GetTextureLayers(11, _yPositions);

        BucketRigthBorder = _graphics.PreferredBackBufferWidth - _bucket.Width / 2 - 30;
        BucketLeftBorder = _bucket.Width / 2 - 30;
        _isGameStarted = false;
        _isOpenTutorial = false;
        _healthAmount = 3;
    }

    public void ChangeState(State state)
    {
        _nextState = state;
    }

    protected override void Initialize() { base.Initialize(); }

    protected override void LoadContent()
    {
        _tutorialFont = Content.Load<SpriteFont>("Tutorial Text");
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _currentState = new MenuState(this, _graphics.GraphicsDevice, Content, _gameModel, _spriteBatch, _isOpenTutorial, _tutorialFont);

        foreach (var layer in _textureLayers)
            foreach (var texture in layer) _gameModel.LoadContent(texture);

        _gameModel.LoadContent(_tutorialText);
        _gameModel.LoadContent(_bucket);
        _gameModel.LoadContent(_blackBlackground);
        _gameModel.LoadContent(_bubble);

        _font = Content.Load<SpriteFont>("Score");
    }

    protected override void Update(GameTime gameTime)
    {
        _currentState.Update(gameTime);
        _currentState.PostUpdate(gameTime);

        if (_nextState != null)
        {
            _currentState = _nextState;
            _nextState = null;
            _isGameStarted = true;
        }
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (_isGameStarted)
        {
            _yPositionsIndex = 0;
            _shieldActivePeriodTimer++;
            #region Bucket Move
            var keyBoardState = Keyboard.GetState();
            var delta = _bucket.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyBoardState.IsKeyDown(Keys.Right))
                _bucket.X += delta;
            if (keyBoardState.IsKeyDown(Keys.Left))
                _bucket.X -= delta;

            if (_bucket.X > BucketRigthBorder)
                _bucket.X = BucketRigthBorder;
            else if (_bucket.X < BucketLeftBorder)
                _bucket.X = BucketLeftBorder;
            #endregion

            if (_shieldActivePeriodTimer == ShieldActivePeriodTime)
            {
                _shieldActivePeriodTimer = 0;
                _isShieldActive = false;
            }

            foreach (var layer in _textureLayers)
            {
                foreach (var texture in layer)
                {
                    _gameModel.MoveTexture(gameTime, texture);
                    if (texture.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + 50)
                    {
                        var newLayer = _gameModel.GetObjectLayer(_yPositions[_index++], 400);

                        if (layer.Length >= newLayer.Length) _gameModel.ChangeTextures(newLayer, layer);
                        if (newLayer.Length >= layer.Length) _gameModel.ChangeTextures(layer, newLayer);

                        _gameModel.InstantiteLayer(layer, _yPositions[_yPositionsIndex++]);
                    }

                    if (_gameModel.IsTouching(_bucket, texture))
                    {
                        if (texture is Fruit) _collisionCounter++;
                        if (texture is Shield) _isShieldActive = true;
                        if (texture is Heal && _healthAmount < 3) _healthAmount++;
                        if (texture is Tool && !_isShieldActive) _healthAmount--;
                        texture.Y -= 2000;
                    }
                }
                _index = 0;
            }

            if (_healthAmount == 0 || _collisionCounter == 100) Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Bisque);
        _spriteBatch.Begin();

        _currentState.Draw(gameTime, _spriteBatch);

        foreach (var layer in _textureLayers)
            foreach (var texture in layer) 
                _gameModel.DrawTexture(_spriteBatch, texture);

        _gameModel.DrawTexture(_spriteBatch, _bucket);

        if (_isShieldActive)
        {
            _bubble.X = _bucket.X - 40;
            _bubble.Y = _bucket.Y - 50;
            _gameModel.DrawTexture(_spriteBatch, _bubble);
        }       
        if (_currentState.IsPossibleOpenTutorial)
        {
            _gameModel.DrawTexture(_spriteBatch, _blackBlackground);
            _gameModel.DrawTexture(_spriteBatch, _tutorialText, new Vector2(_tutorialText.X, _tutorialText.Y));
            _currentState.DrawOne(gameTime, _spriteBatch, 2);
        }

        if (!_currentState.IsPossibleOpenTutorial)
        {
            _spriteBatch.DrawString(_font, "Points: ", new Vector2(0, 0), Color.Green);
            _spriteBatch.DrawString(_font, _collisionCounter.ToString(), new Vector2(60, 0), Color.Green);
            _spriteBatch.DrawString(_font, "Health: ", new Vector2(0, 30), Color.Red);
            _spriteBatch.DrawString(_font, _healthAmount.ToString(), new Vector2(60, 30), Color.Red);
            _spriteBatch.DrawString(_font, "Shield Time: ", new Vector2(0, 60), Color.Red);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
