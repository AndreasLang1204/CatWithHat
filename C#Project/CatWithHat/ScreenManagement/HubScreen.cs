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
#endregion

namespace MMP1
{
    public class HubScreen: GameScreen
    {
        #region Const Region
        const int itemsRequired = 3;
        const string blockTxt = "STOP!\nYou need at least 3 hats\n to enter the club.";
        const string passTxt = "Now you can enter the club\nand face the mighty Walter Sprayer!";
        #endregion

        #region Member Region
        Texture2D backgroundImg;
        FileManager fileManager;
        bool displayTxt = false;
        string gatekeeperTxt;
        #endregion

        #region Mono Method Region
        // load content for the screen
        public override void LoadContent(ContentManager Content, ref Player player)
        {
            base.LoadContent(Content, ref player);

            if (font == null)
                font = content.Load<SpriteFont>("catFont");

            fileManager = new FileManager();
            backgroundImg = content.Load<Texture2D>("HubScreen");

            map = new Map();
            map.LoadContent(content, "MapHub");
            player.Position = new Vector2(256, 32);
            player.ChangePlayerSprite(0);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, map.Layer);
            GameManager.Instance.DrawPlayer = true;
          
            Entity e;
            e = player;
            map.UpdateCollision(ref e);
            player = (Player)e;
        
            // if player steps on one of the goals
            if (player.StageCleare)
            {
                player.StageCleare = false;
                
                // "free Hat" goal ---> switch to GameplayScreen
                if(player.Position.X < (map.Layer.LayerWidth / 3))
                {
                    player.Position = new Vector2(64, 0);
                    player.GravityActive = true;
                    GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                    GameManager.Instance.SwitchScreen(new GameplayScreen());
                }
                // "home sign" goal ---> switch to TitleScreen
                else if (player.Position.X > (map.Layer.LayerWidth * (2.0f / 3.0f)))
                {
                     player.Position = new Vector2(256, 0);
                     player.GravityActive = true;
                     GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                     GameManager.Instance.SwitchScreen(new TitleScreen());
                }
                // "gatekeeper" goal
                else
                {
                    displayTxt = true;
                    if (player.HatCnt >= itemsRequired)
                    {
                        gatekeeperTxt = passTxt;

                        // "boss" goal and player has collected enought hats ---> switch to BossScreen
                        if (player.Position.X > Layer.TileDimensions.X * 18)
                        {
                            player.Position = new Vector2(64, 0);
                            player.GravityActive = true;
                            GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                            GameManager.Instance.SwitchScreen(new BossScreen());
                        }
                    }
                    else
                        gatekeeperTxt = blockTxt;
                }
                
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(backgroundImg,
                             new Rectangle((int)Camera.Instance.Position.X, 0,
                                           backgroundImg.Width, backgroundImg.Height),
                             Color.White);

            // draw gatekeeper text
            if(displayTxt)
                spriteBatch.DrawString(font, gatekeeperTxt, new Vector2(448, 256), Color.White);
            
            map.Draw(spriteBatch);

            spriteBatch.DrawString(font, "Hats collected: " + player.HatCnt.ToString(), new Vector2(32 , 480), Color.Black);
        }
        #endregion
    }
}