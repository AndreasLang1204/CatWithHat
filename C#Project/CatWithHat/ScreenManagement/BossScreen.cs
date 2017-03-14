/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/

#region Using Region
using System;
using System.Collections.Generic;

// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MMP1
{
    public class BossScreen : GameScreen
    {
        #region Const Region
        const byte nrOfBossBullets = 28;
        const string winTxt = "You have defeted the mighty Walter Sprayer\n and therefore you have earned his hat!\n\npress 'Enter' to finish";
        const string loseTxt = "MUHAHAHAHA!!\nsee a low cat is no match for me!\n\npress 'Enter' to continue";
        #endregion

        #region Member Region
        Texture2D backgroundImg;
        FileManager fileManager;
        bool displayTxt = false;
        string bossTxt;
        Boss boss;
        Random rnd;
        List<Color> bgColor;
        TimeSpan switchBGColorTime = new TimeSpan(0, 0, 0, 0, 500);    
        TimeSpan delay;
        int actColorIdx;
        #endregion

        #region Mono Method Region
        // Loads the content for the screen
        public override void LoadContent(ContentManager Content, ref Player player)
        {
            base.LoadContent(Content, ref player);

            if (font == null)
                font = content.Load<SpriteFont>("bossFont");

            rnd = new Random();
            fileManager = new FileManager();
            backgroundImg = content.Load<Texture2D>("bossFight");

            map = new Map();
            map.LoadContent(content, "MapBoss");

            boss = new Boss(content,
                            new Vector2(820, 182), 
                            content.Load<Texture2D>("sprayBossHat_spriteAnim"), 
                            content.Load<Texture2D>("waterDrop"), 
                            nrOfBossBullets);
            player.Position = new Vector2(256, 32);
            boss.IsDefeted = false;

            // list of colors for changing BG color
            bgColor = new List<Color>();
            bgColor.Add(Color.Blue);
            bgColor.Add(Color.Red);
            bgColor.Add(Color.Green);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            // if delay is >= switchBGColorTime change the background color
            delay += gameTime.ElapsedGameTime;
            if (delay >= switchBGColorTime)
            {
                actColorIdx = (actColorIdx + 1) % bgColor.Count;
                delay = TimeSpan.Zero;
            }
               
            // while player is not hit by a bullet
            if(!player.GotHit)
            {
                boss.Update(gameTime, map.Layer);
                player.Update(gameTime, map.Layer);
                player.BulletCollision(boss.Bullets);
                GameManager.Instance.DrawPlayer = true;
                
                Entity e;
                e = player;
                map.UpdateCollision(ref e);
                player = (Player)e;
            }
            // if the player got hit by a bullet
            else
            {
                displayTxt = true;
                bossTxt = loseTxt;
                
                // wait on player input before the current screen changes to the HubScreen
                if (InputManager.KeyPressed(Keys.Enter))
                {
                    player.Position = new Vector2(64, 0);
                    player.GravityActive = true;
                    GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                    player.Deaths++;
                    player.HatCnt--;
                    player.GotHit = false;
                    GameManager.Instance.SwitchScreen(new HubScreen());
                }
            }

            // if the boss is defeted
            if (boss.IsDefeted)
            {
                displayTxt = true;
                bossTxt = winTxt;

                // wait on player input before the current screen changes to the CreditScreen
                if (InputManager.KeyPressed(Keys.Enter))
                {
                    player.Position = new Vector2(64, 0);
                    player.GravityActive = true;
                    GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                    player.ChangePlayerSprite(1);
                    boss.IsDefeted = false;
                    player.HatCnt = 0;
                    player.StageNr = 0;
                    GameManager.Instance.SwitchScreen(new CreditScreen());
                }
            }
        
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(backgroundImg,
                             new Rectangle((int)Camera.Instance.Position.X, 0, backgroundImg.Width, backgroundImg.Height),
                             bgColor[actColorIdx]);

            // draw lose or win text
            if (displayTxt)
                spriteBatch.DrawString(font, 
                                       bossTxt, 
                                       new Vector2((backgroundImg.Width/2)-(font.MeasureString(bossTxt).X/2), 
                                                    backgroundImg.Height * (1f/3)), 
                                       Color.Black);
            
            map.Draw(spriteBatch);
            boss.Draw(spriteBatch);
        }
        #endregion
    }
}

