using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtfGameProject
{
    public class Tool : GameTexture
    {
        public Tool(float positionX, float positionY, float speed, int width, int height, string name)
        : base(positionX, positionY, speed, width, height, name)
        {
        }
    }
}