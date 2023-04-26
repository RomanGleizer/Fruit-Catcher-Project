using System;
using System.Collections.Generic;

namespace RtfGameProject
{
    public partial class Model
    {
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
}
