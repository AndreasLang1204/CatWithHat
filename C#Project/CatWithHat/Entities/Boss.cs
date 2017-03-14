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
#endregion

namespace MMP1
{
    public class Boss : Entity
    {
        #region Const Region
        const int decreaseWaterLevel = 7;        // amount off bullets boss can fire before his water level drops
        const float bulletSpeed = 6.0f;
        const int bulletActiveTime = 3000;     // time in milliseconds
        const int timeBetweenShots = 500;      // time in milliseconds
        #endregion

        #region Member Region
        int bulletsLeft;
        int xFrame = -1;
        int shotTimer = 0;
        bool switchAnimationFlag = true;

        Texture2D bulletSprite;
        Bullet[] bullets;
        #endregion

        #region Property Region
        public Bullet[] Bullets
        {
            get { return bullets; }
        }

        public bool IsDefeted { get;  set; }
        #endregion

        #region Constructor Region
        /// <summary>
        /// Creates a new Boss Instance, and sets all members to their default values
        /// </summary>
        /// <param name="content">a ContentManager Instance</param>
        /// <param name="position">the Position, in worldspace</param>
        /// <param name="enemySpriteSheet">Spritesheet</param>
        /// <param name="bulletSprite">Sprite which will be used for the bullets</param>
        /// <param name="nrOfBullets">amount of bullets the boss can fire</param>
        public Boss(ContentManager content, Vector2 position ,Texture2D enemySpriteSheet, Texture2D bulletSprite, byte nrOfBullets)
        {
            this.sprite = enemySpriteSheet;
            this.bulletSprite = bulletSprite;
            Random rnd = new Random();

            // create a new SpriteAnimation
            moveAnimation = new SpriteAnimation(content, enemySpriteSheet, position, new Vector2(4, 2), 300);
            bullets = new Bullet[nrOfBullets];

            // create the bullets for the boss, each bullet gets a random Y position
            // the start position of each bullet is the center of the boss + the random Y modifiere 
            for(int bulletIdx = 0; bulletIdx < bullets.Length; bulletIdx++)
            {
                float rndYPos = 0;
                switch (rnd.Next(5))
                {
                    case 0:
                        rndYPos = (position.Y + (moveAnimation.FrameHeight / 2) - (3 * bulletSprite.Height));
                        break;

                    case 1:
                        rndYPos = (position.Y + (moveAnimation.FrameHeight / 2) - bulletSprite.Height);
                        break;

                    case 2:
                        rndYPos = (position.Y + (moveAnimation.FrameHeight / 2));
                        break;

                    case 3:
                        rndYPos = (position.Y + (moveAnimation.FrameHeight / 2) + bulletSprite.Height);
                        break;

                    case 4:
                        rndYPos = (position.Y + (moveAnimation.FrameHeight / 2) + (1.5f * bulletSprite.Height));
                        break;
                }

                bullets[bulletIdx] = new Bullet(bulletSprite, new Vector2(position.X, rndYPos),
                                      new Vector2(-1, 0), bulletSpeed, bulletActiveTime);
            }
            bulletsLeft = bullets.Length;

            syncTilePos = false;
            gravityActive = false;          // boss is not influenced by gravity
            this.position = position;
            velocity = Vector2.Zero;        // boss doesn't move
        }
        #endregion

        #region Mono Method Region
        public override void Update(GameTime gameTime, Layer layer)
        {
            base.Update(gameTime, layer);
           
            // if boss has bullets left
            if (bulletsLeft > 0)
            {
                // count time between two bullets
                shotTimer += gameTime.ElapsedGameTime.Milliseconds;

                // fire a bullet if shotTimer is greater than timeBetweenShots
                if (shotTimer > timeBetweenShots)
                {
                    shotTimer = 0;

                    // change boss sprite every "decreaseWaterLvl" shots
                    if (bulletsLeft % decreaseWaterLevel == 0)
                    {
                        xFrame++;
                        if (xFrame > moveAnimation.Frames.X - 1)
                            xFrame = (int)moveAnimation.Frames.X - 1;
                    }

                    moveAnimation.CurrentFrame = new Vector2(xFrame, 0);

                    bulletsLeft--;              
                    if (bulletsLeft < 0)
                        bulletsLeft = 0;
                    bullets[bulletsLeft].IsActive = true;       // set the bullet to IsActive
                }
            }
            // when boss has no more bullets
            else
            {
                // execute only once
                // play boss death animation
                if (switchAnimationFlag)
                {
                    moveAnimation.CurrentFrame = new Vector2(0, 1);
                    moveAnimation.IsActive = true;
                    switchAnimationFlag = false;
                }

                // when the death animation is finished
                if (moveAnimation.CurrentFrame.X == 3)
                {
                    moveAnimation.IsActive = false;
                    IsDefeted = true;
                }
            }

            // update each bullet
            for (int bulletIdx = bulletsLeft; bulletIdx < bullets.Length; bulletIdx++)
            {
                bullets[bulletIdx].Update(gameTime);

                // when a bullet exceeds its active time, set IsActive to false
                if (bullets[bulletIdx].TotalActiveTime > bullets[bulletIdx].ActiveTime)
                     bullets[bulletIdx].IsActive = false;
            }

            moveAnimation.Position = position;
            moveAnimation.Update(gameTime);  
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw active bullets
            foreach(Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // draw boss sprite
            moveAnimation.Draw(spriteBatch);
        }
        #endregion
    }
}
