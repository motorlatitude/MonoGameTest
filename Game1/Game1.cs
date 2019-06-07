using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        SpriteFont defaultFont;

        float CameraOriginX = 600f;
        float CameraOriginY = 600f;

        float fps = 0f;

        Texture2D progressBarRedTexture;
        Texture2D resourceHighlightTexture;
        Texture2D MouseCursorTexture;
        float MousePositionX = 0f;
        float MousePositionY = 0f;

        Inventory PlayerInventory;

        List<ItemDrop> ItemDrops;

        Character player;
        Texture2D playerTexture;
        float playerPositionX = 600f;
        float playerPositionY = 600f;
        float playerSpeed = 300f;

        int tileSize = 80;

        Texture2D bridgeTexture;
        Texture2D fishNetTexture;

        //item textures
        Texture2D IronOreNuggetTexture;
        Texture2D WoodNuggetTexture;
        Texture2D NuggetShadowTexture;
        Texture2D CoalTexture;
        Texture2D BerriesTexture;
        Texture2D StoneTexture;

        Song PickUpSound;
        Song ChoppingWood;

        bool drawCollisions = false;

        bool isMouseDown = false;
        bool MouseClicked = false;

        bool buildMode = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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
            PlayerInventory = new Inventory();
            ItemDrops = new List<ItemDrop>();
            //this.IsMouseVisible = true;
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

            defaultFont = Content.Load<SpriteFont>("default");

            playerTexture = Content.Load<Texture2D>("player");
            MouseCursorTexture = Content.Load<Texture2D>("cursor");

            progressBarRedTexture = Content.Load<Texture2D>("progressBarRed");

            resourceHighlightTexture = Content.Load<Texture2D>("res_highlight");

            IronOreNuggetTexture = Content.Load<Texture2D>("iron_ore_nugget");
            WoodNuggetTexture = Content.Load<Texture2D>("wood_nugget");
            NuggetShadowTexture = Content.Load<Texture2D>("nugget_shadow");
            CoalTexture = Content.Load<Texture2D>("coal");
            BerriesTexture = Content.Load<Texture2D>("berries");
            StoneTexture = Content.Load<Texture2D>("stone");

            bridgeTexture = Content.Load<Texture2D>("bridge");
            fishNetTexture = Content.Load<Texture2D>("fishing_net");

            PickUpSound = Content.Load<Song>("pickup");
            ChoppingWood = Content.Load<Song>("chopping_wood");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;
        public double elapsedTime = 0;
        public double oceanFrameElapsedTime = 0;
        public int oceanFrameIndex = 0;
        public int resource_progress = 68;
        public Texture2D BuildModeTexture;
        public string BuildModeBuilding;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                buildMode = false;


            fps = (1 / (float)gameTime.ElapsedGameTime.TotalSeconds);

            var mstate = Mouse.GetState();
            MousePositionX = mstate.Position.X;
            MousePositionY = mstate.Position.Y;
            MouseClicked = false;
            
            if(mstate.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed && !isMouseDown)
            {
                MouseClicked = true;
            }
            else if (mstate.LeftButton == ButtonState.Pressed)
            {
                isMouseDown = true;
            }
            else if(mstate.LeftButton == ButtonState.Released)
            {
                isMouseDown = false;
            }


            if (gameTime.ElapsedGameTime.TotalSeconds == 0)
            {
                islands = new Island[2];
                Island main_island = new Island(Content, CollisionManager);
                main_island.GenerateIsland(16, 0, 0, tileSize, "main");
                main_island.GenerateNewResource(10);
                islands[0] = main_island;
                Island second_island = new Island(Content, CollisionManager);
                second_island.GenerateIsland(16, 16, 0, tileSize, "right");
                islands[1] = second_island;
                GeneratePlayer();

            }
            else
            {
                frameElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                float frameUpdateTime = 1f / 8;
                if (frameElapsedTime > frameUpdateTime)
                {
                    frameElapsedTime -= frameUpdateTime;

                    if (frameIndex < 2)
                        frameIndex++;
                    else
                        frameIndex = 0;
                }

                oceanFrameElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                float oceanFrameUpdateTime = 1f / 6;
                if (oceanFrameElapsedTime > oceanFrameUpdateTime)
                {
                    oceanFrameElapsedTime -= frameUpdateTime;

                    if (oceanFrameIndex < 4)
                        oceanFrameIndex++;
                    else
                        oceanFrameIndex = 0;
                }
                player.animationFrameUpdate(gameTime);
                if ((gameTime.TotalGameTime.TotalSeconds - elapsedTime) > 2)
                {
                    foreach(Island ilnd in islands)
                    {
                        ilnd.GenerateNewResource(); //generate new resource every 2 seconds
                    }
                    elapsedTime = gameTime.TotalGameTime.TotalSeconds;
                }

                var kstate = Keyboard.GetState();
                Island main_island = islands[0];
                if (kstate.IsKeyDown(Keys.F5) && !previousKeyboardState.IsKeyDown(Keys.F5))
                {
                    CollisionManager.RemoveCollisionObjectsInGroup("main"); //make sure to generate new collision map for this island and get rid of the old one
                    main_island.GenerateIsland(16, 0, 0, tileSize, "main");
                }

                if (kstate.IsKeyDown(Keys.F1) && !previousKeyboardState.IsKeyDown(Keys.F1))
                {
                    drawCollisions = drawCollisions ? false : true;
                }

                if (kstate.IsKeyDown(Keys.B) && !previousKeyboardState.IsKeyDown(Keys.B))
                {
                    buildMode = buildMode ? false : true;
                }

                if (kstate.IsKeyDown(Keys.N) && !previousKeyboardState.IsKeyDown(Keys.N))
                {
                    if (buildMode)
                    {
                        BuildModeBuilding = "bridge";
                        BuildModeTexture = bridgeTexture;
                    }
                }
                if (kstate.IsKeyDown(Keys.M) && !previousKeyboardState.IsKeyDown(Keys.M))
                {
                    if (buildMode)
                    {
                        BuildModeBuilding = "fishing_net";
                        BuildModeTexture = fishNetTexture;
                    }
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

                bool collision = CollisionManager.CheckIfCollidingWithObject((int)playerPositionX + 10, (int)playerPositionY + 60, 20, 20);

                if (collision)
                {
                    //dont move
                    playerPositionX = org_playerPositionX;
                    CameraOriginX = org_CameraPositionX;
                    playerPositionY = org_playerPositionY;
                    CameraOriginY = org_CameraPositionY;
                }
                previousKeyboardState = kstate;
                previousMouseState = mstate;
            }


            base.Update(gameTime);
        }

        public double frameElapsedTime = 0;
        public double et = 0;
        public int frameIndex = 0;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(46, 149, 255, 1));

            // TODO: Add your drawing code here
            List<SpriteFrame> Sprites = new List<SpriteFrame>();
            Sprite ResourceHighlightSprite = null;
            IslandResource ActiveIslandResource = null;
            Island main_island = islands[0];
            Random rnd = new Random();
            if (main_island.tiles.Length > 0)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation((int)CameraOriginX - 600, (int)CameraOriginY - 600, 0)); //move viewport / camera
                Rectangle player_extended_rectangle = new Rectangle((int)playerPositionX - tileSize, (int)playerPositionY - tileSize, tileSize * 3, tileSize * 3); //active player area i.e. within the player's reach 3x3
                Rectangle player_rectangle = new Rectangle((int)playerPositionX, (int)playerPositionY, tileSize, tileSize);
                foreach (Island island in islands)
                {
                    int x = 0;
                    int y = 0;
                    foreach (MapTile[] tilesX in island.tiles)
                    {
                        foreach (MapTile tile in tilesX)
                        {
                            if (tile != null && tile.texture != null)
                            {
                                Rectangle tileRectangle = new Rectangle(tile.x * tileSize + island.IslandOriginX * tileSize, tile.y * tileSize + island.IslandOriginY * tileSize, tileSize, tileSize);
                                spriteBatch.Draw(tile.texture, tileRectangle, Color.White);
                                Point mv = new Point(((int)MousePositionX - ((int)CameraOriginX - 600)), ((int)MousePositionY - ((int)CameraOriginY - 600)));
                                if (tileRectangle.Contains(mv) && buildMode && BuildModeBuilding != null)
                                {
                                    if (isMouseDown && player_extended_rectangle.Contains(mv))
                                    {
                                        //place building
                                        island.AddBuilding(tile.x, tile.y, BuildModeBuilding);
                                    }
                                    else
                                    {
                                        //draw current build item in nearest square to cursor
                                        SpriteFrame BuildFrame = new SpriteFrame();
                                        if (player_extended_rectangle.Contains(mv))
                                            BuildFrame.AddSprite(new Sprite(BuildModeTexture, tileRectangle, Color.White * 0.5f, new Rectangle(0, 0, 256, 256)));
                                        else
                                            BuildFrame.AddSprite(new Sprite(BuildModeTexture, tileRectangle, Color.White * 0.2f, new Rectangle(0, 0, 256, 256)));
                                        Sprites.Add(BuildFrame);
                                    }
                                }
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
                    foreach (IslandBuilding building in island.IslandBuildings)
                    {
                        if (building != null && building.BuildingTexture != null)
                        {
                            int fri = oceanFrameIndex;
                            if(building.X % 2 == 0)
                            {
                                fri++;
                                if (fri > 4)
                                    fri = 0;
                            }
                            else if (building.X % 3 == 0)
                            {
                                fri += 2;
                                if (fri > 4)
                                    fri = 0;
                            }
                            else if (building.X % 4 == 0)
                            {
                                fri += 3;
                                if (fri > 4)
                                    fri = 0;
                            }
                            spriteBatch.Draw(building.BuildingTexture, new Rectangle(building.X * tileSize + island.IslandOriginX * tileSize, building.Y * tileSize + island.IslandOriginY * tileSize, tileSize, tileSize), new Rectangle(fri * 256, 0, 256, 256), Color.White);
                        }
                    }
                    IslandResource removeResource = null;
                    foreach (IslandResource res in island.IslandResources)
                    {
                        if (res != null && res.texture != null)
                        {
                            Rectangle res_rectangle = new Rectangle(res.x * tileSize + island.IslandOriginX * tileSize + ((tileSize / 2) - (res.width/2)), res.y * tileSize + island.IslandOriginY * tileSize + (tileSize - res.height), res.width, res.height);
                            Rectangle mouse_res_rectangle = new Rectangle(res.x * tileSize + island.IslandOriginX * tileSize, res.y * tileSize + island.IslandOriginY * tileSize, tileSize, tileSize);
                            SpriteFrame fr = new SpriteFrame();
                            Point mv = new Point(((int)MousePositionX - ((int)CameraOriginX - 600)), ((int)MousePositionY - ((int)CameraOriginY - 600)));
                            if (mouse_res_rectangle.Contains(mv) && player_extended_rectangle.Contains(mouse_res_rectangle) && !buildMode)
                            {
                                ResourceHighlightSprite = new Sprite(resourceHighlightTexture, new Rectangle(res.x * tileSize + island.IslandOriginX * tileSize, res.y * tileSize + island.IslandOriginY * tileSize, tileSize, tileSize), Color.White);
                                fr.AddSprite(new Sprite(ResourceHighlightSprite.texture, ResourceHighlightSprite.destinationRectangle, ResourceHighlightSprite.color, new Rectangle(frameIndex * 256, 0, 256, 256)));
                                ActiveIslandResource = res;
                                if (res.progress <= 0)
                                {
                                    //explode and pick up items
                                    float startX = res.x * tileSize + island.IslandOriginX * tileSize;
                                    float startY = res.y * tileSize + island.IslandOriginY * tileSize;
                                    resource_progress = tileSize - 12;
                                    if (res.type == "iron_ore")
                                    {
                                        int amount = rnd.Next(1, 5);
                                        for(int a = 0; a < amount; a++)
                                        {
                                            ItemDrops.Add(new ItemDrop(startX, startY, (startX + (rnd.Next(-tileSize, tileSize))), (startY + (rnd.Next(-tileSize, tileSize))), new InventoryItem("iron_ore", IronOreNuggetTexture)));
                                        }
                                    }
                                    else if (res.type == "tree")
                                    {
                                        int amount = rnd.Next(3, 6);
                                        for (int a = 0; a < amount; a++)
                                        {
                                            ItemDrops.Add(new ItemDrop(startX, startY, (startX + (rnd.Next(-tileSize, tileSize))), (startY + (rnd.Next(-tileSize, tileSize))), new InventoryItem("wood", WoodNuggetTexture)));
                                        }
                                    }
                                    else if (res.type == "coal_ore")
                                    {
                                        int amount = rnd.Next(3, 8);
                                        for (int a = 0; a < amount; a++)
                                        {
                                            ItemDrops.Add(new ItemDrop(startX, startY, (startX + (rnd.Next(-tileSize, tileSize))), (startY + (rnd.Next(-tileSize, tileSize))), new InventoryItem("coal", CoalTexture)));
                                        }
                                    }
                                    else if (res.type == "berry_bush")
                                    {
                                        int amount = rnd.Next(2, 3);
                                        for (int a = 0; a < amount; a++)
                                        {
                                            ItemDrops.Add(new ItemDrop(startX, startY, (startX + (rnd.Next(-tileSize, tileSize))), (startY + (rnd.Next(-tileSize, tileSize))), new InventoryItem("berries", BerriesTexture)));
                                        }
                                    }
                                    else if (res.type == "rock")
                                    {
                                        int amount = rnd.Next(2, 3);
                                        for (int a = 0; a < amount; a++)
                                        {
                                            ItemDrops.Add(new ItemDrop(startX, startY, (startX + (rnd.Next(-tileSize, tileSize))), (startY + (rnd.Next(-tileSize, tileSize))), new InventoryItem("stone", StoneTexture)));
                                        }
                                    }
                                    CollisionManager.RemoveCollisionObject(res.collision_object);
                                    removeResource = res;
                                }
                            }
                            fr.AddSprite(new Sprite(res.texture, res_rectangle, Color.White));
                            Sprites.Add(fr);
                            if (res.progress != 68)
                            {
                                //this block has been hit, permanentely render bar
                                Texture2D rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
                                Color[] data = new Color[1];
                                data[0] = Color.Chocolate;
                                rect.SetData(data);
                                SpriteFrame ProgressbarFrame = new SpriteFrame();
                                ProgressbarFrame.AddSprite(new Sprite(rect, new Rectangle(res.x * tileSize + island.IslandOriginX * tileSize, res.y * tileSize + island.IslandOriginY * tileSize + tileSize, tileSize, 26), Color.Black));
                                ProgressbarFrame.AddSprite(new Sprite(progressBarRedTexture, new Rectangle(res.x * tileSize + island.IslandOriginX * tileSize + 6, res.y * tileSize + island.IslandOriginY * tileSize + tileSize + 6, res.progress, 14), Color.White));
                                Sprites.Add(ProgressbarFrame);
                            }
                        }
                    }
                    if (removeResource != null)
                        island.IslandResources.Remove(removeResource);
                }
                float ItemSpeed = 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (ItemDrop item in ItemDrops)
                {
                    if ((item.X > item.FinalX - ItemSpeed && item.X < item.FinalX + ItemSpeed && item.Y > item.FinalY - ItemSpeed && item.Y < item.FinalY + ItemSpeed) || (item.FinalX == 0 && item.FinalY == 0))
                    {
                        item.FinalX = 0;
                        item.FinalY = 0;
                        if (player_extended_rectangle.Contains(new Rectangle((int)item.X, (int)item.Y, 80, 80)))
                        {
                            if((item.X + ItemSpeed) > (int)playerPositionX)
                            {
                                item.X = (int)playerPositionX;
                            }
                            else
                            {
                                if ((int)playerPositionX > item.X)
                                {
                                    item.X += ItemSpeed;
                                }
                                else
                                {
                                    item.X -= ItemSpeed;
                                }
                            }
                            if ((item.Y + ItemSpeed) > (int)playerPositionY)
                            {
                                item.Y = (int)playerPositionY;
                            }
                            else
                            {
                                if ((int)playerPositionY > item.Y)
                                {
                                    item.Y += ItemSpeed;
                                }
                                else
                                {
                                    item.Y -= ItemSpeed;
                                }
                            }
                        }
                        if (player_rectangle.Contains(new Rectangle((int)item.X, (int)item.Y, 80, 80)))
                        {
                            PlayerInventory.AddItemToInventory(item.Item, 1); //pickup
                            MediaPlayer.Volume = 0.3f;
                            MediaPlayer.Play(PickUpSound);
                            item.remove = true;
                        }
                        else
                        {
                            SpriteFrame dropped_item_frame = new SpriteFrame();
                            dropped_item_frame.AddSprite(new Sprite(NuggetShadowTexture, new Rectangle((int)item.X, (int)item.Y + 20, 80, 80), Color.White * 0.5f));
                            dropped_item_frame.AddSprite(new Sprite(item.Item.Texture, new Rectangle((int)item.X, (int)item.Y, 80, 80), Color.White));
                            Sprites.Add(dropped_item_frame);
                        }
                    }
                    else
                    {
                        if((item.X + ItemSpeed) <= item.FinalX)
                        {
                            item.X = item.FinalX;
                        }
                        else
                        {
                            if (item.FinalX > item.X)
                            {
                                item.X += ItemSpeed;
                            }
                            else if ((item.X - ItemSpeed) <= item.FinalX)
                            {
                                item.X = item.FinalX;
                            }
                            else
                            {
                                item.X -= ItemSpeed;
                            }
                        }
                        if ((item.Y + ItemSpeed) <= item.FinalY)
                        {
                            item.Y = item.FinalY;
                        }
                        else
                        {
                            if (item.FinalY > item.Y)
                            {
                                item.Y += ItemSpeed;
                            }
                            else if((item.Y - ItemSpeed) <= item.FinalY)
                            {
                                item.Y = item.FinalY;
                            }
                            else
                            {
                                item.Y -= ItemSpeed;
                            }
                        }
                        SpriteFrame dropped_item_frame = new SpriteFrame();
                        dropped_item_frame.AddSprite(new Sprite(NuggetShadowTexture, new Rectangle((int)item.X, (int)item.Y + 20, 64, 64), Color.White * 0.5f));
                        dropped_item_frame.AddSprite(new Sprite(item.Item.Texture, new Rectangle((int)item.X, (int)item.Y, 64, 64), Color.White));
                        Sprites.Add(dropped_item_frame);
                    }
                }
                ItemDrops.RemoveAll((itm) => itm.remove == true);
                SpriteFrame PlayerFrame = new SpriteFrame();
                PlayerFrame.AddSprite(new Sprite(playerTexture, new Rectangle((int)playerPositionX, (int)playerPositionY, 40, 80), Color.White, player.getDrawFrame()));
                Sprites.Add(PlayerFrame);

                Sprites.Sort(
                    delegate (SpriteFrame obj, SpriteFrame obj2)
                    {
                        if (obj != null && obj2 != null)
                        {
                            return (obj.frames.ToArray()[0].destinationRectangle.Y + obj.frames.ToArray()[0].destinationRectangle.Height).CompareTo((obj2.frames.ToArray()[0].destinationRectangle.Y + obj2.frames.ToArray()[0].destinationRectangle.Height));
                        }
                        else
                        {
                            return 0;
                        }
                    }
                );

                foreach (SpriteFrame s in Sprites)
                {
                    if (s != null && s.frames != null)
                    {
                        foreach (Sprite sp in s.frames)
                        {
                            if (sp.sourceRectangle != null)
                            {
                                spriteBatch.Draw(sp.texture, sp.destinationRectangle, sp.sourceRectangle, sp.color);
                            }
                            else
                            {
                                spriteBatch.Draw(sp.texture, sp.destinationRectangle, sp.color);
                            }
                        }
                    }
                }

                if (ResourceHighlightSprite != null && ActiveIslandResource != null && !buildMode)
                {
                    if (isMouseDown || MouseClicked)
                    {
                        Texture2D rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
                        Color[] data = new Color[1];
                        data[0] = Color.White;
                        rect.SetData(data);
                        spriteBatch.Draw(rect, new Rectangle(ResourceHighlightSprite.destinationRectangle.X, ResourceHighlightSprite.destinationRectangle.Y + tileSize, tileSize, 26), Color.Black);
                        spriteBatch.Draw(rect, new Rectangle(ResourceHighlightSprite.destinationRectangle.X + 6, ResourceHighlightSprite.destinationRectangle.Y + tileSize + 6, ActiveIslandResource.animatedProgress, 14), Color.White);
                        spriteBatch.Draw(progressBarRedTexture, new Rectangle(ResourceHighlightSprite.destinationRectangle.X + 6, ResourceHighlightSprite.destinationRectangle.Y + tileSize + 6, ActiveIslandResource.progress, 14), Color.White);
                        if ((gameTime.TotalGameTime.TotalSeconds - et) > ActiveIslandResource.HitDelay || MouseClicked)
                        {
                            ActiveIslandResource.progress -= ActiveIslandResource.HitAmount;
                            et = gameTime.TotalGameTime.TotalSeconds;
                        }
                        if(ActiveIslandResource.animatedProgress > ActiveIslandResource.progress)
                        {
                            ActiveIslandResource.animatedProgress -= ActiveIslandResource.AnimatedHitAmount;
                        }
                    }
                }

                //Draw collision areas
                if (drawCollisions)
                {
                    Texture2D rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
                    Color[] data = new Color[1];
                    data[0] = Color.Chocolate;
                    rect.SetData(data);

                    foreach (CollisionObject obj in CollisionManager.Objects)
                    {
                        spriteBatch.Draw(rect, new Rectangle((int)obj.x, (int)obj.y, obj.width, obj.height), Color.Red * 0.5f);
                    }
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0)); //move viewport / camera
                spriteBatch.DrawString(defaultFont, Math.Round(fps, 1) + " FPS", new Vector2(0, 0), new Color(0, 255, 0, 1));
                spriteBatch.DrawString(defaultFont, "BuildMode: "+buildMode, new Vector2(0, 20), new Color(0, 255, 0, 1));
                int k = 0;
                if (PlayerInventory != null && PlayerInventory.Items != null)
                {
                    foreach (InventoryItem i in PlayerInventory.Items)
                    {
                        spriteBatch.DrawString(defaultFont, i.amount + " " + i.name, new Vector2(0, 40 + k * 20), new Color(0, 255, 0, 1));
                        k++;
                    }
                }

                spriteBatch.Draw(MouseCursorTexture, new Rectangle((int)MousePositionX, (int)MousePositionY, 20, 20), new Rectangle(0, 0, 64, 64), Color.White);

                spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}
