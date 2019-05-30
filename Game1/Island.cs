using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1
{
    class Island
    {
        private int IslandSize;
        private ContentManager cm;
        private CollisionObjects CollisionManager;

        public int IslandOriginX;
        public int IslandOriginY;

        public MapTile[][] tiles;
        public MapTileDetails[] TileDetails;

        public Island(ContentManager mainContentManager, CollisionObjects mainCollisionManager)
        {
            IslandSize = 13;

            // (0, 0) island origin for main island
            IslandOriginX = 0;
            IslandOriginY = 0;

            cm = mainContentManager;
            CollisionManager = mainCollisionManager;
        }

        public void GenerateIsland(int island_size, int origin_x, int origin_y, int TileSize)
        {
            IslandSize = island_size;
            IslandOriginX = origin_x;
            IslandOriginY = origin_y;
            tiles = new MapTile[IslandSize][];
            TileDetails = new MapTileDetails[10];
            Random random = new Random();
            int TileDetailsIndex = 0;
            for (int x = 0; x < IslandSize; x++)
            {
                tiles[x] = new MapTile[IslandSize];
                for(int y = 0; y < IslandSize; y++)
                {
                    int r = random.Next(100);
                    if (y == 0 || x == 0 || y == IslandSize - 1 || x == IslandSize - 1 || r < 12)
                    {
                        tiles[x][y] = new MapTile(x, y, "ocean"); //generate a border of ocean tiles around the island and a few ocean tiles spotted in the island
                        CollisionManager.AddCollisionObject(new CollisionObject(x * TileSize, y * TileSize, TileSize, TileSize));
                    }
                    else
                    {
                        tiles[x][y] = new MapTile(x, y, "land");
                        if(r < 15 && TileDetailsIndex < 10)
                        {
                            TileDetails[TileDetailsIndex] = new MapTileDetails(x, y, cm.Load<Texture2D>("land_flowers"));
                            TileDetailsIndex++;
                        }
                        else if (r < 16 && TileDetailsIndex < 10)
                        {
                            TileDetails[TileDetailsIndex] = new MapTileDetails(x, y, cm.Load<Texture2D>("land_flowers_2"));
                            TileDetailsIndex++;
                        }
                        else if (r < 17 && TileDetailsIndex < 10)
                        {
                            TileDetails[TileDetailsIndex] = new MapTileDetails(x, y, cm.Load<Texture2D>("land_gravel"));
                            TileDetailsIndex++;
                        }
                        else if (r < 18 && TileDetailsIndex < 10)
                        {
                            TileDetails[TileDetailsIndex] = new MapTileDetails(x, y, cm.Load<Texture2D>("land_dirt"));
                            TileDetailsIndex++;
                        }
                    }
                }
            }
            SetTileTextures();
        }

        private void SetTileTextures()
        {
            foreach (MapTile[] x_tiles in tiles)
            {
                foreach(MapTile t in x_tiles)
                {
                    if(t.type == "land")
                    {
                        t.border_left = (t.x > 0 && tiles[t.x - 1][t.y].type == "ocean") ? true : false;
                        t.border_right = (t.x < (IslandSize - 1) && tiles[t.x + 1][t.y].type == "ocean") ? true : false;
                        t.border_top = (t.y > 0 && tiles[t.x][t.y - 1].type == "ocean") ? true : false;
                        t.border_bottom = (t.y < (IslandSize - 1) && tiles[t.x][t.y + 1].type == "ocean") ? true : false;
                        string texture_name = "land";
                        if (t.border_left || t.border_right || t.border_top || t.border_bottom)
                        {
                            texture_name += "_border";
                            texture_name += (t.border_top) ? "_top" : "";
                            texture_name += (t.border_right) ? "_right" : "";
                            texture_name += (t.border_bottom) ? "_bottom" : "";
                            texture_name += (t.border_left) ? "_left" : "";
                        }
                        t.texture = cm.Load<Texture2D>(texture_name);
                    }
                    else if(t.type == "ocean")
                    {
                        string texture_name = "ocean";
                        if (t.y > 0 && tiles[t.x][t.y - 1].type == "land")
                        {
                            t.ocean_border = true;
                            t.ocean_border_left = (t.x > 0 && tiles[t.x - 1][t.y - 1].type == "ocean") ? true : false;
                            t.ocean_border_right = (t.x < (IslandSize - 1) && tiles[t.x + 1][t.y - 1].type == "ocean") ? true : false;
                            if(t.ocean_border || t.ocean_border_left || t.ocean_border_right)
                            {
                                texture_name += "_border";
                                texture_name += t.ocean_border_right ? "_right" : "";
                                texture_name += t.ocean_border_left ? "_left" : "";
                            }
                        }
                        t.texture = cm.Load<Texture2D>(texture_name);
                    }
                }
            }
        }

    }
}
