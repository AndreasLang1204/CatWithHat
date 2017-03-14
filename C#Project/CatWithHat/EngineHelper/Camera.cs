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
#endregion

namespace MMP1
{
    public class Camera
    {
        #region Member Region
        private static Camera instance;

        Vector2 position;
        Matrix viewMatrix;
        float mapWidth;
        #endregion

        #region Property Region
        public float MapWidth
        {
            set { mapWidth = value; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }
       
        // Singelton
        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();
                return instance; 
            }
        }
        #endregion

        #region Constructor Region
        private Camera() { }
        #endregion

        #region Method Region
        /// <summary>
        /// Sets the position of the camera, according to the player's position
        /// </summary>
        /// <param name="focalPosition"></param>
        public void SetFocalPoint(Vector2 focalPosition)
        {
            // camera begins to scroll if the player position is greater then half the viewport's width or height
            position = new Vector2(focalPosition.X - GameManager.Instance.ViewportDimensions.X / 2,
                                   focalPosition.Y - GameManager.Instance.ViewportDimensions.Y / 2);

            // prevents scrolling out of the map
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > mapWidth - GameManager.Instance.ViewportDimensions.X)   
                position.X = mapWidth - GameManager.Instance.ViewportDimensions.X;

            // only X axis needs to be checked -- there is no scrolling in Y direction 
        }

        /// <summary>
        /// Updates the viewMatrix of the camera
        /// </summary>
        public void Update()
        {
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
        #endregion
    }
}
