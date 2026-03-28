using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raylib_Game_C;

public struct Player
{
    //Player fields
    public float speed = 1.5f;

    public Vector2 position = new Vector2(10f,10f);

    public Vector2 direction = Vector2.Zero;

    public Player(){
        //empty constructor for now, but I may need to add some initialization logic here later on.
        //It's to keep the compiler from complaining about the struct not having one.
    }
    public void Update()
    {
        //Updates the direction vector based on player input. I'll need to workout a few things.
        if (Raylib.IsKeyDown(KeyboardKey.W)) position.Y -= 1.0f;
        if (Raylib.IsKeyDown(KeyboardKey.S)) position.Y += 1.0f;
        if (Raylib.IsKeyDown(KeyboardKey.A)) position.X -= 1.0f;
        if (Raylib.IsKeyDown(KeyboardKey.D)) position.X += 1.0f;
        if (Raylib.IsKeyDown(KeyboardKey.Space)) position.X += speed;

        //Moves the player based on the direction vector and speed.
        position += direction * speed * Raylib.GetFrameTime();

        //Stops player from moving when keyboard input is released.
        if (Raylib.IsKeyDown(KeyboardKey.Null))
        {
            position = Vector2.Zero;
        }
       
    }
}
