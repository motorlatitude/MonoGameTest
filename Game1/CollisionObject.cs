using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class CollisionObject
    {

        public int x;
        public int y;
        public int width;
        public int height;
        public string type;

        private Rectangle CollisionObjectRectangle;

        public CollisionObject(int object_x, int object_y, int object_width, int object_height, string object_type = "default")
        {
            x = object_x;
            y = object_y;
            width = object_width;
            height = object_height;
            type = object_type;
            CollisionObjectRectangle = new Rectangle(x, y, width, height);
        }

        public bool CheckCollision(int object_x, int object_y, int object_width, int object_height)
        {
            return CollisionObjectRectangle.Intersects(new Rectangle(object_x, object_y, object_width, object_height));
        }



    }
}
