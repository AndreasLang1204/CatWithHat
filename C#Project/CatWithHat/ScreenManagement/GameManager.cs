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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace MMP1
{
    public class GameManager
    {
        #region Member Region
        private static GameManager instance;                      // ScreenManager instance (singleton)

        ContentManager content;                                   // creating custome contentManager
        Player player;
        GameScreen currentScreen;
        Song backgroundMusik;
        Song bossMusik;
        #endregion

        #region Property Region
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        public Vector2 ViewportDimensions { get; set; }                     // Screens width and height

        public ContentManager Content
        {
            get { return content; }
        }

        public Layer.Mode Gamemode { get; set; }
        public bool ExitGame { get; set; }
        public bool DrawPlayer { get; set; }
        #endregion

        #region Constructor Region
        private GameManager()
        {

        }
        #endregion

        #region Mono Method Region
        // sets the startscreen to Splashscreen
        public void Initialize()
        {
            currentScreen = new SplashScreen();
            DrawPlayer = true;
        }

        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
         
            Texture2D[] playerSprites = { content.Load<Texture2D>("catWalk"), content.Load<Texture2D>("catWalkHat") };
            player = new Player(content, playerSprites);
           
            // backgroundmusik
            backgroundMusik = content.Load<Song>("S31-City_on_Speed");
            bossMusik = content.Load<Song>("bossFightMusik");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusik);

            currentScreen.LoadContent(content, ref player);
        }

        public void Update(GameTime gameTime)
        {
            // calls the update method for the current screen
            currentScreen.Update(gameTime);

            // toogle mute sound
            if(InputManager.KeyPressed(Keys.M))
            {
                if (MediaPlayer.IsMuted)
                {
                    MediaPlayer.IsMuted = false;
                    player.IsMuted = false;
                }  
                else
                {
                    MediaPlayer.IsMuted = true;
                    player.IsMuted = true;
                }        
            }

            // update camera
            Camera.Instance.Update();  
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
           
            if (DrawPlayer)
                player.Draw(spriteBatch);
        }
        #endregion

        #region Method Region
        // switches currentScreen to screen
        public void SwitchScreen(GameScreen screen)
        {
            // load the content for the new screen
            screen.LoadContent(content, ref player);

            // change the BG musik if the new screen is the bossScreen
            if (screen.GetType() == typeof(BossScreen))
                MediaPlayer.Play(bossMusik);
            else
                MediaPlayer.Play(backgroundMusik);
     
            currentScreen = screen; 
        }
        #endregion
    }
}
