using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace RtfGameProject;

public class MenuState : State
{
    private List<Component> _components;

    public List<Component> Components { get; }

    public MenuState(
        Game1 game, 
        GraphicsDevice graphicsDevice,
        ContentManager content, 
        Model model, 
        SpriteBatch batch,
        bool isOpenTutorial, 
        SpriteFont tutorialText)
        : base(game, graphicsDevice, content, model, batch, isOpenTutorial, tutorialText)
    {
        var buttonTexture = _content.Load<Texture2D>("Button");
        var buttonFont = _content.Load<SpriteFont>("Font");
        
        var newGameButton = new Button(buttonTexture, buttonFont)
        {
            Position = new Vector2(320, 150),
            Text = "New Game"
        };
        newGameButton.Click += NewGameButton_Click;

        var tutorialGameButton = new Button(buttonTexture, buttonFont)
        {
            Position = new Vector2(320, 225),
            Text = "Tutorial"
        };
        tutorialGameButton.Click += TutorialGameButton_Click;

        var exitFromTutorialButton = new Button(buttonTexture, buttonFont)
        {
            Position = new Vector2(600, 375),
            Text = "Back"
        };
        exitFromTutorialButton.Click += ExitFromTutorialMenu_Click;

        var quitGameButton = new Button(buttonTexture, buttonFont)
        {
            Position = new Vector2(320, 300),
            Text = "Quit Game"
        };
        quitGameButton.Click += QuitGameButton_Click;

        _components = new List<Component>()
        {
            newGameButton,
            tutorialGameButton,
            exitFromTutorialButton,
            quitGameButton
        };
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _components[0].Draw(gameTime, spriteBatch);
        _components[1].Draw(gameTime, spriteBatch);
        _components[3].Draw(gameTime, spriteBatch);
    }
    public override void DrawOne(GameTime gameTime, SpriteBatch spriteBatch, int index)
    {
        _components[index].Draw(gameTime, spriteBatch);
    }

    private void NewGameButton_Click(object sender, EventArgs e)
    {
        _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _model, _batch, IsPossibleOpenTutorial, TutorialText));
    }

    public void TutorialGameButton_Click(object sender, EventArgs e)
    {
        IsPossibleOpenTutorial = true;
    }

    public void ExitFromTutorialMenu_Click(object sender, EventArgs e)
    {
        IsPossibleOpenTutorial = false;
    }

    public override void PostUpdate(GameTime gameTime)
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var component in _components)
            component.Update(gameTime);
    }

    private void QuitGameButton_Click(object sender, EventArgs e)
    {
        _game.Exit();
    }
}