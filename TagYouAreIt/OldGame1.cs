using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TagYouAreIt
{
    public class OldGame1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        Model sphere;
        Model cube;
        private List<Model> cubes;
        private List<Vector3> distances;
        private List<Vector3> directions;
        private List<Vector3> cubePoss;
        private List<Vector3> cubeRotations;
        private List<bool> cubeIsTagged;
        private int taggedNum = 0;
        private float deltaTime;
        //private List<float> elapsedTimes;
        private Matrix world = Matrix.Identity;
        private Matrix view = Matrix.Identity;
        private Matrix projection = Matrix.Identity;
        private Matrix spherePos = Matrix.Identity;

        public OldGame1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 700;
            _graphics.PreferredBackBufferHeight = 700;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            spherePos = Matrix.CreateTranslation(new Vector3(0, 0, 0));

            world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            view = Matrix.CreateLookAt(new Vector3(0, 40, 10), new Vector3(0, 0, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), _graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            cubes = new List<Model>();
            cubePoss = new List<Vector3>();
            cubeRotations = new List<Vector3>();
            distances = new List<Vector3>();
            directions = new List<Vector3>();
            cubeIsTagged = new List<bool>();
            //elapsedTimes = new List<float>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Arial12");
            sphere = Content.Load<Model>("Sphere");
            cube = Content.Load<Model>("Cube");

            for (int i = 0; i < 10; i++)
            {
                cubes.Add(cube);
                cubePoss.Add(new Vector3(0, 0, 0));
                cubeRotations.Add(new Vector3(0, 0, 0));
                distances.Add(new Vector3(0, 0, 0));
                directions.Add(new Vector3(0, 0, 0));
                cubeIsTagged.Add(false);
                //elapsedTimes.Add(0f);

                cubePoss[i] = new Vector3(RandomNumber(-5, 5), 0, RandomNumber(-5, 5));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            MoveSphere();
            UpdateCubePositions();
            UpdateCubeRotations();
            CheckCollisions();

            base.Update(gameTime);
        }

        private void MoveSphere()
        {
            int sphereSpeed = 10;
            Vector3 sphereMoveAmount = Vector3.Zero;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
            {
                sphereMoveAmount.Z = -1* sphereSpeed * deltaTime;
            }
            if (state.IsKeyDown(Keys.S))
            {
                sphereMoveAmount.Z = sphereSpeed * deltaTime;
            }
            if (state.IsKeyDown(Keys.A))
            {
                sphereMoveAmount.X = -1 * sphereSpeed * deltaTime;
            }
            if (state.IsKeyDown(Keys.D))
            {
                sphereMoveAmount.X = sphereSpeed * deltaTime;
            }

            spherePos.Translation += sphereMoveAmount;
        }

        private void UpdateCubePositions()
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                int cubeSpeed = 3;
                distances[i] = cubePoss[i] - spherePos.Translation;
                directions[i] = Vector3.Normalize(distances[i]);
                Vector3 cubeMovement = cubeSpeed * directions[i] * deltaTime;

                //Keep the distance to the sphere
                if (distances[i].Length() > 11f)
                {
                    cubePoss[i] -= cubeMovement; //Move closer to the sphere
                }
                if (distances[i].Length() < 10f)
                {
                    cubePoss[i] += cubeMovement; //Move away from the sphere
                }
            }
        }

        private void UpdateCubeRotations()
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                directions[i] = spherePos.Translation - cubePoss[i];
                directions[i].Normalize();
                cubeRotations[i] = new Vector3(0, -(float)Math.Atan2(directions[i].Z, directions[i].X), 0);
            }
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                float distance = Vector3.Distance(cubePoss[i], spherePos.Translation);
                if (distance < 1.0f) //Tag cube when collide
                {
                    if (cubeIsTagged[i] == false)
                    {
                        taggedNum++;
                    }
                    cubeIsTagged[i] = true;
                }

                /*if (cubeIsTagged[i] == true)
                {
                    elapsedTimes[i] += deltaTime;

                    //Change tag back after 3 seconds
                    if (elapsedTimes[i] >= 3f)
                    {
                        cubeIsTagged[i] = false;
                        taggedNum--;
                        elapsedTimes[i] = 0f; //Reset timer
                    }
                }*/
            }
        }

        int RandomNumber(int min, int max)
        {
            Random random = new Random();
            int randomNumber = random.Next(min, max);
            return randomNumber;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Render cubes
            for (int i = 0; i < cubes.Count; i++)
            {
                foreach (ModelMesh mesh in cubes[i].Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World = Matrix.CreateFromYawPitchRoll(cubeRotations[i].Y, cubeRotations[i].X, cubeRotations[i].Z) * Matrix.CreateTranslation(cubePoss[i]);
                        effect.View = view;
                        effect.Projection = projection;

                        //Change cube color if tagged
                        if (cubeIsTagged[i])
                        {
                            effect.DiffuseColor = new Vector3(1, 0, 0);
                        }
                        else
                        {
                            effect.DiffuseColor = new Vector3(0, 1, 0);
                        }
                    }
                    mesh.Draw();
                }
            }

            //Render sphere
            foreach (ModelMesh mesh in sphere.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.DiffuseColor = new Vector3(1, 1, 1);
                    effect.World = spherePos;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }

            //Render text
            _spriteBatch.Begin(); 
            string output = "Tagged: " + taggedNum;
            Vector2 fontOrigin = _font.MeasureString(output) / 2; 
            Vector2 fontPos = new Vector2((GraphicsDevice.Viewport.Width - 100), 10); 
            _spriteBatch.DrawString(_font, output, fontPos, Color.OrangeRed);
            _spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}