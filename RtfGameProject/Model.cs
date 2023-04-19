using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RtfGameProject;

public class Model
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

    public void InstantiteFruit(GameTexture[] textures, int yPosition)
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
            textures[i] = GetObjectLayer(yPositions[i], 170);
        return textures;
    }

    private GameTexture[] GetNotDuplicateObjectLayer(GameTexture[] objects, int size)
    {
        Random random = new Random();
        var randomObjects = new GameTexture[size];
        var objectPositionData = new Dictionary<GameTexture, float>();
        var noDuplicateObjectDict = new Dictionary<GameTexture, float>();
        var result = new List<GameTexture>();

        for (int i = 0; i < randomObjects.Length; i++)
        {
            var randomObject = objects[random.Next(0, objects.Length)];
            randomObjects[i] = randomObject;
            if (!objectPositionData.ContainsKey(randomObject))
                objectPositionData.Add(randomObject, randomObject.PositionX);
        }

        foreach (var fruit in objectPositionData)
            if (!noDuplicateObjectDict.ContainsKey(fruit.Key) 
                && !noDuplicateObjectDict.ContainsValue(fruit.Value))
                noDuplicateObjectDict.Add(fruit.Key, fruit.Value);

        foreach (var obj in noDuplicateObjectDict.Keys) result.Add(obj);

        return result.ToArray();
    }

    private object[][] GetTextureData()
    {
        return new object[][]
        {
            new object[] { 100, 100, "apple" },
            new object[] { 70, 70, "orange" },
            new object[] { 85, 85, "peach" },
            new object[] { 80, 90, "pear" },
            new object[] { 90, 90, "pineapple" },
            new object[] { 70, 70, "axe" },
            new object[] { 60, 60, "pick" },
            new object[] { 80, 80, "tool" },
            new object[] { 110, 110, "SiSharp" },
            new object[] { 90, 90, "Okylovskyi" }
        };
    }
}
