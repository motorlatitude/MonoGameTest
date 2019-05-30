using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    class Sprite
    {

        public Texture2D texture;
        public Rectangle destinationRectangle;
        public Rectangle? sourceRectangle;
        public Color color;

        public Sprite(Texture2D sprite_texture, Rectangle sprite_rect, Color sprite_color, Rectangle? sprite_frame = null)
        {
            texture = sprite_texture;
            destinationRectangle = sprite_rect;
            sourceRectangle = null;
            if(sprite_frame != null)
            {
                sourceRectangle = sprite_frame;
            }
            color = sprite_color;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public Rectangle getDestinationRectangle()
        {
            return destinationRectangle;
        }

    }
}
