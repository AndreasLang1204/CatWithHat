/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/

#region Using Region
using System;

// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MMP1
{
    public class GameplayScreen : GameScreen
    {
        #region Const Region
        const int defaultLevelWidth = 250;
        const float backgroundOffsetScrollSpeed = 0.6f;
        const float parallaxOffsetScrollSpeed = 1.5f;
        const int increaseLevelWidthThreshold = 3;
        const string warningTxt = "-- ! Warning ! -- Auto scroller level, press right to start";
        #endregion

        #region Member Region
        uint actPlayerDeaths;
        int levelWidth;
        #endregion

        #region Mono Method Region
        // Loads the content for the screen
        public override void LoadContent(ContentManager Content, ref Player player)
        {
            base.LoadContent(Content, ref player);
            map = new Map();

            background = new Background(content.Load<Texture2D>("GameBG2"),
                                        content.Load<Texture2D>("layertest"),
                                        GameManager.Instance.ViewportDimensions);

            font = content.Load<SpriteFont>("catFont");

            if (player.StageNr >= increaseLevelWidthThreshold)
                levelWidth = defaultLevelWidth * 2;
            else
                levelWidth = defaultLevelWidth;

            // generate random level
            map.LoadContent(content, new Random(), levelWidth, GameManager.Instance.Gamemode);
            
            player.ResetPosition = new Vector2(64, 0);
            player.StageNr++;
            player.RunFlag = false;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            actPlayerDeaths = player.Deaths;
            player.UpdateAutoRun(gameTime, map.Layer);
            
            // the player loses 1 hat every 9 deaths
            if(player.Deaths % 9 == 0 && (actPlayerDeaths < player.Deaths))
                player.HatCnt--;
            
            GameManager.Instance.DrawPlayer = true;

            Entity e;
            e = player;
            map.UpdateCollision(ref e);
            player = (Player)e;

            // exit the current screen if the player reaches the goal
            // or when he aborts the level by pressing 'E'
            if (player.StageCleare || InputManager.KeyPressed(Keys.E))
            {
                player.StageCleare = false;
                player.Position = new Vector2(64, 32);
                player.GravityActive = true;
                GameManager.Instance.DrawPlayer = false;            // prevent drawing the player during a screen change
                GameManager.Instance.SwitchScreen(new HubScreen());
            }

            // update Background
            background.BackgroundOffset += (int)(player.Velocity.X * backgroundOffsetScrollSpeed);
            background.ParallaxOffset += (int)(player.Velocity.X * parallaxOffsetScrollSpeed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            background.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Stage: " + player.StageNr, new Vector2(32, 416), Color.Black);
            spriteBatch.DrawString(font, "Death Counter: " + player.Deaths, new Vector2(32, 480), Color.Black);
            spriteBatch.DrawString(font, "Hats: " + player.HatCnt, new Vector2(32, 448), Color.Black);
            map.Draw(spriteBatch);
            spriteBatch.DrawString(font, warningTxt, 
                                   new Vector2((GameManager.Instance.ViewportDimensions.X/2) - (font.MeasureString(warningTxt).X/2), 
                                               (map.Layer.LayerHeight / 2) - (font.MeasureString(warningTxt).Y)), 
                                   Color.OrangeRed);
        }
        #endregion
    }
}

