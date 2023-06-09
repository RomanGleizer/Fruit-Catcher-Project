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
    private readonly int touchDelta;

    public Model(ContentManager contentManager)
    {
        touchDelta = 55;
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
            new Rectangle((int)texture.X, (int)texture.Y, texture.Width, texture.Width),
            Color.White);
    }

    public void DrawTexture(SpriteBatch spriteBatch, GameTexture texture, Vector2 position)
    {
        spriteBatch.Draw(
            texture.Texture,
            position,
            Color.White);
    }

    public void MoveTexture(GameTime gameTime, GameTexture texture)
    {
        texture.Y += texture.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void StopTextures(GameTime gameTime, GameTexture[][] texturesLayers)
    {
        foreach (var layer in texturesLayers)
            foreach (var texture in layer)
                texture.Y += 0 * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void InstantiteLayer(GameTexture[] textures, int yPosition)
    {
        foreach (var texture in textures)
        {
            LoadContent(texture);
            texture.Y = yPosition;
        }
    }

    public void SwitchLayers(GameTexture[] layer, int[] yPositions, ref int index, ref int yPositionsIndex)
    {
        var newLayer = GetObjectLayer(yPositions[index++], 400);

        if (layer.Length >= newLayer.Length)
            ChangeTextures(newLayer, layer);
        if (newLayer.Length >= layer.Length)
            ChangeTextures(layer, newLayer);

        InstantiteLayer(layer, yPositions[yPositionsIndex++]);
    }

    public GameTexture[] GetObjectLayer(int texturePositionY, int textureSpeed)
    {
        var random = new Random();
        var textures = new List<GameTexture>();
        var possiblePositionsX = new List<int> { 140, 320, 530, 680 };
        var possibleTextures = GetTextureData();

        for (int i = 0; i < possibleTextures.Length; i++)
        {
            var texturePositionX = possiblePositionsX.ElementAt(random.Next(0, possiblePositionsX.Count));
            var width = 80;
            var height = 80;
            var name = possibleTextures[random.Next(0, possibleTextures.Length)];

            if (name == "apple" || name == "orange" || name == "peach" || name == "pear" || name == "pineapple")
                textures.Add(new Fruit(texturePositionX, texturePositionY, textureSpeed, width, height, name));
            
            if (name == "Okylovskyi")
                textures.Add(new Shield(texturePositionX, texturePositionY, textureSpeed, width, height, name));
            
            if (name == "SiSharp")
                textures.Add(new Heal(texturePositionX, texturePositionY, textureSpeed, width, height, name));
            
            if (name == "axe" || name == "pick" || name == "tool")
                textures.Add(new Tool(texturePositionX, texturePositionY, textureSpeed, width, height, name));
        }

        return GetNotDuplicateObjectLayer(textures.ToArray(), 5);
    }

    public void ChangeTextures(GameTexture[] first, GameTexture[] second)
    {
        for (int i = 0; i < first.Length; i++)
            second[i] = first[i];
    }

    public GameTexture[][] GetTextureLayers(int length, int[] yPositions)
    {
        var textures = new GameTexture[length][];

        for (int i = 0; i < textures.Length; i++)
            textures[i] = GetObjectLayer(yPositions[i], 400);
        return textures;
    }

    public bool IsTouching(GameTexture first, GameTexture second)
    {
        return first.Rectangle.Bottom - touchDelta > second.Rectangle.Top &&
               first.Rectangle.Top < second.Rectangle.Top &&
               first.Rectangle.Right > second.Rectangle.Left &&
               first.Rectangle.Left < second.Rectangle.Right;
    }
}
