/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/

#region Using Region
using System.Collections.Generic;

// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MMP1
{
    public class SplashScreen : GameScreen
    {
        #region Member Region
        List<Texture2D> images;
        int imageNumber;
        FileManager fileManager;
        #endregion

        #region MonoMethod Region
        // load content for the screen
        public override void LoadContent(ContentManager Content, ref Player player)
        {
            base.LoadContent(Content, ref player);
            
            if (font == null)
                font = content.Load<SpriteFont>("Font1");

            imageNumber = 0;
            fileManager = new FileManager();
            images = new List<Texture2D>();

            // load Image names from file
            fileManager.LoadContent("Content/SplashFile.cwh", "");

            for(int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch(fileManager.Attributes[i][j])
                    {
                        case "Image":
                            images.Add(content.Load<Texture2D>(fileManager.Contents[i][j]));
                            break;
                    }
                }
            }

            map = new Map();
            map.LoadContent(content, "MapSplashScreen");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, map.Layer);
           
            Entity e;
            e = player;
            map.UpdateCollision(ref e);
            player = (Player)e;

            if (InputManager.KeyPressed(Keys.E))
            {
                player.StageCleare = false;
                player.Position = new Vector2(64, 32);
                player.GravityActive = true;
                GameManager.Instance.DrawPlayer = false;            // prevent drawing the player during a screen change
                GameManager.Instance.SwitchScreen(new TitleScreen());
            }

            // if player reaches goal --> display next splash screen --> if all splash screens have been shown --> switch to titlescreen
            if (player.StageCleare)
            {
                player.StageCleare = false;
                imageNumber++;
                player.Position = new Vector2(64, player.Position.Y);

                if (imageNumber > images.Count - 1)
                {
                    player.Position = new Vector2(64, 32);
                    player.GravityActive = true;
                    GameManager.Instance.DrawPlayer = false;                // prevent drawing the player during a screen change
                    GameManager.Instance.SwitchScreen(new TitleScreen());
                    imageNumber = images.Count - 1;                         // for last draw call
                }
            }   
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(images[imageNumber], 
                             new Rectangle(0, 
                                           0, 
                                           images[imageNumber].Width, 
                                           images[imageNumber].Height), 
                             Color.White);
           
            map.Draw(spriteBatch);
        }
        #endregion
    }
}
