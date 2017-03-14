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
using System.Collections.Generic;

// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    // Base Class for all Characters
    public class Entity
    {
        #region Member Region
        protected int health;
        protected SpriteAnimation moveAnimation;

        protected float moveSpeed;
        protected float jumpHeight;

        protected Texture2D sprite;
        protected Vector2 position;
        protected Rectangle collisionBox;
        protected float gravity;
        protected Vector2 velocity;
        protected Vector2 prevPosition;
        protected Vector2 destPosition;
        protected Vector2 origPosition;
        protected int range;
        protected int direction;

        protected bool gravityActive;
        protected bool syncTilePos;
        protected bool onTile;

        protected ContentManager content;

        protected List<List<string>> attributes, contents;
        #endregion

        #region Property Region
        // set BoxCollision for Entity
        public FloatRect BoundingBox
        {
            get { return new FloatRect(position.X, position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight); }
        }

        public Vector2 PrevPosition
        {
            get { return prevPosition; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public int Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                destPosition.X = (direction == 2) ? destPosition.X = origPosition.X - range : destPosition.X = origPosition.X + range;
            }
        }

        public bool OnTile
        {
            get { return onTile; }
            set { onTile = value; }
        }

        public bool GravityActive
        {
            get { return gravityActive; }
            set { gravityActive = value; }
        }

        public bool SyncTilePos
        {
            get { return syncTilePos; }
            set { syncTilePos = value; }
        }

        public SpriteAnimation Animation
        {
            get { return moveAnimation; }
        }

        public bool StageCleare { get; set; }
        public bool IsMuted { get; set; }
        #endregion

        #region Mono Method Region
        /// <summary>
        /// Used to Load tne Entity information from an file
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="attributes"></param>
        /// <param name="contents"></param>
        public virtual void LoadContent(ContentManager Content, List<string> attributes, List<string> contents)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            Vector2 tempFrames = Vector2.Zero;
           
            for (int j = 0; j < attributes.Count; j++)
            {
                switch (attributes[j])
                {
                    case "Health":
                        health = int.Parse(contents[j]);
                        break;

                    case "Frames":
                        string[] tempFrame = contents[j].Split(',');
                        tempFrames = new Vector2(int.Parse(tempFrame[0]), int.Parse(tempFrame[1]));
                        break;

                    case "Sprite":
                        sprite = content.Load<Texture2D>(contents[j]);
                        break;

                    case "Position":
                        string[] temp = contents[j].Split(',');
                        position = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
                        break;

                    case "MoveSpeed":
                        moveSpeed = float.Parse(contents[j]);
                        break;

                    case "Range":
                        range = int.Parse(contents[j]);
                        break;
                }
            }

            gravity = 80f;
            velocity = Vector2.Zero;
            syncTilePos = false;
            gravityActive = true;
            jumpHeight = 1500f;
            StageCleare = false;

            moveAnimation = new SpriteAnimation(content, sprite, position, tempFrames, 3);
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, Layer layer)
        {
            syncTilePos = false;
            prevPosition = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
        #endregion
    }
}
