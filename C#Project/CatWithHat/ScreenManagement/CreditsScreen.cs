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
    public class CreditScreen : GameScreen
    {
        #region Const Region
        const int creditsScrollSpeed = 2;
        #endregion

        #region Mono Method Region
        // load the content for the screen
        public override void LoadContent(ContentManager Content, ref Player player)
        {
            base.LoadContent(Content, ref player);
            
            if (font == null)
                font = content.Load<SpriteFont>("catFont");

            background = new Background(content.Load<Texture2D>("GameBG2"),
                                        content.Load<Texture2D>("credits"),
                                        GameManager.Instance.ViewportDimensions);

            map = new Map();
            map.LoadContent(content, "MapSplashScreen");
        }

        public override void UnloadContent()
        {
            base.UnloadContent(); 
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, map.Layer);
            GameManager.Instance.DrawPlayer = true;
         
            Entity e;
            e = player;
            map.UpdateCollision(ref e);
            player = (Player)e;

            // set parallax scroll speed
            background.ParallaxOffset += creditsScrollSpeed;

            // if player reaches goal --> switch to titlescreen
            if (player.StageCleare)
            {
                player.StageCleare = false;
                player.Position = new Vector2(256, 32);
                player.GravityActive = true;
                GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                GameManager.Instance.SwitchScreen(new TitleScreen());
                   
            }   
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            background.Draw(spriteBatch);
            map.Draw(spriteBatch);  
        }
        #endregion
    }
}
