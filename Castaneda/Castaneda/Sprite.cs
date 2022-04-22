using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Castaneda
{
    class Sprite
    {
            // Gambiarra pro fireball desaparecer quando encosta no final da arena
        protected bool visible = false;

        // Cima = 1 Direita  = 2 Baixo = 3 Esquerda = 4
        protected int currentFrame = 0;
        protected int frameHeight;
        protected int frameWidth;
        protected int frames;
        protected float timer = 0;
        protected float interval;
        protected int frameY; // em que "andar" da imagem o estamos, pra saber qual direção ele ta andando
        protected int heightLimit;
        protected int widthLimit;
        protected string AssetName;
        protected int heightStart; // Onde começa a picar os sprites
        protected int widthStart;
        // Gambiarra pra posicioanr o player de volta quando sai da arena
        Vector2 aux;
        //O nome do arquivo com a textura


        public Sprite(int mFrames, int mHeight, int mWidth, float mInterval, string mAssetName, int mHeightStart, int mWidthStart)
        {
            frames = mFrames;
            AssetName = mAssetName;
            frameHeight = mHeight;
            frameWidth = mWidth;
            interval = mInterval;
            heightStart = mHeightStart;
            widthStart = mWidthStart;
        
        }
        //A area retangular da imagem original que voce quer cortar
        protected Rectangle mSource;
        // Propriedades of the hueragem
        public int WidthStart
        {
            get { return widthStart; }
            set { widthStart = value; }
        }
        public int HeightStart
        {
            get { return heightStart; }
            set { heightStart = value; }
        }
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }
        public float Scale
        {  //Quando a Scale for alterada, ele altera o resto pra ficarem de acordo com o novo tamanho
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        //Usados pra colisao
        public Rectangle Size;
        public Vector2 Position = new Vector2(0, 0); // Posição Atual do Sprite
        //Quanto deve ser diminuido ou aumentado do tamanho original do sprite
        private float mScale = 2.5f;


        // O Objeto de textura usado no sprite

        protected Texture2D mSpriteTexture;

        //Dar load na textura
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Source = new Rectangle(heightStart, widthStart, frameWidth ,frameHeight);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));// Voltar aqui depois pra arrumar o quadrado pra selecionar os do meio

        }
        //Atualiza a posição do sprite
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            aux = Position;
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            // Checa se ele saiu da area estipulada pra arenazinha
            if (Position.X < 128 || Position.X > 836)
            {
                Position = aux;
                Visible = false;
            }
            else
                if (Position.Y < 150 || Position.Y > 490)
                {
                    Position = aux;
                    Visible = false;
                }
            // Timer pra animação
            timer += (float)theGameTime.ElapsedGameTime.TotalSeconds * 1000;
            if(theDirection != Vector2.Zero)
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > frames - 1)
                    currentFrame = 0;
            }
            if (theDirection.X == -1)
                frameY = 3;
            else
            {
                if (theDirection.X == 1)
                    frameY = 1;
                else
                    if (theDirection.Y == -1)
                        frameY = 0;
                    else
                        if (theDirection.Y == 1)
                            frameY = 2;
            }              
            Source = new Rectangle(currentFrame * frameWidth , frameY * frameHeight, frameWidth, frameHeight);
        }

        //Desenhar na tela
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Source,
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
