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

        CollisionObjects CollisionManager;
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
            CollisionManager = new CollisionObjects();
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

            if (gameTime.ElapsedGameTime.TotalSeconds == 0)
            {
                islands = new Island[2];
                Island main_island = new Island(Content, CollisionManager);
                main_island.GenerateIsland(13, 0, 0, tileSize);
                islands[0] = main_island;
                Island second_island = new Island(Content, CollisionManager);
                second_island.GenerateIsland(13, 13, 0, tileSize);
                islands[1] = second_island;
                GeneratePlayer();

            }
            else
            {
                player.animationFrameUpdate(gameTime);
                Island main_island = islands[0];
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.Q))
                {
                    main_island.GenerateIsland(13, 0, 0, tileSize);
                }

                //org player pos

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

                bool collision = CollisionManager.CheckIfCollidingWithObject((int)playerPositionX + 10, (int)playerPositionY + 40, 20, 40);

                if (collision)
                {
                    playerPositionX = org_playerPositionX;
                    CameraOriginX = org_CameraPositionX;
                    playerPositionY = org_playerPositionY;
                    CameraOriginY = org_CameraPositionY;
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
                foreach(Island island in islands)
                {
                    int x = 0;
                    int y = 0;
                    foreach (MapTile[] tilesX in island.tiles)
                    {
                        foreach (MapTile tile in tilesX)
                        {
                            if (tile != null && tile.texture != null)
                            {
                                spriteBatch.Draw(tile.texture, new Rectangle(tile.x * tileSize + island.IslandOriginX * tileSize, tile.y * tileSize + island.IslandOriginY * tileSize, tileSize, tileSize), Color.White);
                            }
                            y++;
                        }
                        x++;
                    }
                    foreach (MapTileDetails tile_details in island.TileDetails)
                    {
                        if (tile_details != null && tile_details.texture != null)
                        {
                            spriteBatch.Draw(tile_details.texture, new Rectangle(tile_details.x * tileSize + island.IslandOriginX * tileSize, tile_details.y * tileSize + island.IslandOriginY * tileSize, tileSize, tileSize), Color.White);
                        }
                    }
                }

                spriteBatch.Draw(playerTexture, new Rectangle((int)playerPositionX,(int)playerPositionY, 40, 80), player.getDrawFrame(), Color.White);

                spriteBatch.End();
            }
            

            base.Draw(gameTime);
        }
    }
}
