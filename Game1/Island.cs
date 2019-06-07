﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    class Island
    {
        private int IslandSize;
        private int TileSize;
        private string IslandID;
        private ContentManager cm;
        private CollisionObjects CollisionManager;
        private Random rng;

        public int IslandOriginX;
        public int IslandOriginY;

        public MapTile[][] tiles;
        public MapTileDetails[] TileDetails;

        public List<IslandResource> IslandResources;
        public List<IslandBuilding> IslandBuildings;

        public Island(ContentManager mainContentManager, CollisionObjects mainCollisionManager)
        {
            IslandSize = 13;

            // (0, 0) island origin for main island
            IslandOriginX = 0;
            IslandOriginY = 0;

            cm = mainContentManager;
            CollisionManager = mainCollisionManager;
        }

        public void GenerateIsland(int island_size, int origin_x, int origin_y, int island_TileSize, string island_name)
        {
            IslandSize = island_size;
            IslandID = island_name;
            TileSize = island_TileSize;
            IslandOriginX = origin_x;
            IslandOriginY = origin_y;
            tiles = new MapTile[IslandSize][];
            TileDetails = new MapTileDetails[10];
            IslandResources = new List<IslandResource>();
            IslandBuildings = new List<IslandBuilding>();
            rng = new Random();
            Random random = rng;
            int TileDetailsIndex = 0;
            for (int x = 0; x < IslandSize; x++)
            {
                tiles[x] = new MapTile[IslandSize];
                for(int y = 0; y < IslandSize; y++)
                {
                    int r = random.Next(100);
                    if (y == 0 || x == 0 || y == IslandSize - 1 || x == IslandSize - 1 || r < 8)
                    {
                        tiles[x][y] = new MapTile(x, y, "ocean"); //generate a border of ocean tiles around the island and a few ocean tiles spotted in the island
                        CollisionManager.AddCollisionObject(new CollisionObject(IslandOriginX * TileSize + x * TileSize, IslandOriginY * TileSize + y * TileSize, TileSize, TileSize, "island_tile", IslandID)); //make ocean tiles collideable (player can't move over those)
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

        public void GenerateNewResource(int loop = 1)
        {
            int i = 0;
            while(i < loop)
            {
                int r_x = rng.Next(IslandSize);
                int r_y = rng.Next(IslandSize);
                int r_type = rng.Next(8); //types * 10; types = tree, iron_ore
                //TODO: also check so that we dont spawn a resource on the character as they would get stuck otherwise
                if (tiles[r_x][r_y].type == "land" && !IslandResources.Exists((res) => res.x == r_x && res.y == r_y))
                {
                    if (r_type == 0)
                    {
                        CollisionObject o = new CollisionObject(IslandOriginX * TileSize + r_x * TileSize, IslandOriginY * TileSize + r_y * TileSize + (TileSize / 2), TileSize, (TileSize / 2), "island_tile_resource", IslandID);
                        IslandResources.Add(new IslandResource(r_x, r_y, TileSize-16, TileSize-16, "iron_ore", cm.Load<Texture2D>("iron_ore"), o));
                        CollisionManager.AddCollisionObject(o);
                    }
                    else if (r_type >= 1 && r_type <= 4)
                    {
                        CollisionObject o = new CollisionObject(IslandOriginX * TileSize + r_x * TileSize + (TileSize / 4), IslandOriginY * TileSize + r_y * TileSize + (TileSize / 2), (TileSize / 2), (TileSize / 2), "island_tile_resource", IslandID);
                        IslandResources.Add(new IslandResource(r_x, r_y, TileSize, TileSize * 2, "tree", cm.Load<Texture2D>("tree"), o));
                        CollisionManager.AddCollisionObject(o);
                    }
                    else if (r_type == 5)
                    {
                        CollisionObject o = new CollisionObject(IslandOriginX * TileSize + r_x * TileSize, IslandOriginY * TileSize + r_y * TileSize + (TileSize / 2), TileSize, (TileSize / 2), "island_tile_resource", IslandID);
                        IslandResources.Add(new IslandResource(r_x, r_y, TileSize - 16, TileSize - 16, "coal_ore", cm.Load<Texture2D>("coal_ore"), o));
                        CollisionManager.AddCollisionObject(o);
                    }
                    else if (r_type == 6)
                    {
                        CollisionObject o = new CollisionObject(IslandOriginX * TileSize + r_x * TileSize, IslandOriginY * TileSize + r_y * TileSize + (TileSize / 2), TileSize, (TileSize / 2), "island_tile_resource", IslandID);
                        IslandResources.Add(new IslandResource(r_x, r_y, TileSize - 16, TileSize - 16, "rock", cm.Load<Texture2D>("rock"), o));
                        CollisionManager.AddCollisionObject(o);
                    }
                    else if (r_type == 7)
                    {
                        CollisionObject o = new CollisionObject(IslandOriginX * TileSize + r_x * TileSize, IslandOriginY * TileSize + r_y * TileSize + (TileSize / 2), TileSize, (TileSize / 2), "island_tile_resource", IslandID);
                        IslandResources.Add(new IslandResource(r_x, r_y, TileSize - 16, TileSize - 16, "berry_bush", cm.Load<Texture2D>("berry_bush"), o));
                        CollisionManager.AddCollisionObject(o);
                    }
                }
                i++;
            }
        }

        public void AddBuilding(int x, int y, string type)
        {
            if(type == "bridge" && tiles[x][y].type == "ocean")
            {
                IslandBuildings.Add(new IslandBuilding(x, y, TileSize, TileSize, "bridge", cm.Load<Texture2D>("bridge")));
                //remove collision object
                CollisionManager.RemoveCollisionObjectWithXAndY(IslandOriginX * TileSize + x * TileSize, IslandOriginY * TileSize + y * TileSize, "island_tile");
            }
            else if (type == "fishing_net" && tiles[x][y].type == "ocean")
            {
                IslandBuildings.Add(new IslandBuilding(x, y, TileSize, TileSize, "fishing_net", cm.Load<Texture2D>("fishing_net")));
            }
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
