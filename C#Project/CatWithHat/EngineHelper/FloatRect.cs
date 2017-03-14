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

namespace MMP1
{
    public struct FloatRect
    {
        #region Member Region
        float top;
        float bottom;
        float left;
        float right;
        #endregion

        #region Property Region
        public float Top
        {
            get { return top; }
        }

        public float Bottom
        {
            get { return bottom; }
        }

        public float Left
        {
            get { return left; }
        }

        public float Right
        {
            get { return right; }
        }
        #endregion

        #region Constructor Region
        public FloatRect(float x, float y, float width, float height)
        {
            left = x;
            right = x + width;
            top = y;
            bottom = y + height;
        }
        #endregion

        #region Method Region
        /// <summary>
        /// Check if the given FloatRectangle intersects with this
        /// </summary>
        /// <param name="rect">the other FloatRect for testing</param>
        /// <returns>true if the Rectangles intersect</returns>
        public bool Intersects(FloatRect rect)
        {
            if (right <= rect.Left || left >= rect.Right || top >= rect.Bottom || bottom <= rect.Top)
                return false;
            return true;
        }
        #endregion
    }
}
