using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Character
    {
        public bool isPlayer;
        public int x;
        public int y;

        private float timeElapsed;
        private Rectangle[] frames;
        private int frameIndex;
        private float frameUpdateTime;


        public Character()
        {
            x = 0;
            y = 0;
        }

        public void create(int character_x, int character_y, int fps, int numberOfFrames)
        {
            x = character_x;
            y = character_y;

            frameUpdateTime = (1f / fps);
            frames = new Rectangle[numberOfFrames];
        }

        public void addFrame(int index, Rectangle r)
        {
            frames[index] = r;
        }

        public void animationFrameUpdate(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > frameUpdateTime)
            {
                timeElapsed -= frameUpdateTime;

                if (frameIndex < frames.Length - 1)
                    frameIndex++;
                else
                    frameIndex = 0;
            }
        }

        public Rectangle getDrawFrame()
        {
            return frames[frameIndex];
        }
        
    }
}
