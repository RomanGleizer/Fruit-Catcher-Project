using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace RtfGameProject;

public class MenuState : State
{
    private List<Component> _components;

    public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
    {
        var buttonTexture = _content.Load<Texture2D>("Button");
        var buttonFont = _content.Load<SpriteFont>("Font");
        
        var newGameButton = new Button(buttonTexture, buttonFont)
        {
            Position = new Vector2(300, 200),
            Text = "New Game",
        };

        newGameButton.Click += NewGameButton_Click;

        var quitGameButton = new Button(buttonTexture, buttonFont)
        {
            Position = new Vector2(300, 300),
            Text = "Quit Game",
        };

        quitGameButton.Click += QuitGameButton_Click;

        _components = new List<Component>()
        {
            newGameButton,
            quitGameButton,
        };
}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var component in _components)
            component.Draw(gameTime, spriteBatch);
    }

    private void NewGameButton_Click(object sender, EventArgs e)
    {
        _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
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