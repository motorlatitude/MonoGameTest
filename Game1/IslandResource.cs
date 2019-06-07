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
        public int animatedProgress;
        public CollisionObject collision_object;
        public int HitAmount;
        public int AnimatedHitAmount;
        public float HitDelay;

        /// <summary>
        /// Create a new resource such as trees or flowers on the island
        /// </summary>
        /// <param name="resource_x">The X (island coords) position of the resource</param>
        /// <param name="resource_y">The Y (island coords) position of the resource</param>
        /// <param name="resource_width">The texture width of the resource</param>
        /// <param name="resource_height">The texture height of the resource</param>
        /// <param name="resource_texture">The texture of the resource</param>
        /// <param name="collobj">The collision object of the resource (usually smaller than texture sizes)</param>
        public IslandResource(int resource_x, int resource_y, int resource_width, int resource_height, string resource_type, Texture2D resource_texture, CollisionObject collobj)
        {
            x = resource_x;
            y = resource_y;
            width = resource_width;
            height = resource_height;
            type = resource_type;
            texture = resource_texture;
            collision_object = collobj;
            progress = 68;
            animatedProgress = 68;


            AnimatedHitAmount = 2;
            HitAmount = 25;
            HitDelay = 0.7f;
            if(type == "berry_bush")
            {
                HitAmount = 68;
                AnimatedHitAmount = 10;
                HitDelay = 0.4f;
            }
            else if(type == "coal_ore" || type == "iron_ore")
            {
                HitAmount = 24;
            }
            else if (type == "tree")
            {
                HitAmount = 35;
            }
        }
    }
}
