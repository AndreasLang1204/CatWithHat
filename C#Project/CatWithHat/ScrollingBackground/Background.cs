/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/
/********************************************
* Implementierungsgrundlage für die Klasse, *
* wurde aus einem Tutorial übernommen.      *
*********************************************/

#region Using Region
// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    public class Background
    {
        #region Member Region
        // Textures which hold the background images
        Texture2D backgroundImg;
        Texture2D parallaxImg;

        // viewportWidth/Height determine how large the BG Image will be displayed
        // when rendered to the screen
        int viewportWidth = 1024;       // default value, will be overwriten in the constructor
        int viewportHeight = 720;       // - || -

        int backgroundWidth = 1920;     // default value, will be overwriten in the constructor
        int backgroundHeight = 720;     // - || -
        int parallaxWidth = 1680;       // - || -
        int parallaxHeight = 480;       // - || -

        int backgroundOffset;
        int parallaxOffset;

        // if true the parallax BG will be drawn
        bool drawParallax = true;
        #endregion

        #region Property region
        public int BackgroundOffset
        {
            get { return backgroundOffset; }

            // check to see if we have gone off of either end of the texture and 
            // wrap around if necessary. 
            set
            {
                backgroundOffset = value;
                if(backgroundOffset < 0)
                {
                    backgroundOffset += backgroundWidth;
                }
                if (backgroundOffset > backgroundWidth)
                {
                    backgroundOffset -= backgroundWidth;
                }
            }
        }

        public int ParallaxOffset
        {
            get { return parallaxOffset; }

            // check to see if we have gone off of either end of the texture and 
            // wrap around if necessary. 
            set
            {
                parallaxOffset = value;
                if (parallaxOffset < 0)
                {
                    parallaxOffset += parallaxWidth;
                }
                if (parallaxOffset > parallaxWidth)
                {
                    parallaxOffset -= parallaxWidth;
                }
            }
        }

        public bool DrawParallax
        {
            get { return drawParallax; }
            set { drawParallax = value; }
        }
        #endregion

        #region Constructor region
        public Background(Texture2D backgroundImg, Texture2D parallaxImg, Vector2 screenDimensions)
        {
            this.backgroundImg = backgroundImg;
            backgroundWidth = this.backgroundImg.Width;
            backgroundHeight = this.backgroundImg.Height;

            this.parallaxImg = parallaxImg;
            parallaxWidth = this.parallaxImg.Width;
            parallaxHeight = this.parallaxImg.Height;

            viewportWidth = (int)screenDimensions.X;
            viewportHeight = (int)screenDimensions.Y;
        }
        #endregion

        #region Mono Method Region
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw BG Image, offset by the player's location
            spriteBatch.Draw(backgroundImg, 
                             new Rectangle(-1 * backgroundOffset + (int)Camera.Instance.Position.X,
                                           0,
                                           backgroundWidth,
                                           viewportHeight),
                              Color.White);

            // If the right edge of the background panel will end 
            // within the bounds of the display, draw a second copy 
            // of the background at that location.
            if(backgroundOffset > backgroundWidth - viewportWidth)
            {
                 spriteBatch.Draw(backgroundImg,
                                 new Rectangle((-1 * backgroundOffset) + backgroundWidth + (int)Camera.Instance.Position.X,
                                               0,
                                               backgroundWidth,
                                               viewportHeight),
                             Color.White);
            }

            if(drawParallax)
            {
                // draw parallax BG
                spriteBatch.Draw(parallaxImg,
                            new Rectangle(-1 * parallaxOffset + (int)Camera.Instance.Position.X,
                                          0,
                                          parallaxWidth,
                                          viewportHeight),
                             Color.White);

                // if the player is past the point where the parallax BG will end on the active screen 
                // we need to draw a second copy of it to cover the remaining screen area.
                if (parallaxOffset > parallaxWidth - viewportWidth)
                {
                    spriteBatch.Draw(parallaxImg,
                                     new Rectangle((-1 * parallaxOffset) + parallaxWidth + (int)Camera.Instance.Position.X,
                                                   0,
                                                   parallaxWidth,
                                                   viewportHeight),
                                     Color.White);
                }
            }
        }
        #endregion
    }
}