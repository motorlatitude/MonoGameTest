using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    class IslandBuilding
    {

        public string Type;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Texture2D BuildingTexture;

        public IslandBuilding(int x, int y, int width, int height, string type, Texture2D texture)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = type;
            BuildingTexture = texture;
        }
    }
}
