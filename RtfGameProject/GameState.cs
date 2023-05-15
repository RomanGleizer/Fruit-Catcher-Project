using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RtfGameProject;

public class GameState : State
{
    public GameState(
        Game1 game, 
        GraphicsDevice graphicsDevice,
        ContentManager content, 
        Model model, 
        SpriteBatch batch, 
        bool isOpenTutorial,
        SpriteFont tutorialText)
      : base(game, graphicsDevice, content, model, batch, isOpenTutorial, tutorialText)
    {
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }

    public override void DrawOne(GameTime gameTime, SpriteBatch spriteBatch, int index)
    {
    }

    public override void PostUpdate(GameTime gameTime)
    {
    }

    public override void Update(GameTime gameTime)
    {
    }
}
