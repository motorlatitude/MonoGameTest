using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Texture2D oceanTexture;
        Texture2D landTexture;

        //Land Border Textures
        Texture2D landBorderTopTexture;
        Texture2D landBorderRightTexture;
        Texture2D landBorderBottomTexture;
        Texture2D landBorderLeftTexture;

        Texture2D landBorderTopRightTexture;
        Texture2D landBorderRightBottomTexture;
        Texture2D landBorderBottomLeftTexture;
        Texture2D landBorderTopLeftTexture;

        Texture2D landBorderTopBottomTexture;
        Texture2D landBorderRightLeftTexture;

        Texture2D landBorderTopRightBottomTexture;
        Texture2D landBorderRightBottomLeftTexture;
        Texture2D landBorderTopBottomLeftTexture;
        Texture2D landBorderTopRightLeftTexture;

        Texture2D landBorderTopRightBottomLeftTexture;

        Texture2D oceanBorderLeftTexture;
        Texture2D oceanBorderRightTexture;
        Texture2D oceanBorderRightLeftTexture;
        Texture2D oceanBorderTexture;

        Texture2D landFlowerTexture;
        Texture2D landFlowerTwoTexture;
        Texture2D landGravelTexture;
        Texture2D landDirtTexture;

        MapTile[][] mapTiles = new MapTile[10][];
        MapTilesExtras[] extraMapTiles = new MapTilesExtras[10];

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Character player;
        Texture2D playerTexture;
        float playerPositionX = 600f;
        float playerPositionY = 600f;
        float playerSpeed = 80f;

        int tileSize = 82;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 1200;
            Content.RootDirectory = "Content";

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        public void OnResize(Object sender, EventArgs e)
        {
            // Additional code to execute when the user drags the window
            // or in the case you programmatically change the screen or windows client screen size.
            // code that might directly change the backbuffer width height calling apply changes.
            // or passing changes that must occur in other classes or even calling there OnResize methods
            // though those methods can simply be added to the Windows event caller

        }

        public void GeneratePlayer()
        {
            player = new Character();
            int frames = 5;
            player.create(0, 0, 8, frames);
            for (int i = 0; i < frames; i++)
                player.addFrame(i, new Rectangle(i * 128, 0, 128, 256));
        }

        public void GenerateIsland()
        {
            int islandSize = 13;
            int i = 0;
            int k = 0;
            int totalOceanTiles = 0;
            mapTiles = new MapTile[islandSize][];
            extraMapTiles = new MapTilesExtras[islandSize];
            Random rnd = new Random();
            for (int x = 0; x < islandSize; x++)
            {
                mapTiles[x] = new MapTile[islandSize];
                for(int y = 0; y < islandSize; y++)
                {
                    if(x==0 || y == 0 || x == (islandSize - 1) || y == (islandSize - 1))
                    {
                        //nothing but the sweet ocean
                        mapTiles[x][y] = new MapTile(x, y, "ocean");
                    }
                    else
                    {
                        int r = rnd.Next(100); // gen int lower than 100
                        if(r < 15 && totalOceanTiles < 15) //avoid having too few land tiles, limit max number of ocean tiles within the 9x9 to 25
                        {
                            mapTiles[x][y] = new MapTile(x, y, "ocean");
                            totalOceanTiles++;
                        }
                        else
                        {
                            mapTiles[x][y] = new MapTile(x, y, "land");
                            int r_extras = rnd.Next(100); // gen int lower than 100
                            if (r_extras < 3 && k < 5)
                            {
                                extraMapTiles[k] = new MapTilesExtras(x, y, landFlowerTexture);
                                k++;
                            }
                            else if (r_extras < 7 && k < 5)
                            {
                                extraMapTiles[k] = new MapTilesExtras(x, y, landFlowerTwoTexture);
                                k++;
                            }
                            else if (r_extras < 9 && k < 5)
                            {
                                extraMapTiles[k] = new MapTilesExtras(x, y, landGravelTexture);
                                k++;
                            }
                            else if (r_extras < 12 && k < 5)
                            {
                                extraMapTiles[k] = new MapTilesExtras(x, y, landDirtTexture);
                                k++;
                            }
                        }
                    }
                    i++;
                }
            }

            //determine corners
            foreach (MapTile[] tilesX in mapTiles)
            {
                foreach (MapTile tile in tilesX)
                {
                    if (tile != null && tile.type == "land")
                    {
                        System.Diagnostics.Debug.WriteLine(tile.x);
                        if (tile.x > 0 && mapTiles[tile.x - 1][tile.y].type == "ocean")
                        {
                            //left hand border
                            tile.border_left = true;
                        }
                        if (tile.x < (islandSize - 1) && mapTiles[tile.x + 1][tile.y].type == "ocean")
                        {
                            //right hand border
                            tile.border_right = true;
                        }
                        if (tile.y > 0 && mapTiles[tile.x][tile.y - 1].type == "ocean")
                        {
                            //top hand border
                            tile.border_top = true;
                        }
                        if (tile.y < (islandSize - 1) && mapTiles[tile.x][tile.y + 1].type == "ocean")
                        {
                            //bottom hand border
                            tile.border_bottom = true;
                        }
                    }
                    else if(tile != null && tile.type == "ocean")
                    {
                        if (tile.y > 0 && mapTiles[tile.x][tile.y - 1].type == "land")
                        {
                            //land above
                            tile.ocean_border = true;
                            if (tile.x > 0 && mapTiles[tile.x - 1][tile.y - 1].type == "ocean")
                            {
                                tile.ocean_border_left = true;

                            }
                            if (tile.x < (islandSize - 1) && mapTiles[tile.x + 1][tile.y - 1].type == "ocean")
                            {
                                tile.ocean_border_right = true;

                            }
                        }
                    }
                }
            }

            //add flowers


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            oceanTexture = Content.Load<Texture2D>("ocean");
            landTexture = Content.Load<Texture2D>("land");


            landBorderTopTexture = Content.Load<Texture2D>("land_border_top");
            landBorderRightTexture = Content.Load<Texture2D>("land_border_right");
            landBorderBottomTexture = Content.Load<Texture2D>("land_border_bottom");
            landBorderLeftTexture = Content.Load<Texture2D>("land_border_left");

            landBorderTopRightTexture = Content.Load<Texture2D>("land_border_top_right");
            landBorderRightBottomTexture = Content.Load<Texture2D>("land_border_right_bottom");
            landBorderBottomLeftTexture = Content.Load<Texture2D>("land_border_bottom_left");
            landBorderTopLeftTexture = Content.Load<Texture2D>("land_border_top_left");

            landBorderTopBottomTexture = Content.Load<Texture2D>("land_border_top_bottom");
            landBorderRightLeftTexture = Content.Load<Texture2D>("land_border_right_left");

            landBorderTopRightBottomTexture = Content.Load<Texture2D>("land_border_top_right_bottom");
            landBorderRightBottomLeftTexture = Content.Load<Texture2D>("land_border_right_bottom_left");
            landBorderTopBottomLeftTexture = Content.Load<Texture2D>("land_border_top_bottom_left");
            landBorderTopRightLeftTexture = Content.Load<Texture2D>("land_border_top_right_left");

            landBorderTopRightBottomLeftTexture = Content.Load<Texture2D>("land_border_top_right_bottom_left");

            oceanBorderLeftTexture = Content.Load<Texture2D>("ocean_border_left");
            oceanBorderRightTexture = Content.Load<Texture2D>("ocean_border_right");
            oceanBorderRightLeftTexture = Content.Load<Texture2D>("ocean_border_right_left");
            oceanBorderTexture = Content.Load<Texture2D>("ocean_border");

            landFlowerTexture = Content.Load<Texture2D>("land_flowers");
            landFlowerTwoTexture = Content.Load<Texture2D>("land_flowers_2");
            landGravelTexture = Content.Load<Texture2D>("land_gravel");
            landDirtTexture = Content.Load<Texture2D>("land_dirt");

            playerTexture = Content.Load<Texture2D>("player");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(gameTime.ElapsedGameTime.TotalSeconds == 0)
            {
                GenerateIsland();
                GeneratePlayer();

            }
            else
            {
                player.animationFrameUpdate(gameTime);
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.Q))
                    GenerateIsland();

                //org player pos
                int org_min_ppx = (int)Math.Floor((playerPositionX - 15) / tileSize); // left
                int org_min_ppy = (int)Math.Floor((playerPositionY) / tileSize); // top
                int org_max_ppx = (int)Math.Floor((playerPositionX + 15) / tileSize); // right
                int org_max_ppy = (int)Math.Floor((playerPositionY + 40) / tileSize); // bottom
                float org_playerPositionX = playerPositionX;
                float org_playerPositionY = playerPositionY;

                if (kstate.IsKeyDown(Keys.W))
                    playerPositionY -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (kstate.IsKeyDown(Keys.S))
                    playerPositionY += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (kstate.IsKeyDown(Keys.A))
                    playerPositionX -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (kstate.IsKeyDown(Keys.D))
                    playerPositionX += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Player Collision Detection
                int min_ppx = (int)Math.Floor((playerPositionX - 15) / tileSize); // left
                int min_ppy = (int)Math.Floor((playerPositionY) / tileSize); // top
                int max_ppx = (int)Math.Floor((playerPositionX + 15) / tileSize); // right
                int max_ppy = (int)Math.Floor((playerPositionY + 40) / tileSize); // bottom
                if (min_ppx != org_min_ppx || min_ppy != org_min_ppy || max_ppx != org_max_ppx || max_ppy != org_max_ppy)
                {
                    if(min_ppx < mapTiles.Length && min_ppx > -1 && max_ppx < mapTiles.Length && max_ppx > -1)
                    {
                        if (max_ppy < mapTiles[max_ppx].Length && max_ppy < mapTiles[min_ppx].Length && min_ppy < mapTiles[max_ppx].Length && min_ppy < mapTiles[min_ppx].Length && min_ppy > -1)
                        {
                            if (mapTiles[min_ppx][min_ppy].type != "land")
                            {
                                if (mapTiles[org_min_ppx][min_ppy].type == "land")
                                {
                                    playerPositionX = org_playerPositionX;
                                }
                                else if (mapTiles[min_ppx][org_min_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;

                                }
                            }
                            if (mapTiles[max_ppx][min_ppy].type != "land")
                            {
                                if (mapTiles[org_max_ppx][min_ppy].type == "land")
                                {
                                    playerPositionX = org_playerPositionX;
                                }
                                else if (mapTiles[max_ppx][org_min_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;

                                }
                            }
                            if (mapTiles[min_ppx][max_ppy].type != "land")
                            {
                                if (mapTiles[org_min_ppx][max_ppy].type == "land")
                                {
                                    playerPositionX = org_playerPositionX;
                                }
                                else if (mapTiles[min_ppx][org_max_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;

                                }
                            }
                            if (mapTiles[max_ppx][max_ppy].type != "land")
                            {
                                if (mapTiles[org_max_ppx][max_ppy].type == "land")
                                {
                                    playerPositionX = org_playerPositionX;
                                }
                                else if (mapTiles[max_ppx][org_max_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;

                                }
                            }
                        }
                    }
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if(mapTiles.Length > 0)
            {
                spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0)); //move viewport / camera
                int x = 0;
                int y = 0;
                foreach (MapTile[] tilesX in mapTiles)
                {
                    foreach (MapTile tile in tilesX)
                    {
                        if (tile != null)
                        {
                            if(tile.type == "land")
                            {
                                if (tile.border_top && tile.border_right && tile.border_bottom && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderTopRightBottomLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_top && tile.border_right && tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderTopRightBottomTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_right && tile.border_bottom && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderRightBottomLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_top && tile.border_bottom && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderTopBottomLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_top && tile.border_right && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderTopRightLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_top && tile.border_right)
                                {
                                    spriteBatch.Draw(landBorderTopRightTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_right && tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderRightBottomTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_bottom && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderBottomLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_left && tile.border_top)
                                {
                                    spriteBatch.Draw(landBorderTopLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_left && tile.border_right)
                                {
                                    spriteBatch.Draw(landBorderRightLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_top && tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderTopBottomTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if(tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderBottomTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_top)
                                {
                                    spriteBatch.Draw(landBorderTopTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_right)
                                {
                                    spriteBatch.Draw(landBorderRightTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else
                                {
                                    spriteBatch.Draw(landTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                            }
                            else if (tile.type == "ocean")
                            {
                                if(tile.ocean_border_left && tile.ocean_border_right && tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderRightLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.ocean_border_left && tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderLeftTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.ocean_border_right && tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderRightTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else if (tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                                else
                                {
                                    spriteBatch.Draw(oceanTexture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                                }
                            }
                        }
                        y++;
                    }
                    x++;
                }
                foreach (MapTilesExtras extra_tile in extraMapTiles)
                {
                    if(extra_tile != null)
                    {
                        spriteBatch.Draw(extra_tile.texture, new Rectangle(extra_tile.x * tileSize, extra_tile.y * tileSize, tileSize, tileSize), Color.White);
                    }
                }

                spriteBatch.Draw(playerTexture, new Rectangle((int)playerPositionX - 20,(int)playerPositionY - 40, 40, 80), player.getDrawFrame(), Color.White);

                spriteBatch.End();
            }
            

            base.Draw(gameTime);
        }
    }
}
