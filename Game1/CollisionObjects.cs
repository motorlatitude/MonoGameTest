using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class CollisionObjects
    {

        public List<CollisionObject> Objects;

        public CollisionObjects()
        {
            Objects = new List<CollisionObject>();
        }

        public void AddCollisionObject(CollisionObject obj)
        {
            Objects.Add(obj);
        }

        public void RemoveCollisionObject(CollisionObject obj)
        {
            Objects.Remove(obj);
        }

        public void RemoveCollisionObjectsInGroup(string group_name)
        {
            Objects.RemoveAll(obj => obj.group == group_name);
        }

        public bool CheckIfCollidingWithObject(int object_x, int object_y, int object_width, int object_height)
        {
            bool isColliding = false;
            foreach (CollisionObject obj in Objects)
            {
                bool tempIsColliding = obj.CheckCollision(object_x, object_y, object_width, object_height);
                if (tempIsColliding)
                    isColliding = tempIsColliding;
                
            }
            return isColliding;
        }

    }
}
