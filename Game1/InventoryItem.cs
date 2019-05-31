using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class InventoryItem
    {
        public string name;
        public int amount;

        public InventoryItem(string inventory_name)
        {
            name = inventory_name;
            amount = 0;
        }

    }
}
