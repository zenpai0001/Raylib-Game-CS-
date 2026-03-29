using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raylib_Game_CS_;

public struct Player
{
    //Player fields
    public float speed = 1.01f;

    public bool gravityEnabled = true;

    public float gravityStrength = 9.81f;

    public Vector2 playerPosition = new Vector2 (Raylib.GetScreenWidth()/2.0f, Raylib.GetScreenHeight()/2.0f);

    public Vector2 direction = Vector2.Zero;

    public Player(){
        //empty constructor for now, but I may need to add some initialization logic here later on.
        //It's to keep the compiler from complaining about the struct not having one.
    }
    public void Update()
    {
        //Updates the direction vector based on player input. I'll need to workout a few things.
        if (Raylib.IsKeyDown(KeyboardKey.W)) playerPosition.Y -= gravityStrength/2.0f;
        if (Raylib.IsKeyDown(KeyboardKey.A)) playerPosition.X -= 1.0f;
        if (Raylib.IsKeyDown(KeyboardKey.D)) playerPosition.X += 1.0f;

        //Moves the player based on the direction vector and speed.
        playerPosition += direction * speed * Raylib.GetFrameTime();

        if(gravityEnabled == true)
        {
            playerPosition.Y += gravityStrength * Raylib.GetFrameTime();
        }


        //Stops player from moving when keyboard input is released.
        if (Raylib.IsKeyDown(KeyboardKey.Null))
        {
            playerPosition = Vector2.Zero;
        }
       
    }
}
