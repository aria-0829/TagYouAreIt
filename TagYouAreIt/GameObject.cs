using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace TagYouAreIt
{
    internal class GameObject
    {
        public Model fbxModel;
        public Vector3 DiffuseColor { get; set; }
        public Vector3 Translation { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix World = Matrix.Identity;
        public int Speed { get; set; }

        public GameObject(ContentManager content)
        {
            fbxModel = null;
            DiffuseColor = Vector3.One;
            Translation = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            World = Matrix.Identity;
            Speed = 0;
        }

        public virtual void DrawGameObject(Matrix view, Matrix projection)
        {
            World = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Translation);
            
            foreach (ModelMesh mesh in fbxModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.DiffuseColor = DiffuseColor;
                    effect.World = World;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}
