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
*  add Mode Enum                                                          *
*      GenerateLevel Methode                                              *
*      SplitTilesetInSrcRects Methode                                     *
***************************************************************************/

#region Using Region
using System;
using System.Collections.Generic;

// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    public class Layer
    {
        #region Enum Region
        public enum Mode { Easy, Normal, Hard };
        #endregion

        #region Member Region
        List<Tile> tiles;
        Rectangle[] tilesSrcRects;      // the position of each tile in the used tileset
        List<string> solid;
        List<string> goal;
        Vector2 layerDimensions;

        ContentManager content;
        FileManager fileManager;
        Texture2D tileSheet;
        #endregion

        #region Property Region
        public float LayerWidth
        {
            get { return layerDimensions.X; }
        }

        public float LayerHeight
        {
            get { return layerDimensions.Y; }
        }

        public static Vector2 TileDimensions
        {
            get { return new Vector2(32, 32); }
        }

        public Texture2D TileSet
        {
            get { return tileSheet; }
        }

        public Rectangle GetTileSrcRect(int index)
        {
            if (index < 0 || index > tilesSrcRects.Length)
                throw new IndexOutOfRangeException();

            return tilesSrcRects[index];
        }
        #endregion

        #region Method Region
        // load the layer information from a file
        public void LoadContent(ContentManager Content, Map map, Vector2 tileDimensions, string layerID)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            fileManager = new FileManager();

            tiles = new List<Tile>();
            solid = new List<string>();
            goal = new List<string>();

            string nullTile = "";

            fileManager.LoadContent("Content/" + map.ID + ".cwh", layerID);       // loads layer from file
            int yIndex = 0;

            // loop through each attribute
            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "LayerSize":
                            string[] split = fileManager.Contents[i][j].Split(',');
                            layerDimensions = new Vector2(float.Parse(split[0]) * tileDimensions.X, float.Parse(split[1]) * tileDimensions.Y);
                            break;

                        case "TileSet":
                            tileSheet = content.Load<Texture2D>(fileManager.Contents[i][j]);
                            SplitTilesetInSrcRects();
                            break;

                        case "Goal":
                            goal.Add(fileManager.Contents[i][j]);
                            break;

                        case "Solid":
                            solid.Add(fileManager.Contents[i][j]);
                            break;

                        case "Motion":
                            break;

                        case "NullTile":
                            nullTile = fileManager.Contents[i][j];
                            break;

                        case "StartLayer":
                           
                            Tile.State tempState;

                            // load the information foreach tile of the layer
                            for (int k = 0; k < fileManager.Contents[i].Count; k++)
                            {
                                if (fileManager.Contents[i][k] != nullTile) // set nothing if null tile
                                { 
                                    int tilesetIdx = int.Parse(fileManager.Contents[i][k]);
                                    tiles.Add(new Tile(this));

                                    if (solid.Contains(fileManager.Contents[i][k]))
                                        tempState = Tile.State.Solid;
                                    else if (goal.Contains(fileManager.Contents[i][k]))
                                        tempState = Tile.State.Goal;
                                    else
                                        tempState = Tile.State.Passive;

                                    // set the information for the current tile
                                    tiles[tiles.Count - 1].SetTile(tempState, new Vector2(k * 32, yIndex * 32), GetTileSrcRect(tilesetIdx));
                                }
                            }
                            yIndex++;
                            break;
                    }
                }
            }
            
        }

        /// <summary>
        /// Generate a new random Layer
        /// </summary>
        /// <param name="width">the width of the layer</param>
        /// <param name="height">the height of the layer</param>
        /// <param name="tileDimensions">the dimension of the tiles (32,32) by default</param>
        /// <param name="tileSet">the used tileset for the layer</param>
        /// <param name="rnd">Random Generator</param>
        /// <param name="mode">difficulty level</param>
        /// <param name="Content"></param>
        public void GenerateLevel(int width, int height, 
                                  Vector2 tileDimensions, Texture2D tileSet, 
                                  Random rnd, Mode mode, 
                                  ContentManager Content)
        {
            layerDimensions = new Vector2(width * tileDimensions.X, height * tileDimensions.Y);
            tiles = new List<Tile>();
            tileSheet = tileSet;
            SplitTilesetInSrcRects();
            float actWidth = 0;

            #region start platform, the same for every level
            for (float widthIdx = tileDimensions.X; widthIdx < 5 * tileDimensions.X; widthIdx += tileDimensions.X)
            {
                tiles.Add(new Tile(this));
                if(widthIdx == tileDimensions.X)
                    tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(widthIdx, layerDimensions.Y * (3.0f/4)), GetTileSrcRect(6));
                else if(widthIdx == tileDimensions.X * 4)
                    tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(widthIdx, layerDimensions.Y * (3.0f / 4)), GetTileSrcRect(7));
                else
                    tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(widthIdx, layerDimensions.Y * (3.0f / 4)), GetTileSrcRect(4));

                actWidth = widthIdx;
            }
            #endregion

            int voidCnt = 0;
            float yMod;
            int itemCnt = 0;

            // create random platforms between start/end floor
            while (actWidth < layerDimensions.X - (8 * tileDimensions.X))
            {
                int tmp = rnd.Next(2);

                // draw a platform if tmp is 0 or if there was no platform for the last 8 tiles
                // and there where at least 5 tiles without a platform
                if ((tmp == 0 || voidCnt > 8)  && voidCnt >= 5)
                {
                    Vector2 lastTile = tiles[tiles.Count - 1].Position;     // last drawn Tile
                    tiles.Add(new Tile(this));      // new Tile

                    // modify Y position
                    if (rnd.NextDouble() >= 0.5f)
                        yMod = lastTile.Y + (tileDimensions.X * 3);
                    else
                        yMod = lastTile.Y - (tileDimensions.X * 5);

                    // check if Y position is vaild (within the viewport)
                    if (yMod > (layerDimensions.Y - tileDimensions.Y))
                        yMod = layerDimensions.Y - tileDimensions.Y;
                    else if (yMod < (layerDimensions.Y / 3))
                        yMod = (layerDimensions.Y / 3);

                    #region Mode
                    // the mode determines how large the platforms are
                    // Easy ----> platfrom is 3 tiles large
                    // Normal --> platform is 2 tiles large
                    // Hard ----> platform is 1 tile large
                    if (mode == Mode.Hard)
                        tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(actWidth, yMod), GetTileSrcRect(4));
                    else
                        tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(actWidth, yMod), GetTileSrcRect(6));

                    actWidth += tileDimensions.X;

                    if (mode == Mode.Easy)
                    {
                        tiles.Add(new Tile(this));
                        tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(actWidth, yMod), GetTileSrcRect(4));
                        actWidth += tileDimensions.X;
                    }

                    if(mode == Mode.Normal || mode == Mode.Easy)
                    {
                        tiles.Add(new Tile(this));
                        tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(actWidth, yMod), GetTileSrcRect(7));
                    }
                    #endregion

                    #region draw poles for platforms
                    //if (yMod < layerDimensions.Y - tileDimensions.Y)
                    //{
                    //    for (float y = yMod + tileDimensions.Y; y < layerDimensions.Y; y += tileDimensions.Y)
                    //    {
                    //        tiles.Add(new Tile(this));
                    //        tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(actWidth, y), GetTileSrcRect(5));
                    //    }
                    //}                   
                    #endregion

                    actWidth = tiles[tiles.Count - 1].Position.X;

                    #region spawn item at the half of the level
                    if (actWidth >= (layerDimensions.X / 2) && itemCnt < 1)
                    {
                        tiles.Add(new Tile(this));
                        tiles[tiles.Count - 1].SetTile(Tile.State.Item, new Vector2(actWidth, yMod - tileDimensions.Y), GetTileSrcRect(3));
                        itemCnt++;
                    }
                    #endregion

                    voidCnt = 0;        // reset void count
                }
                else
                {
                    voidCnt++;          // increase void count if no platform has been drawn 
                    actWidth += tileDimensions.X;
                }
              
            }

            // set goal
            tiles.Add(new Tile(this));
            tiles[tiles.Count - 1].SetTile(Tile.State.Goal, new Vector2(layerDimensions.X - tileDimensions.X, layerDimensions.Y * (3.0f / 4)), GetTileSrcRect(1));

            #region end platform, the same for every level
            for (float widthIdx = layerDimensions.X - (5 * tileDimensions.X); widthIdx < layerDimensions.X; widthIdx += tileDimensions.X)
            {
                tiles.Add(new Tile(this));
                if (widthIdx == layerDimensions.X - (5 * tileDimensions.X))
                    tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(widthIdx, layerDimensions.Y * (3.0f / 4) + tileDimensions.Y), GetTileSrcRect(6));
                else if (widthIdx == layerDimensions.X - tileDimensions.X)
                    tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(widthIdx, layerDimensions.Y * (3.0f / 4) + tileDimensions.Y), GetTileSrcRect(7));
                else
                    tiles[tiles.Count - 1].SetTile(Tile.State.Solid, new Vector2(widthIdx, layerDimensions.Y * (3.0f / 4) + tileDimensions.Y), GetTileSrcRect(4));
                actWidth = widthIdx;
            }
            #endregion
        }

        /// <summary>
        /// calculate the position of each tile in the tileset and save it
        /// </summary>
        public void SplitTilesetInSrcRects()
        {
            int numberOfTilesInRow = (int)(tileSheet.Width / TileDimensions.X);
            int numberOfRows = (int)(tileSheet.Height / TileDimensions.Y);

            tilesSrcRects = new Rectangle[numberOfTilesInRow * numberOfRows];
            int tileCnt = 0;

            for (int currentRow = 0; currentRow < numberOfRows; currentRow++)
            {
                for (int tileNr = 0; tileNr < numberOfTilesInRow; tileNr++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = (int)TileDimensions.X;
                    rect.Height = (int)TileDimensions.Y;
                    rect.X = tileNr * (int)TileDimensions.X;
                    rect.Y = currentRow * (int)TileDimensions.Y;

                    tilesSrcRects[tileCnt] = rect;
                    tileCnt++;
                }
            }
            
        }

        public void UnloadContent()
        {
            tiles.Clear();
            fileManager = null;
        }
        #endregion

        #region Mono Mehtod Region
        public void UpdateCollision(ref Entity entity)
        {
            // update collision for all actual tiles
            for (int tileIdx = 0; tileIdx < tiles.Count; tileIdx++)               
            {          
                tiles[tileIdx].UpdateCollision(ref entity);    
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw all actual tiles
            for (int tileIdx = 0; tileIdx < tiles.Count; tileIdx++)               
            {
                 tiles[tileIdx].Draw(spriteBatch);           
            }   
        }
        #endregion
    }
}
