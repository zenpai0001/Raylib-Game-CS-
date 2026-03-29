using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raylib_Game_CS_
{
    public class Collisions
    {
        public Collisions() { }

        //Checks for collision between two collidable objects. Shared logic for all collision types.
        public interface ICollidable
        {
            public Vector2 Hitbox { get; set; }

            public Vector2 Position { get; set; }
            public float Radius { get; set; }

            public void CollisionCheck(ICollidable collidable)
            {
                //Check collision between two rectangles. This is a placeholder for now, but I'll need to workout the logic for this later on.
                Raylib.CheckCollisionRecs (new Rectangle(Position.X, Position.Y, Hitbox.X, Hitbox.Y), new Rectangle(Position.X, Position.Y, Hitbox.X, Hitbox.Y));
            }
        }

    }
}
