using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class ItemDrop
    {
        public InventoryItem Item;
        public float X;
        public float Y;
        public float FinalX;
        public float FinalY;
        public bool remove;

        public ItemDrop(float x, float y, float final_x, float final_y, InventoryItem item)
        {
            X = x;
            Y = y;
            FinalX = final_x;
            FinalY = final_y;
            Item = item;
            remove = false;
        }

    }
}