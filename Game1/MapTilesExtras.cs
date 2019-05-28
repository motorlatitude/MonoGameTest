using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class MapTilesExtras
    {
        public int x;
        public int y;
        public Texture2D texture;

        public MapTilesExtras(int x_pos, int y_pos, Texture2D extra_texture)
        {
            x = x_pos;
            y = y_pos;
            texture = extra_texture;
        }
    }
}
