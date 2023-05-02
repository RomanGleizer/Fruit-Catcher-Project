using System;
using System.Collections.Generic;

namespace RtfGameProject;

public partial class Model
{
    private GameTexture[] GetNotDuplicateObjectLayer(GameTexture[] objects, int size)
    {
        Random random = new Random();
        var objectPositionData = new Dictionary<GameTexture, float>();
        var noDuplicateObjectDict = new Dictionary<GameTexture, float>();
        var result = new List<GameTexture>();

        for (int i = 0; i < size; i++)
        {
            var randomObject = objects[random.Next(0, objects.Length)];
            if (!objectPositionData.ContainsKey(randomObject))
                objectPositionData.Add(randomObject, randomObject.X);
        }

        foreach (var fruit in objectPositionData)
            if (!noDuplicateObjectDict.ContainsKey(fruit.Key)
                && !noDuplicateObjectDict.ContainsValue(fruit.Value))
                noDuplicateObjectDict.Add(fruit.Key, fruit.Value);

        foreach (var obj in noDuplicateObjectDict.Keys) result.Add(obj);

        return result.ToArray();
    }

    private string[] GetTextureData()
    {
        return new string[]
        {
            "apple",
            "orange",
            "peach",
            "pear",
            "pineapple",
            "axe",
            "pick",
            "tool",
            "SiSharp",
            "Okylovskyi"
        };
    }
}
