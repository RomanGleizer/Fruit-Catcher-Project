﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RtfGameProject;

public partial class Model
{
    private ContentManager content;

    public Model(ContentManager contentManager)
    {
        content = contentManager;
    }

    public void LoadContent(GameTexture texture)
    {
        texture.Texture = content.Load<Texture2D>(texture.Name);
    }

    public void DrawTexture(SpriteBatch spriteBatch, GameTexture texture)
    {
        spriteBatch.Draw(
            texture.Texture,
            new Rectangle((int)texture.PositionX, (int)texture.PositionY, texture.Width, texture.Width),
            Color.White);
    }

    public void MoveTexture(GameTime gameTime, GameTexture texture)
    {
        texture.PositionY += texture.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void InstantiteLayer(GameTexture[] textures, int yPosition)
    {
        foreach (var texture in textures)
        {
            LoadContent(texture);
            texture.PositionY = yPosition;
        }
    }

    public GameTexture[] GetObjectLayer(int texturePositionY, int textureSpeed)
    {
        Random random = new Random();
        List<GameTexture> textures = new List<GameTexture>();
        var possiblePositions = new List<int> { 0, 160, 340, 550, 700 };
        var possibleTextures = GetTextureData();

        for (int i = 0; i < possibleTextures.GetLength(0); i++)
        {
            textures.Add(new GameTexture(
                possiblePositions.ElementAt(random.Next(0, possiblePositions.Count)),
                texturePositionY,
                textureSpeed,
                (int)possibleTextures[i][0],
                (int)possibleTextures[i][1],
                (string)possibleTextures[i][2]
                ));
        }

        return GetNotDuplicateObjectLayer(textures.ToArray(), 5);
    }

    public GameTexture[][] GetTextureLayers(int length, int[] yPositions)
    {
        var textures = new GameTexture[length][];

        for (int i = 0; i < textures.Length; i++)
            textures[i] = GetObjectLayer(yPositions[i], 320);
        return textures;
    }

    public bool IsTouching(GameTexture first, GameTexture second)
    {
        return first.Rectangle.Bottom + first.Velocity.Y > second.Rectangle.Top &&
               first.Rectangle.Top < second.Rectangle.Top &&
               first.Rectangle.Right > second.Rectangle.Left &&
               first.Rectangle.Left < second.Rectangle.Right;
    }
}
