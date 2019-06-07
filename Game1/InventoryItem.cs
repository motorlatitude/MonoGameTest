using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class InventoryItem
    {
        public string name;
        public int amount;

        public Texture2D Texture;

        public InventoryItem(string inventory_name, Texture2D item_texture)
        {
            name = inventory_name;
            amount = 0;
            Texture = item_texture;
        }

    }
}
