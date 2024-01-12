using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TagYouAreIt
{
    internal class Sphere : GameObject
    {
        public Sphere(ContentManager content) : base(content)
        {
            fbxModel = content.Load<Model>("Sphere");
            DiffuseColor = new Vector3(1, 1, 1);
            Speed = 10;
        }

        public void Move(float deltaTime)
        {
            Vector3 sphereMoveAmount = Vector3.Zero;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
            {
                sphereMoveAmount.Z = -1 * Speed * deltaTime;
            }
            if (state.IsKeyDown(Keys.S))
            {
                sphereMoveAmount.Z = Speed * deltaTime;
            }
            if (state.IsKeyDown(Keys.A))
            {
                sphereMoveAmount.X = -1 * Speed * deltaTime;
            }
            if (state.IsKeyDown(Keys.D))
            {
                sphereMoveAmount.X = Speed * deltaTime;
            }

            Translation += sphereMoveAmount;
        }
    }
}
