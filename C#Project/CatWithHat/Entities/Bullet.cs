/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/

#region Using Region
// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    public class Bullet
    {
        #region Member Region
        Texture2D sprite;
        float speed;
        #endregion

        #region Property Region
        public bool IsActive { get; set; }

        // BoxCollision for the bullet
        public FloatRect BoundingBox        
        {
            get { return new FloatRect(Position.X, Position.Y, sprite.Width * (1f/2f), sprite.Height); }
        }
        public int ActiveTime { get; set; }
        public int TotalActiveTime { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        #endregion

        #region Constructor Region
        /// <summary>
        ///  Creates a new Bullet Instance, and sets all members to their default values
        /// </summary>
        /// <param name="texture">the sprite for the bullet</param>
        /// <param name="position">the Position, in worldspace</param>
        /// <param name="direction">the direction of the bullet (left/right)</param>
        /// <param name="speed">the movementspeed of the bullet</param>
        /// <param name="activeTime">bullet lifetime</param>
        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, float speed, int activeTime)
        {
            sprite = texture;
            Position = position;
            Direction = direction;
            this.speed = speed;
            ActiveTime = activeTime;
            IsActive = false;

            TotalActiveTime = 0;
        }
        #endregion

        #region MonoGame Methods
        public void Update(GameTime gameTime)
        {
            // update bullet if IsActive
            if (IsActive)
            {
                Position += Direction * speed;
                TotalActiveTime += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw bullet if IsActive
            if(IsActive)
                spriteBatch.Draw(sprite, Position, Color.White);
        }
        #endregion
    }
}
