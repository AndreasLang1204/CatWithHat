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
using Microsoft.Xna.Framework.Input;
#endregion

namespace MMP1
{
    public static class InputManager
    {
        #region Member Region
        static KeyboardState lastKeyState;              // KeyboardState of prev. Frame
        static KeyboardState currentKeyState;           // current KeyboardState
        #endregion

        #region Property Region
        public static KeyboardState LastKeyState
        {
            get { return lastKeyState; }
            set { lastKeyState = value; }
        }

        public static KeyboardState CurrentKeyState
        {
            get { return currentKeyState; }
            set { currentKeyState = value; }
        }
        #endregion

        #region Mono Method Region
        // Update KeyboardStates
        // lastState = currentState of prev. Frame
        // currentState = keyboardState of active frame
        public static void Update()
        {
            lastKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }
        #endregion

        #region Method Region
        /// <summary>
        /// Checks if the given key is pressed
        /// </summary>
        /// <param name="key">Keyboard Key to check</param>
        /// <returns>true, if the key wasn't pressed in the last frame, but is pressed now</returns>
        public static bool KeyPressed(Keys key)
        {
            if (currentKeyState.IsKeyDown(key) && lastKeyState.IsKeyUp(key))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if any of the given keys is pressed
        /// </summary>
        /// <param name="key">Keyboard Keys to check</param>
        /// <returns>true, if any of the keys wasn't pressed in the last frame, but is pressed now</returns>
        public static bool KeyPressed(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && lastKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the given key is released
        /// </summary>
        /// <param name="key">Keyboard Key to check</param>
        /// <returns>true, if the key was pressed in the last frame, but is not pressed now</returns>
        public static bool KeyReleased(Keys key)
        {
            if (currentKeyState.IsKeyUp(key) && lastKeyState.IsKeyDown(key))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if any of the given keys is released
        /// </summary>
        /// <param name="key">Keyboard Keys to check</param>
        /// <returns>true, if any of the keys was pressed in the last frame, but is not pressed now</returns>
        public static bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && lastKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the given key is down
        /// </summary>
        /// <param name="key">Keyboard Key to check</param>
        /// <returns>true, if the key is pressed</returns>
        public static bool KeyDown(Keys key)
        {
            if (currentKeyState.IsKeyDown(key))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if any of the given keys is down
        /// </summary>
        /// <param name="key">Keyboard Keys to check</param>
        /// <returns>true, if any of the keys is pressed</returns>
        public static bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }
        #endregion
    }
}
