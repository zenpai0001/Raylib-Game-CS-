using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


//Checks for collision between two collidable objects. Shared logic for all collision types.
public interface ICollidable<T>
{
    public Vector2 Position { get; set; }

    public float Radius { get; set; }

    public Rectangle Target { get; set; }

    /// <summary>
    /// Detecting collision between two collidable objects.
    /// This method will be called in the main game loop, and will check for collisions between all collidable objects in the game. 
    /// It will use Raylib's built-in collision detection methods to check for collisions.
    /// </summary>
    /// <param name="collidable">Collidable object representing any object inheriting ICollidable</param>
    public virtual void CollisionDetection(ICollidable<T> collidable)
    {
        //This is where the collision detection logic will go.

        if (Raylib.CheckCollisionCircles(Position, Radius, collidable.Position, collidable.Radius))
        {
            // Handle circle-circle collision
            Raylib.DrawText("Circle-Circle Collision Detected!", 10, 10, 20, Color.Red);
        }
        else if (Raylib.CheckCollisionRecs(Target, collidable.Target))
        {
            // Handle rectangle-rectangle collision
            Raylib.DrawText("Rectangle Collision Detected!", 10, 10, 20, Color.Red);
        }
        else if (Raylib.CheckCollisionCircleRec(Position, Radius, collidable.Target))
        {
            // Handle circle-rectangle collision
            Raylib.DrawText("Circle-Rectangle Collision Detected!", 10, 10, 20, Color.Red);
        }
    }
}