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
*  überarbeitet:                                                          *
*      SetTile Methode                                                    *
*      UpdateCollision Methode                                            *
***************************************************************************/

#region Using Region
// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    public class Tile
    {
        #region Enum Region
        public enum State { Solid, Passive, Item, Goal };
        #endregion

        #region Member Region
        State state;            // the State of the Tile
        Layer layer;            // the Layer the Tile belongs to

        Vector2 position;
        Rectangle srcRect;      // defines wich tile from the coresponding tilesheet should be drawn for this tile

        bool containsEntity;
        #endregion

        #region Property Region
        public Vector2 Position
        {
            get { return position; }
        }

        public Tile(Layer layer)
        {
            this.layer = layer;
        }
        #endregion

        #region Method Region
        // set State, position and srcRect for the Tile
        public void SetTile(State state, Vector2 position, Rectangle tileArea)
        {
            this.state = state;
            this.position = position;
            srcRect = tileArea;
            containsEntity = false;
        }

        public void UpdateCollision(ref Entity entity)
        {
            // Entity Collision
            FloatRect tileBoundingBox = new FloatRect(position.X, position.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            if (entity.OnTile && containsEntity)
            {
                if (!entity.SyncTilePos)
                {
                    entity.SyncTilePos = true;
                }

                // activate gravity if player runs of a platform
                if (entity.BoundingBox.Right >= tileBoundingBox.Left - entity.Animation.FrameWidth || entity.BoundingBox.Left >= tileBoundingBox.Right || entity.BoundingBox.Bottom != tileBoundingBox.Top)
                {
                    entity.OnTile = false;
                    containsEntity = false;
                    entity.GravityActive = true;
                }
            }

            // end of level
            if (entity.BoundingBox.Intersects(tileBoundingBox) && state == State.Goal) 
            {
                entity.StageCleare = true;
            }

            // Item collision, increase player.hatcnt and set tile state to passive
            if (entity.BoundingBox.Intersects(tileBoundingBox) && state == State.Item) 
            {
                Player p = (Player)entity;
                p.HatCnt++;
                entity = p;

                srcRect = layer.GetTileSrcRect(0);
                state = State.Passive;
            }

            // collision
            if (entity.BoundingBox.Intersects(tileBoundingBox) && state == State.Solid) 
            {
                // get the boundingbox of the prev entity position
                FloatRect prevEntity = new FloatRect(entity.PrevPosition.X, entity.PrevPosition.Y, entity.Animation.FrameWidth, entity.Animation.FrameHeight);

                // bottom
                if (entity.BoundingBox.Bottom >= tileBoundingBox.Top && prevEntity.Bottom <= tileBoundingBox.Top)  
                {
                    entity.Position = new Vector2(entity.Position.X, position.Y - entity.Animation.FrameHeight);
                    entity.GravityActive = false;
                    entity.OnTile = true;
                    containsEntity = true;

                }
                // top
                else if (entity.BoundingBox.Top <= tileBoundingBox.Bottom && prevEntity.Top <= tileBoundingBox.Bottom) 
                {
                    entity.Position = new Vector2(entity.Position.X, position.Y + Layer.TileDimensions.Y);
                    entity.Velocity = new Vector2(entity.Position.X, 0);
                    entity.GravityActive = true;

                }

                // left right
                if (entity.BoundingBox.Right >= tileBoundingBox.Left && prevEntity.Right <= tileBoundingBox.Left ||
                    entity.BoundingBox.Left <= tileBoundingBox.Right && prevEntity.Left >= tileBoundingBox.Right) 
                {
                    entity.Position = new Vector2(entity.PrevPosition.X, entity.Position.Y);
                    if (entity.Direction == 1)
                        entity.Direction = 2;
                    else
                        entity.Direction = 1;
                }
            }
            entity.Animation.Position = entity.Position;
        }
        #endregion

        #region Mono Method Region
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(layer.TileSet, position, srcRect, Color.White);
        }
        #endregion
    }
}
