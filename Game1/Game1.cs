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
        Texture2D ballTexture;
        Texture2D oceanTexture;
        Texture2D landTexture;
        Texture2D corLandTexture;

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

        MapTile[][] mapTiles = new MapTile[10][];

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1280;
            Content.RootDirectory = "Content";
        }

        public void GenerateIsland()
        {
            int i = 0;
            int totalOceanTiles = 0;
            Random rnd = new Random();
            for (int x = 0; x < 10; x++)
            {
                mapTiles[x] = new MapTile[10];
                for(int y = 0; y < 10; y++)
                {
                    if(x==0 || y == 0 || x == 9 || y == 9)
                    {
                        //nothing but the sweet ocean
                        mapTiles[x][y] = new MapTile(x, y, "ocean");
                    }
                    else
                    {
                        int r = rnd.Next(100); // gen int lower than 100
                        if(r < 25 && totalOceanTiles < 15) //avoid having too few land tiles, limit max number of ocean tiles within the 9x9 to 25
                        {
                            mapTiles[x][y] = new MapTile(x, y, "ocean");
                            totalOceanTiles++;
                        }
                        else
                        {
                            mapTiles[x][y] = new MapTile(x, y, "land");
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
                        if (tile.x < 9 && mapTiles[tile.x + 1][tile.y].type == "ocean")
                        {
                            //right hand border
                            tile.border_right = true;
                        }
                        if (tile.y > 0 && mapTiles[tile.x][tile.y - 1].type == "ocean")
                        {
                            //top hand border
                            tile.border_top = true;
                        }
                        if (tile.y < 9 && mapTiles[tile.x][tile.y + 1].type == "ocean")
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
                            if (tile.x < 9 && mapTiles[tile.x + 1][tile.y - 1].type == "ocean")
                            {
                                tile.ocean_border_right = true;

                            }
                        }
                    }
                }
            }


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
            ballTexture = Content.Load<Texture2D>("ball");
            oceanTexture = Content.Load<Texture2D>("ocean");
            landTexture = Content.Load<Texture2D>("land");
            corLandTexture = Content.Load<Texture2D>("cor_land");

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
            }

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.A))
                GenerateIsland();


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
                spriteBatch.Begin();
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
                                    spriteBatch.Draw(landBorderTopRightBottomLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_top && tile.border_right && tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderTopRightBottomTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_right && tile.border_bottom && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderRightBottomLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_top && tile.border_bottom && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderTopBottomLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_top && tile.border_right && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderTopRightLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_top && tile.border_right)
                                {
                                    spriteBatch.Draw(landBorderTopRightTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_right && tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderRightBottomTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_bottom && tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderBottomLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_left && tile.border_top)
                                {
                                    spriteBatch.Draw(landBorderTopLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_left && tile.border_right)
                                {
                                    spriteBatch.Draw(landBorderRightLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_top && tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderTopBottomTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if(tile.border_bottom)
                                {
                                    spriteBatch.Draw(landBorderBottomTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_top)
                                {
                                    spriteBatch.Draw(landBorderTopTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_right)
                                {
                                    spriteBatch.Draw(landBorderRightTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.border_left)
                                {
                                    spriteBatch.Draw(landBorderLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else
                                {
                                    spriteBatch.Draw(landTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                            }
                            else if (tile.type == "ocean")
                            {
                                if(tile.ocean_border_left && tile.ocean_border_right && tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderRightLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.ocean_border_left && tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderLeftTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.ocean_border_right && tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderRightTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else if (tile.ocean_border)
                                {
                                    spriteBatch.Draw(oceanBorderTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                                else
                                {
                                    spriteBatch.Draw(oceanTexture, new Rectangle(tile.x * 128, tile.y * 128, 128, 128), Color.White);
                                }
                            }
                        }
                        y++;
                    }
                    x++;
                }
                spriteBatch.End();
            }
            

            base.Draw(gameTime);
        }
    }
}
