using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class MapTile
    {
        public int x;
        public int y;
        public string type;
        public bool border_left;
        public bool border_right;
        public bool border_top;
        public bool border_bottom;
        public bool ocean_border_left;
        public bool ocean_border_right;
        public bool ocean_border;

        public Texture2D texture;

        public MapTile(int x_pos, int y_pos, string tile_type)
        {
            x = x_pos;
            y = y_pos;
            type = tile_type;
        }
        
    }
}
