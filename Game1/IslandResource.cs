using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    class IslandResource
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public string type;
        public Texture2D texture;
        public int progress;
        public CollisionObject collision_object;

        public IslandResource(int resource_x, int resource_y, int resource_width, int resource_height, string resource_type, Texture2D resource_texture, CollisionObject collobj)
        {
            x = resource_x;
            y = resource_y;
            width = resource_width;
            height = resource_height;
            type = resource_type;
            texture = resource_texture;
            collision_object = collobj;
            progress = 0;
        }

        public void UpdateProgress()
        {
            progress += 1;
        }
    }
}
