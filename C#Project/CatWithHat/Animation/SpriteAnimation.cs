/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/
/**************************************************************************
* Implementierungsgrundlage für die Klasse wurde aus dem                  *
* “XNA Platformer” ­Tutorial von CodeingMadeEasy übernommen. Und dann von  *
* mir auf meine Anforderungen angepasst.                                  *     
* (https://www.youtube.com/watch?v=FR7crO2xq8A&list=PLE500D63CA505443B)   *
***************************************************************************/

#region Using Region
// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    public class SpriteAnimation
    {
        #region Field Region
        private Texture2D sprite;
        private Color drawColor;

        private Rectangle srcRect;        // position and size of the current frame in the used Spritesheet
        private float scale;
        private float rotation;
        private float alpha;
        private Vector2 origin;           // center of the image
        private Vector2 position;
        private bool isActive;

        private ContentManager content;

        Vector2 frames;                 // dimension of the Spritesheet: X amount of sprites in a row | Y amount of rows
        Vector2 currentFrame;

        int frameCounter;               
        int switchFrame;                // time between animation frames
        #endregion

        #region Property Region
        public int SwitchFrame
        {
            set { switchFrame = value; }
        }

        public Color DrawColor
        {
            set { drawColor = value; }
        }

        public Vector2 Frames
        {
            set { frames = value; }
            get { return frames; }
        }

        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }

        // width of a single sprite/frame in the used Spritesheet
        public int FrameWidth
        {
            get { return sprite.Width / (int)frames.X; }
        }

        // height of a single sprite/frame in the used Spritesheet
        public int FrameHeight
        {
            get { return sprite.Height / (int)frames.Y; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public Rectangle SourceRect
        {
            get { return srcRect; }
            set { srcRect = value; }
        }
        #endregion

        #region Constructor Region
        /// <summary>
        /// Create a new SpriteAnimation Objekt, an set it's default values
        /// </summary>
        /// <param name="Content">a ContentManager Instance</param>
        /// <param name="sprite">the Spritesheet</param>
        /// <param name="position">the Position of the Animation, in worldspace</param>
        /// <param name="frames">dimension of the Spritesheet: X amount of sprites in a row | Y amount of rows</param>
        /// <param name="switchFrame">time between animation frames</param>
        public SpriteAnimation(ContentManager Content, Texture2D sprite, Vector2 position, Vector2 frames, int switchFrame)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");

            frameCounter = 0;
            this.switchFrame = switchFrame;
            this.sprite = sprite;
            this.position = position;

            rotation = 0.0f;
            scale = 1.0f;
            isActive = false;
            alpha = 1.0f;
            drawColor = Color.White;

            this.frames = frames;
            currentFrame = Vector2.Zero;

            // if there is a spritesheet, claculate the scrRectangle for a sprite/frame
            if (sprite != null)
                srcRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
        #endregion

        #region Mono Method Region
        /// <summary>
        /// If IsActive is true, the current Animationframe gets changed every switchFrame milliseconds
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(IsActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;    // count time between frames
                if(frameCounter >= switchFrame)                                     
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * FrameWidth >= Sprite.Width)                // set currentFrame to the first frame if all frames
                        currentFrame.X = 0;                                         // have been displayed once (loop animation)
                }
            }
            else
                frameCounter = 0;       // render the first frame if IsActive is false
            
            // calculate the posistion of the currentFrame in the used SpriteSheet
            srcRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        /// <summary>
        /// Draw the currentFrame at his assigned position
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
            {
                origin = new Vector2(srcRect.Width / 2, srcRect.Height / 2);
                spriteBatch.Draw(sprite, position + origin, srcRect, drawColor * alpha, rotation, origin, new Vector2(scale, scale), SpriteEffects.None, 0.0f);
            }
        }
        #endregion
    }
}
