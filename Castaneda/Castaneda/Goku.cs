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
    class Goku : Sprite
    {
        SoundEffect turnSayan;
        SoundEffectInstance turnSayanInstance;
        SoundEffect blast;
        SoundEffectInstance blastInstance;
        SoundEffect sayan2Loop;
        public SoundEffectInstance sayan2LoopInstance;
        SoundEffect sayan3Loop;
        public SoundEffectInstance sayan3LoopInstance;
        // Projeteis
        // Gambiarra pra saber a ultima posição que o mano tava olhando e atirar na direção certa
        protected Vector2 aux2 = new Vector2(0,-1);
        protected Vector2 aux3 = Vector2.Zero; // Usado para guardar as coordenadas que devem ser somadas a posição inicial do projétil no call da função Fire

        public List<Fireball> mFireballs = new List<Fireball>(); // As fireballs são publicas e pertencem ao estado
        private float sayan = 0;
        protected int sayanUnlocked = 0; // até onde voce pode desbloqueou e pode virar sayajin
        protected int damage; // Quando de dano goku pode dar
        ContentManager mContentManager;
        // constantes
        protected int START_POSITION_X = 800;
        protected int START_POSITION_Y = 350;
        const int WIZARD_SPEED = 220;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        // Propriedades
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public int SayanUnlocked
        {
            get { return sayanUnlocked; }
            set { sayanUnlocked = value; }
        }
        public float Sayan
        {
            get { return sayan; }
            set { sayan = value; }
        }

        // Inicialização

        public Goku(int mFrames, int mHeight, int mWidth, float mInterval, string mAssetName,int mHeightStart, int mWidthStart)
            : base(mFrames, mHeight, mWidth, mInterval, mAssetName,mHeightStart, mWidthStart)
        {
            frames = mFrames;
            AssetName = mAssetName;
            frameHeight = mHeight;
            frameWidth = mWidth;
            interval = mInterval;
            heightStart = mHeightStart;
            widthStart = mWidthStart;
            mDirection.X = MOVE_RIGHT;
        
        }
        // Estados em que o wizard pode estar
        enum State
        {
            Walking
        }
        // Inicializando pra ele começar andando
        State mCurrentState = State.Walking;
        // Guarda a posi~ção que o viadinho tava quando começou a pular
        protected Vector2 mStartingPosition = Vector2.Zero;
        protected Vector2 mDirection;// Voce começa pra direita
        protected Vector2 mSpeed = Vector2.Zero;
        KeyboardState mPreviousKeyboardState;

        public void Update(GameTime theGameTime)
        {

            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);
            UpdateSayan(aCurrentKeyboardState);
            UpdateFireball(theGameTime, aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);                       // Calculo dos irmão das marotage pra funfar diferentes cortes pra diferentes sprites
            Source = new Rectangle(currentFrame * frameWidth + ((int)sayan * 3 * frameWidth + (sayan != 0 ? 7 : 0) + (sayan == 3 ? 4 : 0 )), frameY * frameHeight, frameWidth, frameHeight); // O source do goku é diferente, entao tem que mudar, já que o sprite muda tb
        }


        private void UpdateFireball(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                ShootFireball();
            }
        }
        private void UpdateSayan(KeyboardState aCurrentKeyboardState)
        {
            if ((sayan < sayanUnlocked))
                if (aCurrentKeyboardState.IsKeyDown(Keys.RightControl) == true && mPreviousKeyboardState.IsKeyDown(Keys.RightControl) == false)
                {
                    sayan++;
                    if (sayan > 3)
                        sayan = 3;
                    turnSayanInstance.Play();
                    if (sayan == 1)
                    {
                        sayan2LoopInstance.IsLooped = true;
                        sayan2LoopInstance.Play();
                    }
                    else
                    {
                        if (sayan == 2)
                        {
                            sayan3LoopInstance.IsLooped = true;
                            sayan2LoopInstance.Stop();
                            sayan3LoopInstance.Play();

                        }
                        else
                        {
                            sayan3LoopInstance.Stop();
                        }
                    }

                }

            mSpeed = mSpeed * sayan + new Vector2(200,200);
            Damage = 20 * (int)sayan + 20;
        }
        private void ShootFireball()
        {
            blastInstance.Play();
            // Arrumar a posição do tiro porque senao ele fica FEIO
                if (aux2.X == 1)
            {
                aux3 = new Vector2(30, frameHeight - sayan * 7);
            }
            if (aux2.X == -1)
            {
                aux3 = new Vector2(-30 * sayan, frameHeight - sayan * 7);
            }

            if (aux2.Y == -1)
            {
                aux3 = new Vector2(-20 * sayan + 15, 25*sayan);
                if(sayan >= 2)
                    aux3 = new Vector2(-3 * sayan, 3 * sayan);
            } 
            else
                if (aux2.Y == 1)
                {
                    aux3 = new Vector2(-20 * sayan + 15, 25 * sayan);
                    if (sayan >= 2)
                        aux3 = new Vector2(-3 * sayan, 10 * sayan);
                }
            if (mCurrentState == State.Walking)
            {

                bool aCreateNew = true;
                foreach (Fireball aFireball in mFireballs)
                {
                   // if (Visible == true)
                   // {  
                        if (aFireball.Visible == false)
                        {
                            aCreateNew = false;
                            aFireball.Sayan = sayan; // Muda o tamanho do tiro antigo depois de reativa-lo // MUDAR ESSA PORRA
                             aFireball.Fire(Position + aux3,
                                new Vector2(200, 200) * 2, aux2); // EU NAO FAÇÕ IDEIA DO PORQUE MAS FUNCIONOU ENTAO FUCK LOGIC
                            break;
                      //  }
                    }
                }

                if (aCreateNew == true)
                {
                     Fireball aFireball = new Fireball(2,27,27,200,"Kamehameha",0,0,sayan);
                    aFireball.LoadContent(mContentManager);
                    aFireball.Fire(Position + aux3,
                        new Vector2(200, 200) * 2, aux2);
                     mFireballs.Add(aFireball);
                }
            }
        }
        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true || aCurrentKeyboardState.IsKeyDown(Keys.Right) == true || aCurrentKeyboardState.IsKeyDown(Keys.Up) == true || aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    if(mDirection != Vector2.Zero)
                      aux2 = mDirection; // Na primeira vez que roda funciona, na segunda ele acaba dando a aux2 o valor vector2.zero, que buga o movimento pra cima e pra baixo
                }

                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                else
                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_UP;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
                else {
                   currentFrame = 1;
                }
                       
            }
        }
 
        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.LoadContent(theContentManager);
            }
            turnSayan = theContentManager.Load<SoundEffect>("sayan2");
            turnSayanInstance = turnSayan.CreateInstance();
            blast = theContentManager.Load<SoundEffect>("blast");
            blastInstance = blast.CreateInstance();
            SoundEffect.MasterVolume = 0.4f;
            sayan2Loop = theContentManager.Load<SoundEffect>("sayan2loop");
            sayan2LoopInstance = sayan2Loop.CreateInstance();
            sayan3Loop = theContentManager.Load<SoundEffect>("sayan3loop");
            sayan3LoopInstance = sayan3Loop.CreateInstance();
            sayan2LoopInstance.Stop();
            sayan3LoopInstance.Stop();
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, AssetName);
        }
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Draw(theSpriteBatch);
            }
            base.Draw(theSpriteBatch);
        }
    }
}

