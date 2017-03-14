/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/

#region Using Region
// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MMP1
{
    public class TitleScreen : GameScreen
    {
        #region Const Region
        const string gameMode = "Select difficulty:\npress: 'E' for easy   'N' for normal   'H' for hard";
        const string creditsTxt = "Credits";
        const string startTxt = "Start Game";
        const string continueTxt = "Continue";
        const string exitTxt = "EXIT";
        #endregion

        #region Member Region
        Texture2D backgroundImg;
        FileManager fileManager;
        
        string playTxt = startTxt;
        bool displayMode;
        #endregion

        #region Mono Method Region
        // load content for screen
        public override void LoadContent(ContentManager Content, ref Player player)
        {
            base.LoadContent(Content, ref player);

            if (font == null)
                font = content.Load<SpriteFont>("catFont");

            fileManager = new FileManager();
            backgroundImg = content.Load<Texture2D>("rainbowBG");

            map = new Map();
            map.LoadContent(content, "MapTitleMenu");
            player.Position = new Vector2(256, 32);
            player.ChangePlayerSprite(0);
            GameManager.Instance.DrawPlayer = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            if(!displayMode)
                player.Update(gameTime, map.Layer);
           
            Entity e;
            e = player;
            map.UpdateCollision(ref e);
            player = (Player)e;

            // if player steps on one of the goals
            if (player.StageCleare)
            {
                player.StageCleare = false;

                // "credit" goal ---> switch to CreditScreen
                if (player.Position.X < (map.Layer.LayerWidth / 3))
                {
                    player.Position = new Vector2(64, 0);
                    player.GravityActive = true;
                    GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                    GameManager.Instance.SwitchScreen(new CreditScreen());
                }
                // "exit" goal ---> exit the game
                else if (player.Position.X > (map.Layer.LayerWidth * (2.0f / 3.0f)))
                    GameManager.Instance.ExitGame = true;
                // "start/continue" goal ---> switch to HubScreen
                else
                {
                    displayMode = true;
                    GameManager.Instance.DrawPlayer = false;
                    // player choose "easy" mode
                    if (InputManager.KeyPressed(Keys.E))
                    {
                        player.Position = new Vector2(64, 0);
                        player.GravityActive = true;
                        GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                        GameManager.Instance.Gamemode = Layer.Mode.Easy;
                        GameManager.Instance.SwitchScreen(new HubScreen());
                    }
                    // player choose "normal" mode
                    else if (InputManager.KeyPressed(Keys.N))
                    {
                        player.Position = new Vector2(64, 0);
                        player.GravityActive = true;
                        GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                        GameManager.Instance.Gamemode = Layer.Mode.Normal;
                        GameManager.Instance.SwitchScreen(new HubScreen());
                    }
                    // player choose "hard" mode
                    else if (InputManager.KeyPressed(Keys.H))
                    {
                        player.Position = new Vector2(64, 0);
                        player.GravityActive = true;
                        GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                        GameManager.Instance.Gamemode = Layer.Mode.Hard;
                        GameManager.Instance.SwitchScreen(new HubScreen());
                    }
                }
                
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            if (player.StageNr > 0)
                playTxt = continueTxt;

            spriteBatch.Draw(backgroundImg,
                             new Rectangle((int)Camera.Instance.Position.X, 0,
                                           backgroundImg.Width, backgroundImg.Height),
                             Color.White);

            // draw choose game mode
            if (displayMode)
            {
                spriteBatch.DrawString(font, gameMode, 
                                       new Vector2((map.Layer.LayerWidth / 2) - (Layer.TileDimensions.X / 2) - (font.MeasureString(gameMode).X / 2), 
                                                   map.Layer.LayerHeight * (1f/4) - font.MeasureString(gameMode).Y), 
                                       Color.Black);
            }
            // draw Credits, Start, Exit Signs
            else
            {
                spriteBatch.DrawString(font, creditsTxt, new Vector2(96 - (Layer.TileDimensions.X / 2) - (font.MeasureString(creditsTxt).X / 2), 256), Color.Black);
                spriteBatch.DrawString(font, playTxt, new Vector2(512 - (Layer.TileDimensions.X / 2) - (font.MeasureString(playTxt).X / 2), 192), Color.Black);
                spriteBatch.DrawString(font, exitTxt, new Vector2(960 - (Layer.TileDimensions.X / 2) - (font.MeasureString(exitTxt).X / 2), 320), Color.Black);
            }
            map.Draw(spriteBatch);
        }
        #endregion
    }
}