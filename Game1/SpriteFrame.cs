using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    class SpriteFrame
    {

        public List<Sprite> frames;

        public SpriteFrame()
        {
            frames = new List<Sprite>();
        }

        public void AddSprite(Sprite sprite)
        {
            frames.Add(sprite);
        }

    }
}
