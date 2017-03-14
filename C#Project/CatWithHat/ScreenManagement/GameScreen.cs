/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/
/**************************************************************************
* Idee für die Funktion der Klasse wurde aus dem                          *
* “XNA Platformer” ­Tutorial von CodeingMadeEasy übernommen.               *
* (https://www.youtube.com/watch?v=FR7crO2xq8A&list=PLE500D63CA505443B)   *
*                                                                         *
* Den Großteil der Funktionsweise habe ich dann selbst implementiert      *
***************************************************************************/

#region Using Region
// MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MMP1
{
    // base class for all Screens
    public class GameScreen
    {
        #region Member Region
        protected ContentManager content;
        protected Player player;
        protected Map map;
        protected Background background;
        protected SpriteFont font;
        #endregion

        #region Property Region
        public Map Map
        {
            get { return map; }
            set { map = value; }
        }
        #endregion

        #region Mono Method Region
        public virtual void LoadContent(ContentManager Content, ref Player player)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            this.player = player;
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }  
        #endregion
    }
}
