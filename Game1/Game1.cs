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
        Island[] islands;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float CameraOriginX = 600f;
        float CameraOriginY = 600f;

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
                islands = new Island[1];
                Island main_island = new Island(Content);
                main_island.GenerateIsland(13);
                islands[0] = main_island;
                GeneratePlayer();

            }
            else
            {
                player.animationFrameUpdate(gameTime);
                Island main_island = islands[0];
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.Q))
                {
                    main_island.GenerateIsland(13);
                }

                //org player pos
                int org_min_ppx = (int)Math.Floor((playerPositionX - 15) / tileSize); // left
                int org_min_ppy = (int)Math.Floor((playerPositionY) / tileSize); // top
                int org_max_ppx = (int)Math.Floor((playerPositionX + 15) / tileSize); // right
                int org_max_ppy = (int)Math.Floor((playerPositionY + 40) / tileSize); // bottom
                float org_playerPositionX = playerPositionX;
                float org_playerPositionY = playerPositionY;
                float org_CameraPositionX = CameraOriginX;
                float org_CameraPositionY = CameraOriginY;

                if (kstate.IsKeyDown(Keys.W))
                {
                    playerPositionY -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    CameraOriginY += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Keys.S))
                {
                    playerPositionY += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    CameraOriginY -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Keys.A))
                {
                    playerPositionX -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    CameraOriginX += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Keys.D))
                {
                    playerPositionX += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    CameraOriginX -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                MapTile[][] mapTiles = main_island.tiles;

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
                                    CameraOriginX = org_CameraPositionX;
                                }
                                else if (mapTiles[min_ppx][org_min_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginY = org_CameraPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginX = org_CameraPositionX;
                                    CameraOriginY = org_CameraPositionY;

                                }
                            }
                            if (mapTiles[max_ppx][min_ppy].type != "land")
                            {
                                if (mapTiles[org_max_ppx][min_ppy].type == "land")
                                {
                                    playerPositionX = org_playerPositionX;
                                    CameraOriginX = org_CameraPositionX;
                                }
                                else if (mapTiles[max_ppx][org_min_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginY = org_CameraPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginX = org_CameraPositionX;
                                    CameraOriginY = org_CameraPositionY;

                                }
                            }
                            if (mapTiles[min_ppx][max_ppy].type != "land")
                            {
                                if (mapTiles[org_min_ppx][max_ppy].type == "land")
                                {
                                    playerPositionX = org_playerPositionX;
                                    CameraOriginX = org_CameraPositionX;
                                }
                                else if (mapTiles[min_ppx][org_max_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginY = org_CameraPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginX = org_CameraPositionX;
                                    CameraOriginY = org_CameraPositionY;

                                }
                            }
                            if (mapTiles[max_ppx][max_ppy].type != "land")
                            {
                                if (mapTiles[org_max_ppx][max_ppy].type == "land")
                                {
                                    playerPositionX = org_playerPositionX;
                                    CameraOriginX = org_CameraPositionX;
                                }
                                else if (mapTiles[max_ppx][org_max_ppy].type == "land")
                                {
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginY = org_CameraPositionY;
                                }
                                else
                                {
                                    playerPositionX = org_playerPositionX;
                                    playerPositionY = org_playerPositionY;
                                    CameraOriginX = org_CameraPositionX;
                                    CameraOriginY = org_CameraPositionY;

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
            Island main_island = islands[0];
            if (main_island.tiles.Length > 0)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation((int)CameraOriginX - 600, (int)CameraOriginY - 600, 0)); //move viewport / camera
                int x = 0;
                int y = 0;
                foreach (MapTile[] tilesX in main_island.tiles)
                {
                    foreach (MapTile tile in tilesX)
                    {
                        if (tile != null && tile.texture != null)
                        {
                            spriteBatch.Draw(tile.texture, new Rectangle(tile.x * tileSize, tile.y * tileSize, tileSize, tileSize), Color.White);
                        }
                        y++;
                    }
                    x++;
                }
                foreach (MapTileDetails tile_details in main_island.TileDetails)
                {
                    if(tile_details != null && tile_details.texture != null)
                    {
                        spriteBatch.Draw(tile_details.texture, new Rectangle(tile_details.x * tileSize, tile_details.y * tileSize, tileSize, tileSize), Color.White);
                    }
                }

                spriteBatch.Draw(playerTexture, new Rectangle((int)playerPositionX - 20,(int)playerPositionY - 40, 40, 80), player.getDrawFrame(), Color.White);

                spriteBatch.End();
            }
            

            base.Draw(gameTime);
        }
    }
}
