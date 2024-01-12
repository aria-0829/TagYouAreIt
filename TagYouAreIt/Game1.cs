using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TagYouAreIt
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private List<Cube> cubes;
        Sphere sphere;

        private int taggedNum = 0;
        private float deltaTime;
        private Matrix view = Matrix.Identity;
        private Matrix projection = Matrix.Identity;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 700;
            _graphics.PreferredBackBufferHeight = 700;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            view = Matrix.CreateLookAt(new Vector3(0, 40, 10), new Vector3(0, 0, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), _graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            cubes = new List<Cube>();
            sphere = new Sphere(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Arial12");

            for (int i = 0; i < 10; i++)
            {
                cubes.Add(new Cube(Content));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Update sphere
            sphere.Move(deltaTime);

            //Update cubes
            foreach (Cube cube in cubes)
            {
                cube.Distance = cube.Translation - sphere.Translation;
                cube.Update(deltaTime);

                //Check collisions
                if (cube.IsTagged == false)
                {
                    if (cube.Distance.Length() < 1.0f) //Tag cube when collide
                    {
                        taggedNum++;
                        cube.IsTagged = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Render cubes
            foreach (Cube cube in cubes)
            {
                cube.DrawGameObject(view, projection);
            }

            //Render sphere
            sphere.DrawGameObject(view, projection);

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