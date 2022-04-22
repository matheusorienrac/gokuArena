using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Castaneda
{
    class Monster : Sprite
    {
        SoundEffect blast;
        SoundEffectInstance blastInstance;
        ContentManager mContentManager;
        // constantes
        protected int START_POSITION_X = 250;
        protected int START_POSITION_X2 = 650; // Posição que ele tem que nascer caso o player esteja demais pra direita no mapa
        protected int START_POSITION_Y = 350;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        protected int aux;
        protected int round = 0;
        protected int reset = 0; // Quantas vezes o player chegou até o final da arena pros monstros ficarem mais fortes;
        protected int defense; // O quanto de dano ele absorve
        protected int health = 100;
        protected int speed = 111;
        protected int score;
        // Inicialização
        // Inicializando pra ele começar andando
        State mCurrentState = State.Walking;
        // Guarda a posi~ção que o viadinho tava quando começou a pular
        protected Vector2 mStartingPosition = Vector2.Zero;
        protected Vector2 mDirection = Vector2.Zero;
        protected Vector2 mSpeed = Vector2.Zero;
        protected KeyboardState mPreviousKeyboardState;
        protected Goku player;
        // Round é qual level o cara tá, pra mexer na dificuldade dos mobs


        // Propriedades
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }
        public int Reset
        {
            get { return reset; }
            set { reset = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int Round
        {
            get { return round; }
            set { round = value; }
        }
        public int Speed{
            get { return speed; }
            set { speed = value; }
        }
        enum State
        {
            Walking
        }
        public Monster(int mFrames, int mHeight, int mWidth, float mInterval, string mAssetName, Goku mPlayer, int mHeightStart, int mWidthStart)
            : base(mFrames, mHeight, mWidth, mInterval, mAssetName, mHeightStart, mWidthStart)
        {
            frames = mFrames;
            AssetName = mAssetName;
            frameHeight = mHeight;
            frameWidth = mWidth;
            interval = mInterval;
            player = mPlayer;
            widthStart = mWidthStart;
            heightStart = mHeightStart;
            round = 0;
            Score = 0;
            defense = 1;
   
        }
        public void Update(GameTime theGameTime)
        {

            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            UpdateDamage();
            base.Update(theGameTime, mSpeed, mDirection);                   
            Source = new Rectangle(currentFrame * frameWidth + WidthStart,frameY * frameHeight + HeightStart, frameWidth, frameHeight); // Aqui faz a transição entre monstros
        }
        private void UpdateDamage() //////////// DANO TOMADO PELOS MOBS E MUDANÇA ENTRE ELES
        {
            foreach (Fireball aFireball in player.mFireballs)// MUDAR AQUI AS COORDENADAS DA BOLA
            {
                if (Position.X + Size.Width/2 <= aFireball.Position.X + aFireball.Size.Right && Position.X + Size.Width/2 >=  aFireball.Position.X)
                {
                    if ((aFireball.Position.Y + Size.Height/2 >= Position.Y && aFireball.Position.Y + aFireball.Size.Height/2 <= Position.Y + Size.Height))
                        if (Health > 0)
                        {
                            if (player.Damage - Defense >= 0)
                                Health -= player.Damage - Defense;
                            blastInstance.Play();
                            aFireball.Position = new Vector2(1000, 1000); // Gambiarra pra ela sair demais do ponto inicial dela fazendo com o que o codigo a recolha
                        }
                }
                if (Health <= 0)
                {// Se o bixo morrer e o player tiver do lado errado ele poe o spwan do bixo pro outro lado
                    if (player.Position.X <= 500 && START_POSITION_X <= 500)
                    {
                        aux = START_POSITION_X;
                        START_POSITION_X = START_POSITION_X2;
                        START_POSITION_X2 = aux;
                    }
                    else
                        if (player.Position.X >= 500 && START_POSITION_X >= 600)
                        {
                            aux = START_POSITION_X;
                            START_POSITION_X = START_POSITION_X2;
                            START_POSITION_X2 = aux;
                        }
                    Score++;
                    round++;
                    defense = Defense + (Reset * 3);
                    if (player.SayanUnlocked == 2) // Se ele for super sayajin 3, a defesa só pode ir até 55 senao ele dá 0 de dano
                        if (Defense >= 60)
                            Defense = 55;
                    if (Defense > 75) // Aqui é o limite pro sayajin 4
                          defense = 75;
                    speed += 5;
                    Health = 100; // Calculos especificos pros sprites dos montros
                    if (round == 0)
                    {
                         WidthStart = 3 * round * 25;
                    }
                    else
                        if(round == 1)
                            WidthStart = 3 * round * 25;
                        else 
                            if (round == 2)
                                WidthStart = 3 * round * 24;
                            else
                                if (round == 3)
                                     WidthStart = 3 * round * 24;
                                else
                                    if (round == 4)
                                    {
                                        WidthStart = 0;
                                        HeightStart = 4 * 33;

                                    }
                                    else
                                        if (round == 5)
                                        {
                                            WidthStart = 3 * 24;
                                            HeightStart = 4 * 33;

                                        }
                                        else 
                                            if (round == 6)
                                            {
                                                WidthStart =  145;
                                                HeightStart = 126;

                                            }
                                                else 
                                                    if (round == 7)
                                                    {
                                                        WidthStart = 218;
                                                        HeightStart = 4 * 32;

                                                    }
                                                    else
                                                    {
                                                        if (round == 8)
                                                        {
                                                            AssetName = "Monsters2";
                                                            if(player.Sayan == 0)
                                                                Defense = player.Damage - 3;
                                                            else
                                                                Defense = player.Damage - 1;
                                                        }
                                                        else
                                                        {
                                                            AssetName = "Monsters";
                                                            heightStart = 0;
                                                            reset++;
                                                            player.SayanUnlocked++;
                                                            if (player.SayanUnlocked > 3)
                                                                player.SayanUnlocked = 3;
                                                            round = 0;
                                                        }
                                                    }

                    LoadContent(mContentManager);

                   
                }

            }

        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            blast = theContentManager.Load<SoundEffect>("hit");
            blastInstance = blast.CreateInstance();
            base.LoadContent(theContentManager, AssetName);
        }
        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {

                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (Position.X >= player.Position.X + player.Size.Width/2 + 5)
                {
                    mSpeed.X = speed;
                    mDirection.X = MOVE_LEFT;
                }
                if (Position.X <= player.Position.X + player.Size.Width/2 - 5)
                {
                    mSpeed.X = speed;
                    mDirection.X = MOVE_RIGHT;
                }
                if (Position.Y >= player.Position.Y + 5)
                {
                    mSpeed.Y = speed;
                    mDirection.Y = MOVE_UP;
                }
                if (Position.Y <= player.Position.Y -5)
                {
                    mSpeed.Y = speed;
                    mDirection.Y = MOVE_DOWN;
                }


            }
        }
        public override void Draw(SpriteBatch theSpriteBatch) // draw modificado da principal
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Source,
     Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);

        }
    }
}
