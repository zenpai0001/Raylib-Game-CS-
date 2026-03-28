using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raylib_Game_C_
{
    public class Collisions
    {
        public Collisions() { }

        //Checks for collision between two collidable objects. Shared logic for all collision types.
        public interface ICollidable
        {
            public Vector2 Hitbox { get; set; }
            public float radius { get; set; }

            public void CollisionCheck()
            {
                //Collision logic goes here.
                //For now, this is just a placeholder to show where the collision logic will go.
            }
        }

    }
}
