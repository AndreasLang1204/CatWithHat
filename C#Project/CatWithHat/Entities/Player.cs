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
*                                                                         *
*  add DoubleJump                                                         *
*      AutoRun Mechanik                                                   *
*      Hat Counter                                                        *
*      ChangePlayerSprite Methode                                         *
*      Bullet Kollision                                                   *
***************************************************************************/

#region Using Region
using System;

// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace MMP1
{
    public class Player : Entity
    {
        #region Const Region
        const float defaultGravity = 80f;
        const float defaultMoveSpeed = 300f;
        const float defaultDoubleJumpHeight = 1000f;
        const float defaultJumpHeight = 1500f;
        #endregion
        #region Member Region
        SoundEffect jumpSound;
        SoundEffect deathSound;
        Texture2D[] playerSprites;

        int doubleJumpFlag;
        int hatCnt;
        float doubleJumpHeight;
        #endregion

        #region Property Region
        public uint Deaths { get; set; }
        public Vector2 ResetPosition { get; set; }
        public bool RunFlag {get; set;}
        public int StageNr { get; set; }
        public bool AutoRun { get; set; }
        public bool GotHit { get; set; }

        public int HatCnt
        {
            get { return hatCnt; }
            set
            {
                hatCnt = value;
                if (hatCnt < 0)
                    hatCnt = 0;
            }
        }
        #endregion

        #region Constructor Region
        /// <summary>
        /// Creates a new Player Instance, and sets all members to their default values
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sprites">Spritesheets used for the playercharacter</param>
        public Player(ContentManager content, params Texture2D[] sprites)
        {
            playerSprites = sprites;
        
            gravity = defaultGravity;
            moveSpeed = defaultMoveSpeed;
            doubleJumpHeight = defaultDoubleJumpHeight;
            jumpHeight = defaultJumpHeight;
            velocity = Vector2.Zero;
            position = new Vector2(64, 32);

            syncTilePos = false;
            gravityActive = true;
            StageCleare = false;
            doubleJumpFlag = 0;
            RunFlag = false;
           
            HatCnt = 0;
            StageNr = 0;
            IsMuted = false;

            deathSound = content.Load<SoundEffect>("deathSound");
            jumpSound = content.Load<SoundEffect>("jumping");
            moveAnimation = new SpriteAnimation(content, playerSprites[0], position, new Vector2(19,2), 3);
        }
        #endregion

        #region Mono Method Region
        // Update Mehtod when player has full controll
        public override void Update(GameTime gameTime, Layer layer)
        {
            base.Update(gameTime, layer);
            moveAnimation.IsActive = true;
            moveAnimation.DrawColor = Color.White;

            // move right
            if (InputManager.KeyDown(Keys.Right, Keys.D))
            {
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0); 
            }
            // move left
            else if (InputManager.KeyDown(Keys.Left, Keys.A))
            {
                velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);  
            }
            else 
                velocity.X = 0;

            // jump & double jump
            if (InputManager.KeyPressed(Keys.Up, Keys.W, Keys.Space) && doubleJumpFlag < 2)
            {
                if(!IsMuted)
                    jumpSound.Play();

                if (doubleJumpFlag < 1)
                    velocity.Y = -jumpHeight * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.Y = -doubleJumpHeight * (float)gameTime.ElapsedGameTime.TotalSeconds;

                doubleJumpFlag++;
                gravityActive = true;
            
            }

            // check if player should be influenced by gravity
            if (gravityActive)
            {
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                velocity.Y = 0;
                doubleJumpFlag = 0;
            }

            // update player position 
            position += velocity;

            // if player falls of the screen on the Y-Axis
            if (position.Y >= layer.LayerHeight)    
                Death();

            moveAnimation.Position = position;
            moveAnimation.Update(gameTime);

            //Camera
            Camera.Instance.SetFocalPoint(new Vector2(position.X, GameManager.Instance.ViewportDimensions.Y / 2));
        }

        // Update Method for autoscrolling levels
        public void UpdateAutoRun(GameTime gameTime, Layer layer)
        {
            base.Update(gameTime, layer);
            moveAnimation.IsActive = true;
            moveAnimation.DrawColor = Color.White;

            // press "Right" or "D" once to start, then auto movement
            if (InputManager.KeyDown(Keys.Right, Keys.D) || RunFlag)          
            {
                RunFlag = true;
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
            }
            else
                velocity.X = 0;

            // jump & double jump
            if (InputManager.KeyPressed(Keys.Up, Keys.W, Keys.Space) && doubleJumpFlag < 2)
            {
                if(!IsMuted)
                    jumpSound.Play();

                if (doubleJumpFlag < 1)
                    velocity.Y = -jumpHeight * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.Y = -doubleJumpHeight * (float)gameTime.ElapsedGameTime.TotalSeconds;
                doubleJumpFlag++;
                gravityActive = true;
            }

            // check if player should be influenced by gravity
            if (gravityActive)
            {
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                velocity.Y = 0;
                doubleJumpFlag = 0;
            }

            // update player position 
            position += velocity;

            // if player falls of the screen on the Y-Axis
            if (position.Y >= layer.LayerHeight)
                Death();

            moveAnimation.Position = position;
            moveAnimation.Update(gameTime);

            //Camera
            Camera.Instance.SetFocalPoint(new Vector2(position.X, GameManager.Instance.ViewportDimensions.Y / 2));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
        }
        #endregion

        #region Method Region
        // Change the spritesheet which will be used to draw the player
        public void ChangePlayerSprite(int idx)
        {
            if (idx < 0 || idx >= playerSprites.Length)
                throw new IndexOutOfRangeException();

            moveAnimation.Sprite = playerSprites[idx];
        }

        // check if the player got hit by any of the given bullets
        public void BulletCollision(Bullet[] bullets)
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet.IsActive && BoundingBox.Intersects(bullet.BoundingBox))
                {
                    GotHit = true;
                    moveAnimation.DrawColor = Color.Red;
                }
            }
        }

        // on death, play lose sound, increase death count and reset the player position
        private void Death()
        {
            if(!IsMuted)
                deathSound.Play();

            Deaths++;
            moveAnimation.DrawColor = Color.Red;
            RunFlag = false;
            position = ResetPosition;  
        }
        #endregion
    }
}
