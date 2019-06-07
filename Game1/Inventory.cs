using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    class Inventory
    {

        public List<InventoryItem> Items;

        public Inventory()
        {
            Items = new List<InventoryItem>();
        }

        public void AddItemToInventory(InventoryItem ii, int amount = 1)
        {
            if(Items.Exists((i) => i.name == ii.name))
            {
                InventoryItem item = Items.Find((i) => i.name == ii.name);
                item.amount += amount;
            }
            else
            {
                ii.amount += amount;
                Items.Add(ii);
            }
        }

    }
}
