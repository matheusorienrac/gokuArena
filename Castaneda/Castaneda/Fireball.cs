using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Castaneda
{
    class Fireball : Sprite
    {
        public const int MAX_DISTANCE = 400;
        protected float sayan; // Pra saber em que forma do sayan estamos

        protected Vector2 aux3; // Gambiarra para subir a posição dos tiros quando ficam grandes demais nos sayans 1+
        protected Vector2 mStartPosition;
        protected Vector2 mSpeed;
        protected Vector2 mDirection;
        //Propriedades
        public float Sayan
        {
            get { return sayan; }
            set { sayan = value; }
        }
        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "Kamehameha");
            Scale = 1.5f * sayan + 1.5f;
        }
        public Fireball(int mFrames, int mHeight, int mWidth, float mInterval, string mAssetName, int mHeightStart, int mWidthStart, float mSayan)
            : base(mFrames, mHeight, mWidth, mInterval, mAssetName, mHeightStart, mWidthStart)
        {
            frames = mFrames;
            AssetName = mAssetName;
            frameHeight = mHeight;
            frameWidth = mWidth;
            interval = mInterval;
            heightStart = mHeightStart;
            widthStart = mWidthStart;
            sayan = mSayan;
        
        }
        public void Update(GameTime theGameTime)
        {
            if (Vector2.Distance(mStartPosition, Position) > MAX_DISTANCE) //|| Position.X < 128 || Position.X > 836 || Position.Y < 150 || Position.Y > 490)
            {
                Visible = false;
            }

            if (Visible == true)
            {
                base.Update(theGameTime, mSpeed, mDirection);
            }
        }
        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {

            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed;
            mDirection = theDirection;
            Visible = true;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {  
                // aTUALIZA A SCALE
                Scale = 0.5f * sayan + 1.5f;
                base.Draw(theSpriteBatch);      


            }
        }
    }


}
