using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raylib_Game_CS_
{


        //Checks for collision between two collidable objects. Shared logic for all collision types.
        public interface ICollidable
        {
            public Vector2 Hitbox { get; set; }
            
            public float Radius { get; set; }

            public Rectangle hitbox1 { get; set; }
           public  Rectangle hitbox2 { get; set; }

        public void CollisionCheck(ICollidable collidable)
            {
                Rectangle hitbox1 = new Rectangle(Hitbox.X, Hitbox.Y, Radius * 2, Radius * 2);

                Rectangle hitbox2 = new Rectangle(collidable.Hitbox.X, collidable.Hitbox.Y, collidable.Radius * 2, collidable.Radius * 2);

                Raylib.CheckCollisionRecs(hitbox1, hitbox2);
            }
        }

    }
