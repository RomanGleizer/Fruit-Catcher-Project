using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace RtfGameProject;

public partial class Game1
{
    public Game1()
    {
        #region Initialization
        _graphics = new GraphicsDeviceManager(this);
        _gameModel = new Model(Content);
        _bucket = new Bucket(360, 400, 500, 65, 65, "bucket");
        _bubble = new GameTexture(_bucket.X, _bucket.Y, 500, 150, 150, "bubble");
        _tutorial = new GameTexture(0, 0, 0, 0, 0, "Tutorial");
        _win = new GameTexture(0, 0, 0, 0, 0, "Win");
        _finalYuri = new GameTexture(300, 250, 0, 150, 150, "Yurchik");

        _yPositions = new int[] { -50, -150, -250, -350, -450, -550, -650, -750, -850, -950, -1050, -1150 };
        _textureLayers = _gameModel.GetTextureLayers(11, _yPositions);

        BucketRigthBorder = _graphics.PreferredBackBufferWidth - _bucket.Width / 2 - 30;
        BucketLeftBorder = _bucket.Width / 2 - 30;
        _isGameStarted = false;
        _isOpenTutorial = false;
        _healthAmount = 3;
        _shieldTime = ShieldActivePeriodTime;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        #endregion
    }

    public void ChangeState(State state)
    {
        _nextState = state;
    }

    protected override void Initialize() { base.Initialize(); }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _currentState = new MenuState(
            this, 
            _graphics.GraphicsDevice, 
            Content, 
            _gameModel, 
            _spriteBatch, 
            _isOpenTutorial,
            _isGameEnd,
            _tutorialFont);

        #region Load Content
        foreach (var layer in _textureLayers)
            foreach (var texture in layer) 
                _gameModel.LoadContent(texture);

        _gameModel.LoadContent(_tutorial);
        _gameModel.LoadContent(_win);
        _gameModel.LoadContent(_bucket);
        _gameModel.LoadContent(_bubble);
        _gameModel.LoadContent(_finalYuri);
        _tutorialFont = Content.Load<SpriteFont>("Tutorial Text");
        _font = Content.Load<SpriteFont>("Score");
        #endregion
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        #region Menu Region
        _currentState.Update(gameTime);
        _currentState.PostUpdate(gameTime);

        if (_nextState != null)
        {
            _currentState = _nextState;
            _nextState = null;
            _isGameStarted = true;
        }
        #endregion

        if (_isGameStarted)
        {
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
            #region Game Logics
            _yPositionsIndex = 0;

            foreach (var layer in _textureLayers)
            {
                foreach (var texture in layer)
                {
                    _gameModel.MoveTexture(gameTime, texture);

                    if (texture.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + 50)
                        _gameModel.SwitchLayers(layer, _yPositions, ref _index, ref _yPositionsIndex);

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

            if (_isShieldActive)
            {
                _shieldActivePeriodTimer++;
                _shieldTime--;
            }
            if (_shieldActivePeriodTimer == ShieldActivePeriodTime)
            {
                _shieldActivePeriodTimer = 0;
                _shieldTime = ShieldActivePeriodTime;
                _isShieldActive = false;
            }
            if (_healthAmount == 0) Exit();
            if (_collisionCounter == 2) _isGameStarted = false;

            #endregion
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Bisque);
        _spriteBatch.Begin();

        _currentState.Draw(gameTime, _spriteBatch);
        _gameModel.DrawTexture(_spriteBatch, _bucket);

        foreach (var layer in _textureLayers)
            foreach (var texture in layer) 
                _gameModel.DrawTexture(_spriteBatch, texture);

        if (_isShieldActive)
        {
            _bubble.X = _bucket.X - 40;
            _bubble.Y = _bucket.Y - 50;
            _gameModel.DrawTexture(_spriteBatch, _bubble);
        }       
        if (_currentState.IsPossibleOpenTutorial)
        {
            _gameModel.DrawTexture(_spriteBatch, _tutorial, new Vector2(_tutorial.X, _tutorial.Y));
            _currentState.DrawOne(gameTime, _spriteBatch, 2);
        }
        if (!_currentState.IsPossibleOpenTutorial)
        {
            var shiledData = "Shield Time: " + (Math.Round(_shieldTime / 100)).ToString();
            _spriteBatch.DrawString(_font, "Points: " + _collisionCounter.ToString(), new Vector2(0, 0), Color.Green);
            _spriteBatch.DrawString(_font, "Health: " + _healthAmount.ToString(), new Vector2(0, 30), Color.Red);
            _spriteBatch.DrawString(_font, shiledData, new Vector2(0, 60), Color.Orange);
        }
        if (_collisionCounter == 2 && _isGameStarted == false)
        {
            _currentState.DrawOne(gameTime, _spriteBatch, 2);
            _gameModel.DrawTexture(_spriteBatch, _win, new Vector2(_tutorial.X, _tutorial.Y));
            _gameModel.DrawTexture(_spriteBatch, _finalYuri, new Vector2(_finalYuri.X, _finalYuri.Y));
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
