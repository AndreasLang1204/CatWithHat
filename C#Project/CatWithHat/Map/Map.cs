/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/
/***************************************************************************
* Implementierungsgrundlage für die Klasse wurde aus dem                   *
* “XNA Platformer” ­Tutorial von CodeingMadeEasy übernommen. Und dann von   *
* mir auf meine Anforderungen angepasst.                                   *     
* (https://www.youtube.com/watch?v=FR7crO2xq8A&list=PLE500D63CA505443B)    *
*                                                                          *
*  add: LoadContent - überladen damit random Levels erstellt werden können *
****************************************************************************/

#region Using Region
using System;

// MonoGame
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    public class Map
    {
        #region Const Region
        const int defaultLevelHeight = 16;
        #endregion
        #region Member Region
        string id;
        #endregion

        #region Property Region
        public Layer Layer { get; set; }
        public string ID
        {
            get { return id; }
        }
        #endregion

        #region Method Region
        // load layer from file
        public void LoadContent(ContentManager Content, string mapID)
        {
            Layer = new Layer();
            id = mapID;
            Layer.LoadContent(Content, this, Layer.TileDimensions, "Layer1");
            
            Camera.Instance.MapWidth = Layer.LayerWidth;
        }

        // create layer for random level
        public void LoadContent(ContentManager Content, Random rnd, int rndLevelWitdh, Layer.Mode mode)
        {
            Layer = new Layer();
            Layer.GenerateLevel(rndLevelWitdh, 
                                defaultLevelHeight, 
                                Layer.TileDimensions, 
                                Content.Load<Texture2D>("SideTileset"), 
                                rnd, 
                                mode, 
                                Content);

            Camera.Instance.MapWidth = Layer.LayerWidth;
        }

        public void UnloadContent()
        {
            Layer.UnloadContent();
        }
        #endregion

        #region Mono Method Region
        public void UpdateCollision(ref Entity entity)
        {
            Layer.UpdateCollision(ref entity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Layer.Draw(spriteBatch);
        }
        #endregion
    }
}
