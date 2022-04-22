using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
///// TO DO

// COLISAO DO TIRO NO EIXO Y // Done
// SONS E MENU
// TROCA DE MONSTROS (AINDA TA BUGADO ESSA MERDA) // Quase done
// HEALTH BAR ??? 
namespace Castaneda
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Song menuTheme;
        Song gameTheme;
        int mouseX, mouseY;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Declaração dos Sprites
        Goku mGokuSprite;
        Monster mMonsterSprite;
        protected Texture2D background;
        protected Texture2D menu;
        protected Texture2D pointer;
        protected Texture2D instrucoes;
        protected Rectangle mainFrame;
        protected SpriteFont font;
        // Alternadores entre menu e jogo
        protected bool gameOn = false;
        protected bool menuOn = true;
        // Health Bar
        protected SpriteBatch mBatch;
        protected Texture2D mHealthBar;
        //
        private void DrawText()
        {
            spriteBatch.DrawString(font, "Score: " + mMonsterSprite.Score, new Vector2(20, 30), Color.Black);
            spriteBatch.DrawString(font, "Monster Def: " + mMonsterSprite.Defense, new Vector2(20, 60), Color.Black);
            spriteBatch.DrawString(font, "Monster Health Bar: " + mMonsterSprite.Health, new Vector2(380, 40), Color.Black);
            if(mMonsterSprite.Reset > 0 && mGokuSprite.Sayan != mGokuSprite.SayanUnlocked)
                spriteBatch.DrawString(font, " Super Sayajin  " + (mGokuSprite.SayanUnlocked + 1) +  " Press:Right Control", new Vector2(350, 250), Color.Black);
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 960;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mGokuSprite = new Goku(3,33,23,100,"Player",0,0);
            mMonsterSprite = new Monster(3,33,23,100,"Monsters",mGokuSprite,0,0);
            font = Content.Load<SpriteFont>("spriteFont1");
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mGokuSprite.LoadContent(this.Content); 
            mMonsterSprite.LoadContent(this.Content); 
            mBatch = new SpriteBatch(this.graphics.GraphicsDevice);
            mHealthBar = Content.Load<Texture2D>("HealthBar") as Texture2D;
            background = Content.Load<Texture2D>("Arena");
            pointer = Content.Load<Texture2D>("menu_selected");
            instrucoes = Content.Load<Texture2D>("instrucoes");
            menu = Content.Load<Texture2D>("Menu");
            menuTheme =  Content.Load<Song>("menuTheme");
            gameTheme = Content.Load<Song>("fightTheme2");
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            MediaPlayer.Play(menuTheme);
            MediaPlayer.IsRepeating = true;


        }
        protected void resetGame()
        {

            Initialize();
            LoadContent();
            menuOn = true;
            gameOn = false;
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (gameOn)
            {
                mGokuSprite.Update(gameTime);
                mMonsterSprite.Update(gameTime);
                if (mMonsterSprite.Position.X + mMonsterSprite.Size.Width / 2 >= mGokuSprite.Position.X - 5 && mMonsterSprite.Position.X + mMonsterSprite.Size.Width / 2 <= mGokuSprite.Position.X + mGokuSprite.Size.Width + 5 && mMonsterSprite.Position.Y + mMonsterSprite.Size.Height / 2 >= mGokuSprite.Position.Y && mMonsterSprite.Position.Y + mMonsterSprite.Size.Height / 2 <= mGokuSprite.Position.Y + mGokuSprite.Size.Height)
                    resetGame();
            }
            else
                UpdateMouse();
            base.Update(gameTime);
        }

        protected void UpdateMouse()
        {
            MouseState current_mouse = Mouse.GetState();

            // The mouse x and y positions are returned relative to the
            // upper-left corner of the game window.

            mouseX = current_mouse.X;
            mouseY = current_mouse.Y;
            if (current_mouse.LeftButton == ButtonState.Pressed)
                if (mouseX > 118 && mouseX < 405 && mouseY > 325 && mouseY < 430)
                {

                    gameOn = true;
                    menuOn = false;
                    MediaPlayer.Stop();
                    MediaPlayer.Play(gameTheme);
                }
                else
                    if (mouseX > 118 && mouseX < 405 && mouseY > 212 && mouseY < 315)
                        menuOn = false;
                    else
                        if (!gameOn && !menuOn)
                            if (mouseX > 17 && mouseX < 300 && mouseY > 478 && mouseY < 580)
                                menuOn = true;
                  
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (gameOn)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(background, mainFrame, Color.White);
                    mGokuSprite.Draw(this.spriteBatch);
                    mMonsterSprite.Draw(this.spriteBatch);
                    
                    spriteBatch.End();
                    // Draw da healtb ar
                    mBatch.Begin();
                    //Espaço negativo
                    mBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,

                         30, mHealthBar.Width, 44), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Gray);


                    //Vida atual
                    mBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,
                         30, (int)(mHealthBar.Width * ((double)mMonsterSprite.Health / 100)), 44),
                         new Rectangle(0, 45, mHealthBar.Width, 44), Color.Red);

                    //Caixa da bar
                    mBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,

                        30, mHealthBar.Width, 44), new Rectangle(0, 0, mHealthBar.Width, 44), Color.White);

                    mBatch.End();
                    spriteBatch.Begin();
                    DrawText();
                    spriteBatch.End();
            }
            else
                if (menuOn)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(menu, mainFrame, Color.White);
                    spriteBatch.Draw(pointer, new Vector2(mouseX, mouseY), Color.White);
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(instrucoes, mainFrame, Color.White);
                    spriteBatch.Draw(pointer, new Vector2(mouseX, mouseY), Color.White);
                    spriteBatch.End();  
                }
            base.Draw(gameTime);
        }
    }
}
