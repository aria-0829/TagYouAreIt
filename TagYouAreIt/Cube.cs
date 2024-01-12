using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace TagYouAreIt
{
    internal class Cube : GameObject
    {
        public bool IsTagged { get; set; }
        public Vector3 Distance { get; set; }
        public Vector3 Direction { get; set; }
        private Random random = new Random();

        public Cube(ContentManager content) : base(content)
        {
            fbxModel = content.Load<Model>("Cube");
            DiffuseColor = new Vector3(0, 1, 0);
            Translation = new Vector3(random.Next(-5, 6), 0, random.Next(-5, 6)); //random.Next(-5, 6) is >= -5 and <= 5
            Speed = 3;

            //Unique to cube
            IsTagged = false;
            Distance = Vector3.Zero;
            Direction = Vector3.Zero;
        }

        public void Update(float deltaTime)
        {
            Direction = Vector3.Normalize(Distance); //Set the Direction
            Rotation = new Vector3(0, -(float)Math.Atan2(Direction.Z, Direction.X), 0); //Set the cube Rotation to face the sphere

            Vector3 cubeMovement = Speed * Direction * deltaTime;

            //Keep the distance to the sphere
            if (Distance.Length() > 11f)
            {
                Translation -= cubeMovement; //Move closer to the sphere
            }
            if (Distance.Length() < 10f)
            {
                Translation += cubeMovement; //Move away from the sphere
            }

            //Change to red if tagged
            if (IsTagged)
            {
                DiffuseColor = new Vector3(1, 0, 0);
            }
        }
    }
}
